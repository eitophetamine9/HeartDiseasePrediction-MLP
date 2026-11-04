using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace RealWorldPrediction
{
    /// <summary>
    /// Metrics produced by one cross-validation fold. The reported test numbers come
    /// from the fold that was held out entirely during training and threshold tuning.
    /// </summary>
    public sealed class CrossValidationFold
    {
        public int Fold { get; set; }
        public int TrainSamples { get; set; }
        public int ValidationSamples { get; set; }
        public int TestSamples { get; set; }
        public int SelectedEpoch { get; set; }
        public double ValidationLoss { get; set; }
        public double Threshold { get; set; }
        public double ValidationAuc { get; set; }
        public double TestLoss { get; set; }
        public ClassificationReport TestReport { get; set; }
    }

    /// <summary>
    /// Aggregate cross-validation summary. Means and sample standard deviations are
    /// computed across folds, which gives a more stable estimate than a single split.
    /// </summary>
    public sealed class CrossValidationResult
    {
        public int Folds { get; set; }
        public List<CrossValidationFold> FoldResults { get; set; } = new List<CrossValidationFold>();

        public double MeanAccuracy { get; set; }
        public double StdAccuracy { get; set; }
        public double MeanAuc { get; set; }
        public double StdAuc { get; set; }
        public double MeanF1 { get; set; }
        public double StdF1 { get; set; }
        public double MeanPrecision { get; set; }
        public double StdPrecision { get; set; }
        public double MeanRecall { get; set; }
        public double StdRecall { get; set; }
        public double MeanSpecificity { get; set; }
        public double StdSpecificity { get; set; }
        public double MeanThreshold { get; set; }
        public double StdThreshold { get; set; }
        public double MeanSelectedEpoch { get; set; }
    }

    /// <summary>
    /// Runs k-fold cross-validation using the same training recipe as the interactive
    /// app: BCE loss, Xavier initialization, early stopping on validation loss, and a
    /// Youden-J threshold tuned on validation. Each fold's test partition is touched
    /// only once, after that fold's model has been selected.
    /// </summary>
    public static class CrossValidator
    {
        /// <summary>
        /// Trains <paramref name="folds"/> models and returns per-fold and aggregate metrics.
        /// </summary>
        /// <param name="data">Loaded dataset. Its splits are overwritten for each fold.</param>
        /// <param name="folds">Number of folds (must be at least 2).</param>
        /// <param name="hiddenNodes">Hidden neurons per model.</param>
        /// <param name="learningRate">Gradient descent step size.</param>
        /// <param name="epochs">Maximum epochs per fold.</param>
        /// <param name="seed">Seed for the split and the per-fold network initialization.</param>
        /// <param name="token">Cancellation token checked between folds and epochs.</param>
        /// <param name="onFoldCompleted">Called with (completedFolds, totalFolds) after each fold.</param>
        /// <param name="log">Optional per-fold log sink.</param>
        public static CrossValidationResult Run(
            DataHandler data,
            int folds,
            int hiddenNodes,
            double learningRate,
            int epochs,
            int seed,
            CancellationToken token,
            Action<int, int> onFoldCompleted = null,
            Action<string> log = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (folds < 2) throw new ArgumentOutOfRangeException(nameof(folds), "At least two folds are required.");
            if (epochs < 1) throw new ArgumentOutOfRangeException(nameof(epochs));

            List<CrossValidationFold> results = new List<CrossValidationFold>();

            for (int fold = 0; fold < folds; fold++)
            {
                token.ThrowIfCancellationRequested();

                data.SplitKFold(fold, folds, seed, 0.15);

                int trainCount = data.TrainFeatures.Count;
                int validationCount = data.ValidationFeatures.Count;
                int testCount = data.TestFeatures.Count;

                // Use a distinct initialization seed per fold so the ensemble is not
                // five copies of the same starting point.
                NeuralNetwork network = new NeuralNetwork(
                    data.FeatureCount, hiddenNodes, data.OutputCount, learningRate, seed + fold);

                double bestValidationLoss = double.MaxValue;
                int bestEpoch = 0;
                double[] bestWeights = null;

                for (int epoch = 1; epoch <= epochs; epoch++)
                {
                    token.ThrowIfCancellationRequested();

                    double lossSum = 0.0;
                    for (int s = 0; s < trainCount; s++)
                    {
                        lossSum += network.TrainSample(data.TrainFeatures[s], data.TrainLabels[s]);
                    }

                    double validationLossSum = 0.0;
                    for (int s = 0; s < validationCount; s++)
                    {
                        double p = network.Predict(data.ValidationFeatures[s])[0];
                        int actual = (int)data.ValidationLabels[s][0];
                        double pc = Math.Max(1e-7, Math.Min(1.0 - 1e-7, p));
                        validationLossSum += -(actual * Math.Log(pc) + (1 - actual) * Math.Log(1.0 - pc));
                    }
                    double validationLoss = validationLossSum / validationCount;

                    if (validationLoss < bestValidationLoss)
                    {
                        bestValidationLoss = validationLoss;
                        bestEpoch = epoch;
                        bestWeights = network.ExportWeights();
                    }
                }

                if (bestWeights != null)
                {
                    network.ImportWeights(bestWeights);
                }

                // Tune the decision threshold on this fold's validation rows.
                double[] validationProbabilities = new double[validationCount];
                int[] validationLabels = new int[validationCount];
                for (int s = 0; s < validationCount; s++)
                {
                    validationProbabilities[s] = network.Predict(data.ValidationFeatures[s])[0];
                    validationLabels[s] = (int)data.ValidationLabels[s][0];
                }
                double validationAuc = ModelMetrics.ComputeAuc(validationProbabilities, validationLabels);
                double youdenJ;
                double threshold = ModelMetrics.FindBestThreshold(
                    validationProbabilities, validationLabels, out youdenJ);

                // Evaluate once on the untouched fold.
                double[] testProbabilities = new double[testCount];
                int[] testLabels = new int[testCount];
                double testLossSum = 0.0;
                for (int s = 0; s < testCount; s++)
                {
                    double p = network.Predict(data.TestFeatures[s])[0];
                    testProbabilities[s] = p;
                    testLabels[s] = (int)data.TestLabels[s][0];

                    double pc = Math.Max(1e-7, Math.Min(1.0 - 1e-7, p));
                    testLossSum += -(testLabels[s] * Math.Log(pc) + (1 - testLabels[s]) * Math.Log(1.0 - pc));
                }

                ClassificationReport report = ModelMetrics.Evaluate(testProbabilities, testLabels, threshold);

                results.Add(new CrossValidationFold
                {
                    Fold = fold + 1,
                    TrainSamples = trainCount,
                    ValidationSamples = validationCount,
                    TestSamples = testCount,
                    SelectedEpoch = bestEpoch,
                    ValidationLoss = bestValidationLoss,
                    Threshold = threshold,
                    ValidationAuc = validationAuc,
                    TestLoss = testLossSum / testCount,
                    TestReport = report
                });

                onFoldCompleted?.Invoke(fold + 1, folds);
                log?.Invoke(string.Format(CultureInfo.InvariantCulture,
                    "  Fold {0}/{1}: epoch {2}, threshold {3:F3}, test acc {4:F2}%, AUC {5:F3}, F1 {6:F2}%",
                    fold + 1, folds, bestEpoch, threshold, report.Accuracy, report.Auc, report.F1));
            }

            return Summarize(folds, results);
        }

        private static CrossValidationResult Summarize(int folds, List<CrossValidationFold> results)
        {
            double[] accuracy = new double[results.Count];
            double[] auc = new double[results.Count];
            double[] f1 = new double[results.Count];
            double[] precision = new double[results.Count];
            double[] recall = new double[results.Count];
            double[] specificity = new double[results.Count];
            double[] threshold = new double[results.Count];
            double[] selectedEpoch = new double[results.Count];

            for (int i = 0; i < results.Count; i++)
            {
                ClassificationReport report = results[i].TestReport;
                accuracy[i] = report.Accuracy;
                auc[i] = report.Auc;
                f1[i] = report.F1;
                precision[i] = report.Precision;
                recall[i] = report.Recall;
                specificity[i] = report.Specificity;
                threshold[i] = results[i].Threshold;
                selectedEpoch[i] = results[i].SelectedEpoch;
            }

            CrossValidationResult summary = new CrossValidationResult
            {
                Folds = folds,
                FoldResults = results
            };

            summary.MeanAccuracy = Mean(accuracy);
            summary.StdAccuracy = StandardDeviation(accuracy, summary.MeanAccuracy);
            summary.MeanAuc = Mean(auc);
            summary.StdAuc = StandardDeviation(auc, summary.MeanAuc);
            summary.MeanF1 = Mean(f1);
            summary.StdF1 = StandardDeviation(f1, summary.MeanF1);
            summary.MeanPrecision = Mean(precision);
            summary.StdPrecision = StandardDeviation(precision, summary.MeanPrecision);
            summary.MeanRecall = Mean(recall);
            summary.StdRecall = StandardDeviation(recall, summary.MeanRecall);
            summary.MeanSpecificity = Mean(specificity);
            summary.StdSpecificity = StandardDeviation(specificity, summary.MeanSpecificity);
            summary.MeanThreshold = Mean(threshold);
            summary.StdThreshold = StandardDeviation(threshold, summary.MeanThreshold);
            summary.MeanSelectedEpoch = Mean(selectedEpoch);

            return summary;
        }

        private static double Mean(double[] values)
        {
            if (values.Length == 0) return 0.0;
            double sum = 0.0;
            for (int i = 0; i < values.Length; i++) sum += values[i];
            return sum / values.Length;
        }

        /// <summary>Sample standard deviation (n - 1 denominator).</summary>
        private static double StandardDeviation(double[] values, double mean)
        {
            if (values.Length < 2) return 0.0;
            double sumSquares = 0.0;
            for (int i = 0; i < values.Length; i++)
            {
                double diff = values[i] - mean;
                sumSquares += diff * diff;
            }
            return Math.Sqrt(sumSquares / (values.Length - 1));
        }
    }
}
