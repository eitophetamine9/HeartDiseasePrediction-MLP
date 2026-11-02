using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RealWorldPrediction
{
    /// <summary>
    /// Handles loading, parsing, normalization, and train-test splitting 
    /// of the Cleveland Heart Disease dataset.
    /// 
    /// Dataset: UCI Cleveland Heart Disease (303 samples, 13 features)
    /// Task:    Binary classification — Heart Disease (1) vs No Disease (0)
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

        // --- Data Storage ---
        public List<double[]> AllFeatures { get; private set; }
        public List<double[]> AllLabels { get; private set; }

        public List<double[]> TrainFeatures { get; private set; }
        public List<double[]> TrainLabels { get; private set; }
        public List<double[]> TestFeatures { get; private set; }
        public List<double[]> TestLabels { get; private set; }

        // --- Normalization Parameters (MinMax scaling to [0,1]) ---
        public double[] FeatureMin { get; private set; }
        public double[] FeatureMax { get; private set; }

        /// <summary>Number of input features (13 for Heart Disease).</summary>
        public int FeatureCount => 13;

        /// <summary>Number of output neurons (1 for binary classification).</summary>
        public int OutputCount => 1;

        /// <summary>
        /// Loads the Heart Disease dataset from a CSV file.
        /// Expected format: age,sex,cp,trestbps,chol,fbs,restecg,thalach,exang,oldpeak,slope,ca,thal,target
        /// First line is treated as a header if non-numeric.
        /// </summary>
        /// <param name="csvPath">Path to the heart.csv file</param>
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

                // Skip header row (first value won't parse as a number)
                if (!double.TryParse(parts[0].Trim(), out double firstVal))
                    continue;

                // Parse all 13 features
                double[] features = new double[13];
                features[0] = firstVal;
                bool valid = true;
                for (int i = 1; i < 13; i++)
                {
                    if (!double.TryParse(parts[i].Trim(), out features[i]))
                    {
                        valid = false;
                        break;
                    }
                }
                if (!valid) continue;

                // Parse target (last column)
                if (!double.TryParse(parts[13].Trim(), out double target))
                    continue;

                AllFeatures.Add(features);
                AllLabels.Add(new[] { target >= 1.0 ? 1.0 : 0.0 });
            }
        }

        /// <summary>
        /// Performs MinMax normalization, scaling every feature to the range [0, 1].
        /// Formula: x_normalized = (x - min) / (max - min)
        /// Stores min/max values for later use when normalizing user input.
        /// </summary>
        public void NormalizeFeatures()
        {
            if (AllFeatures == null || AllFeatures.Count == 0) return;

            int count = AllFeatures[0].Length;
            FeatureMin = new double[count];
            FeatureMax = new double[count];

            // Step 1: Find min and max for each feature column
            for (int f = 0; f < count; f++)
            {
                FeatureMin[f] = AllFeatures.Min(row => row[f]);
                FeatureMax[f] = AllFeatures.Max(row => row[f]);
            }

            // Step 2: Apply MinMax scaling to all samples
            for (int i = 0; i < AllFeatures.Count; i++)
            {
                for (int f = 0; f < count; f++)
                {
                    double range = FeatureMax[f] - FeatureMin[f];
                    AllFeatures[i][f] = range > 0
                        ? (AllFeatures[i][f] - FeatureMin[f]) / range
                        : 0.0;
                }
            }
        }

        /// <summary>
        /// Normalizes a single raw input vector using the stored min/max parameters.
        /// Used when the user enters patient data for prediction.
        /// </summary>
        public double[] NormalizeSingle(double[] rawInput)
        {
            double[] normalized = new double[rawInput.Length];
            for (int f = 0; f < rawInput.Length; f++)
            {
                double range = FeatureMax[f] - FeatureMin[f];
                normalized[f] = range > 0
                    ? (rawInput[f] - FeatureMin[f]) / range
                    : 0.0;
            }
            return normalized;
        }

        /// <summary>
        /// Splits data into training and testing sets using Fisher-Yates shuffle.
        /// </summary>
        /// <param name="trainRatio">Fraction of data for training (default 0.8 = 80%)</param>
        /// <param name="seed">Random seed for reproducible results</param>
        public void SplitTrainTest(double trainRatio = 0.8, int seed = 42)
        {
            int total = AllFeatures.Count;
            int trainSize = (int)(total * trainRatio);

            // Fisher-Yates shuffle for unbiased randomization
            int[] indices = Enumerable.Range(0, total).ToArray();
            Random rng = new Random(seed);
            for (int i = total - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }

            // Partition into train and test sets
            TrainFeatures = new List<double[]>();
            TrainLabels = new List<double[]>();
            TestFeatures = new List<double[]>();
            TestLabels = new List<double[]>();

            for (int i = 0; i < total; i++)
            {
                int idx = indices[i];
                if (i < trainSize)
                {
                    TrainFeatures.Add(AllFeatures[idx]);
                    TrainLabels.Add(AllLabels[idx]);
                }
                else
                {
                    TestFeatures.Add(AllFeatures[idx]);
                    TestLabels.Add(AllLabels[idx]);
                }
            }
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
                   $"Features:           {FeatureCount}\r\n" +
                   $"Train Samples:      {TrainFeatures?.Count ?? 0}\r\n" +
                   $"Test Samples:       {TestFeatures?.Count ?? 0}\r\n" +
                   $"Normalization:      MinMax [0, 1]\r\n" +
                   $"══════════════════════════════════";
        }
    }
}

