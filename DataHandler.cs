using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RealWorldPrediction
{
    /// <summary>
    /// Handles loading, parsing, normalization, and train-test splitting
    /// of the Cleveland Heart Disease dataset.
    ///
    /// Dataset: UCI Cleveland Heart Disease (303 raw samples, 13 features;
    ///          297 samples remain after removing records with missing values)
    /// Task:    Binary classification — Heart Disease (1) vs No Disease (0)
    ///
    /// Preprocessing order is deliberately leak-free:
    ///   1. Parse the raw CSV into <see cref="AllFeatures"/> / <see cref="AllLabels"/>.
    ///   2. Shuffle and split the RAW rows into train/validation/test.
    ///   3. One-hot encode the categorical features (cp, restecg, slope, thal).
    ///   4. Fit the MinMax min/max values on the TRAIN split only.
    ///   5. Transform all three splits with those train-derived parameters.
    ///
    /// The held-out portion (100% - trainRatio) is divided evenly between
    /// validation and test, so the default 70% train ratio gives a 70/15/15 split.
    ///
    /// Encoding: 9 numeric/binary features are kept as-is, while cp (4 levels),
    /// restecg (3), slope (3), and thal (3) are one-hot encoded. That produces
    /// <see cref="EncodedFeatureCount"/> = 22 network inputs from 13 raw features.
    ///
    /// Features:
    ///   0. Age              (continuous, 29-77)
    ///   1. Sex              (binary: 1=Male, 0=Female)
    ///   2. Chest Pain Type  (categorical: 1=Typical Angina, 2=Atypical Angina, 3=Non-Anginal, 4=Asymptomatic)
    ///   3. Resting BP       (continuous, 94-200 mmHg)
    ///   4. Cholesterol      (continuous, 126-564 mg/dl)
    ///   5. Fasting BS >120  (binary: 1=True, 0=False)
    ///   6. Resting ECG      (categorical: 0=Normal, 1=ST-T Abnormality, 2=LV Hypertrophy)
    ///   7. Max Heart Rate   (continuous, 71-202 bpm)
    ///   8. Exercise Angina  (binary: 1=Yes, 0=No)
    ///   9. ST Depression    (continuous, 0.0-6.2)
    ///  10. Slope            (categorical: 1=Upsloping, 2=Flat, 3=Downsloping)
    ///  11. Major Vessels    (0-3)
    ///  12. Thal             (categorical: 3=Normal, 6=Fixed Defect, 7=Reversible Defect)
    /// </summary>
    public class DataHandler
    {
        // --- Feature Names for the UI ---
        public static readonly string[] FeatureNames =
        {
            "Age",
            "Sex",
            "Chest Pain Type",
            "Resting Blood Pressure",
            "Cholesterol",
            "Fasting Blood Sugar > 120",
            "Resting ECG",
            "Max Heart Rate",
            "Exercise Induced Angina",
            "ST Depression (oldpeak)",
            "Slope of ST Segment",
            "Major Vessels (ca)",
            "Thal"
        };

        // --- Feature descriptions for tooltips ---
        public static readonly string[] FeatureDescriptions =
        {
            "Patient age in years (29–77)",
            "0 = Female, 1 = Male",
            "1 = Typical Angina, 2 = Atypical Angina, 3 = Non-Anginal Pain, 4 = Asymptomatic",
            "Resting blood pressure in mmHg (94–200)",
            "Serum cholesterol in mg/dl (126–564)",
            "0 = No, 1 = Yes (fasting blood sugar > 120 mg/dl)",
            "0 = Normal, 1 = ST-T Abnormality, 2 = LV Hypertrophy",
            "Maximum heart rate achieved (71–202 bpm)",
            "0 = No, 1 = Yes",
            "ST depression induced by exercise (0.0–6.2)",
            "1 = Upsloping, 2 = Flat, 3 = Downsloping",
            "Number of major vessels colored by fluoroscopy (0–3)",
            "3 = Normal, 6 = Fixed Defect, 7 = Reversible Defect"
        };

        // --- Encoded feature layout ---
        public const int RawFeatureCount = 13;
        public const int EncodedFeatureCount = 22;

        /// <summary>
        /// Names of the encoded network inputs, in the same order as the encoded rows.
        /// </summary>
        public static readonly string[] EncodedFeatureNames =
        {
            "Age",
            "Sex",
            "CP=1", "CP=2", "CP=3", "CP=4",
            "Resting Blood Pressure",
            "Cholesterol",
            "Fasting Blood Sugar > 120",
            "Restecg=0", "Restecg=1", "Restecg=2",
            "Max Heart Rate",
            "Exercise Induced Angina",
            "ST Depression (oldpeak)",
            "Slope=1", "Slope=2", "Slope=3",
            "Major Vessels (ca)",
            "Thal=3", "Thal=6", "Thal=7"
        };

        /// <summary>
        /// Marks the encoded columns that are already binary one-hot indicators and
        /// therefore must not be MinMax-scaled.
        /// </summary>
        private static readonly bool[] OneHotMask =
        {
            false, false,
            true, true, true, true,
            false, false, false,
            true, true, true,
            false, false, false,
            true, true, true,
            false,
            true, true, true
        };

        // --- Raw data (never normalized in place) ---
        public List<double[]> AllFeatures { get; private set; }
        public List<double[]> AllLabels { get; private set; }

        // --- Normalized, split data ---
        public List<double[]> TrainFeatures { get; private set; }
        public List<double[]> TrainLabels { get; private set; }
        public List<double[]> ValidationFeatures { get; private set; }
        public List<double[]> ValidationLabels { get; private set; }
        public List<double[]> TestFeatures { get; private set; }
        public List<double[]> TestLabels { get; private set; }

        // --- Normalization Parameters (fit on the training split only) ---
        public double[] FeatureMin { get; private set; }
        public double[] FeatureMax { get; private set; }

        /// <summary>Number of encoded network inputs (22 after one-hot encoding).</summary>
        public int FeatureCount => EncodedFeatureCount;

        /// <summary>Number of output neurons (1 for binary classification).</summary>
        public int OutputCount => 1;

        /// <summary>
        /// Expands a raw 13-feature patient row into the 22-input encoded layout.
        /// Categorical features are one-hot encoded; continuous and binary features
        /// are copied through unchanged. Unknown category values produce an all-zero
        /// indicator block rather than failing the prediction.
        /// </summary>
        public static double[] EncodeRaw(double[] rawInput)
        {
            if (rawInput == null) throw new ArgumentNullException(nameof(rawInput));
            if (rawInput.Length != RawFeatureCount)
                throw new ArgumentException(
                    $"Expected {RawFeatureCount} raw features but received {rawInput.Length}.",
                    nameof(rawInput));

            double[] encoded = new double[EncodedFeatureCount];
            int k = 0;

            encoded[k++] = rawInput[0];   // Age
            encoded[k++] = rawInput[1];   // Sex

            AddOneHot(encoded, ref k, rawInput[2], 1.0, 2.0, 3.0, 4.0);   // Chest pain type

            encoded[k++] = rawInput[3];   // Resting BP
            encoded[k++] = rawInput[4];   // Cholesterol
            encoded[k++] = rawInput[5];   // Fasting blood sugar

            AddOneHot(encoded, ref k, rawInput[6], 0.0, 1.0, 2.0);        // Resting ECG

            encoded[k++] = rawInput[7];   // Max heart rate
            encoded[k++] = rawInput[8];   // Exercise angina
            encoded[k++] = rawInput[9];   // ST depression

            AddOneHot(encoded, ref k, rawInput[10], 1.0, 2.0, 3.0);       // Slope

            encoded[k++] = rawInput[11];  // Major vessels

            AddOneHot(encoded, ref k, rawInput[12], 3.0, 6.0, 7.0);       // Thal

            return encoded;
        }

        private static void AddOneHot(double[] encoded, ref int index, double value, params double[] categories)
        {
            for (int i = 0; i < categories.Length; i++)
            {
                encoded[index++] = value == categories[i] ? 1.0 : 0.0;
            }
        }

        /// <summary>
        /// Loads the Heart Disease dataset from a CSV file.
        /// Expected format: age,sex,cp,trestbps,chol,fbs,restecg,thalach,exang,oldpeak,slope,ca,thal,target
        /// The first line is treated as a header if its first value is non-numeric.
        /// Rows containing missing or non-numeric values are skipped.
        /// </summary>
        /// <param name="csvPath">Path to the heart.csv file.</param>
        public void LoadFromCsv(string csvPath)
        {
            AllFeatures = new List<double[]>();
            AllLabels = new List<double[]>();

            string[] lines = File.ReadAllLines(csvPath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length < 14) continue;

                // Skip header row (first value won't parse as a number).
                if (!double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double firstVal))
                    continue;

                double[] features = new double[13];
                features[0] = firstVal;
                bool valid = true;
                for (int i = 1; i < 13; i++)
                {
                    if (!double.TryParse(parts[i].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out features[i]))
                    {
                        valid = false;
                        break;
                    }
                }
                if (!valid) continue;

                if (!double.TryParse(parts[13].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double target))
                    continue;

                AllFeatures.Add(features);
                AllLabels.Add(new[] { target >= 1.0 ? 1.0 : 0.0 });
            }
        }

        /// <summary>
        /// Splits the raw data into training, validation, and testing sets using a
        /// Fisher-Yates shuffle. The held-out portion (100% - <paramref name="trainRatio"/>)
        /// is divided evenly between validation and test.
        ///
        /// MinMax normalization is then fitted on the training split only and applied to
        /// all three splits, keeping the validation and test data completely unseen.
        /// </summary>
        /// <param name="trainRatio">Fraction of data for training (default 0.70 = 70%).</param>
        /// <param name="seed">Random seed for reproducible splitting (default 42).</param>
        public void SplitTrainValidationTest(double trainRatio = 0.70, int seed = 42)
        {
            if (AllFeatures == null || AllFeatures.Count == 0) return;
            if (trainRatio <= 0.0 || trainRatio >= 1.0)
                throw new ArgumentOutOfRangeException(nameof(trainRatio), "Train ratio must be between 0 and 1.");

            int total = AllFeatures.Count;
            int trainSize = (int)(total * trainRatio);
            int holdoutSize = total - trainSize;
            int validationSize = holdoutSize / 2;

            if (trainSize < 2 || validationSize < 1 || holdoutSize - validationSize < 1)
                throw new ArgumentException("The dataset is too small for the requested split ratios.", nameof(trainRatio));

            // Fisher-Yates shuffle for unbiased randomization.
            int[] indices = Enumerable.Range(0, total).ToArray();
            Random rng = new Random(seed);
            for (int i = total - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }

            // Copy raw rows into train/validation/test. AllFeatures stays untouched so the
            // split can be repeated with a different ratio without double-normalizing.
            TrainFeatures = new List<double[]>();
            TrainLabels = new List<double[]>();
            ValidationFeatures = new List<double[]>();
            ValidationLabels = new List<double[]>();
            TestFeatures = new List<double[]>();
            TestLabels = new List<double[]>();

            for (int i = 0; i < total; i++)
            {
                int idx = indices[i];
                if (i < trainSize)
                {
                    TrainFeatures.Add(EncodeRaw(AllFeatures[idx]));
                    TrainLabels.Add((double[])AllLabels[idx].Clone());
                }
                else if (i < trainSize + validationSize)
                {
                    ValidationFeatures.Add(EncodeRaw(AllFeatures[idx]));
                    ValidationLabels.Add((double[])AllLabels[idx].Clone());
                }
                else
                {
                    TestFeatures.Add(EncodeRaw(AllFeatures[idx]));
                    TestLabels.Add((double[])AllLabels[idx].Clone());
                }
            }

            FitNormalizationOnTrain();
            NormalizeRows(TrainFeatures);
            NormalizeRows(ValidationFeatures);
            NormalizeRows(TestFeatures);
        }

        /// <summary>
        /// Computes per-feature min/max values using only the training split.
        /// </summary>
        private void FitNormalizationOnTrain()
        {
            int count = TrainFeatures[0].Length;
            FeatureMin = new double[count];
            FeatureMax = new double[count];

            for (int f = 0; f < count; f++)
            {
                if (OneHotMask[f])
                {
                    FeatureMin[f] = 0.0;
                    FeatureMax[f] = 1.0;
                    continue;
                }

                FeatureMin[f] = TrainFeatures.Min(row => row[f]);
                FeatureMax[f] = TrainFeatures.Max(row => row[f]);
            }
        }

        /// <summary>
        /// Applies the stored MinMax parameters to each row in place.
        /// </summary>
        private void NormalizeRows(List<double[]> rows)
        {
            foreach (double[] row in rows)
            {
                for (int f = 0; f < row.Length; f++)
                {
                    if (OneHotMask[f]) continue;

                    double range = FeatureMax[f] - FeatureMin[f];
                    row[f] = range > 0.0
                        ? (row[f] - FeatureMin[f]) / range
                        : 0.0;
                }
            }
        }

        /// <summary>
        /// Encodes and normalizes a single raw 13-feature input vector using the
        /// training-derived min/max parameters. Values outside the training range are
        /// clipped to [0, 1] so live predictions cannot push the network into saturation.
        /// </summary>
        public double[] NormalizeSingle(double[] rawInput)
        {
            if (rawInput == null) throw new ArgumentNullException(nameof(rawInput));
            if (FeatureMin == null || FeatureMax == null)
                throw new InvalidOperationException("Normalization parameters are not fitted. Call SplitTrainValidationTest first.");
            if (rawInput.Length != RawFeatureCount)
                throw new ArgumentException(
                    $"Expected {RawFeatureCount} raw features but received {rawInput.Length}.", nameof(rawInput));

            double[] encoded = EncodeRaw(rawInput);
            double[] normalized = new double[encoded.Length];
            for (int f = 0; f < encoded.Length; f++)
            {
                if (OneHotMask[f])
                {
                    normalized[f] = encoded[f];
                    continue;
                }

                double range = FeatureMax[f] - FeatureMin[f];
                double value = range > 0.0
                    ? (encoded[f] - FeatureMin[f]) / range
                    : 0.0;
                normalized[f] = Math.Max(0.0, Math.Min(1.0, value));
            }
            return normalized;
        }

        /// <summary>
        /// Returns a formatted summary string describing the loaded dataset.
        /// </summary>
        public string GetDataSummary()
        {
            int diseaseCount = AllLabels.Count(l => l[0] >= 1.0);
            int healthyCount = AllLabels.Count(l => l[0] < 1.0);

            return $"Cleveland Heart Disease Dataset\r\n" +
                   $"══════════════════════════════════\r\n" +
                   $"Total Samples:      {AllFeatures.Count}\r\n" +
                   $"Heart Disease (1):  {diseaseCount}\r\n" +
                   $"No Disease (0):     {healthyCount}\r\n" +
                   $"Raw Features:       {RawFeatureCount}\r\n" +
                   $"Encoded Inputs:     {EncodedFeatureCount} (one-hot: cp, restecg, slope, thal)\r\n" +
                   $"Train Samples:      {TrainFeatures?.Count ?? 0}\r\n" +
                   $"Validation Samples: {ValidationFeatures?.Count ?? 0}\r\n" +
                   $"Test Samples:       {TestFeatures?.Count ?? 0}\r\n" +
                   $"Normalization:      MinMax [0, 1] (fit on train)\r\n" +
                   $"══════════════════════════════════";
        }
    }
}
