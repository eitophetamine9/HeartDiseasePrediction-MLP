using System;
using System.Collections.Generic;
using System.Linq;

namespace RealWorldPrediction
{
    /// <summary>
    /// Threshold-dependent and threshold-independent binary classification metrics.
    /// </summary>
    public sealed class ClassificationReport
    {
        public int TP { get; set; }
        public int FP { get; set; }
        public int TN { get; set; }
        public int FN { get; set; }

        public double Threshold { get; set; }
        public double Accuracy { get; set; }
        public double Precision { get; set; }
        public double Recall { get; set; }
        public double Specificity { get; set; }
        public double F1 { get; set; }

        /// <summary>Area under the ROC curve, independent of the threshold.</summary>
        public double Auc { get; set; }

        public int Total => TP + FP + TN + FN;
    }

    /// <summary>
    /// Pure metric helpers shared by the training loop and the experiment harness.
    /// </summary>
    public static class ModelMetrics
    {
        /// <summary>
        /// Builds a full classification report for the given decision threshold.
        /// </summary>
        public static ClassificationReport Evaluate(double[] probabilities, int[] labels, double threshold)
        {
            Validate(probabilities, labels);

            ClassificationReport report = BuildConfusion(probabilities, labels, threshold);
            report.Auc = ComputeAuc(probabilities, labels);
            return report;
        }

        /// <summary>
        /// Computes the area under the ROC curve using pairwise ranking with
        /// half-credit for tied scores.
        /// </summary>
        public static double ComputeAuc(double[] probabilities, int[] labels)
        {
            Validate(probabilities, labels);

            int positives = 0;
            int negatives = 0;
            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] == 1) positives++;
                else negatives++;
            }

            if (positives == 0 || negatives == 0) return 0.0;

            double concordant = 0.0;
            double tied = 0.0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                if (labels[i] != 1) continue;
                for (int j = 0; j < probabilities.Length; j++)
                {
                    if (labels[j] != 0) continue;

                    if (probabilities[i] > probabilities[j]) concordant += 1.0;
                    else if (probabilities[i] == probabilities[j]) tied += 1.0;
                }
            }

            return (concordant + 0.5 * tied) / (positives * negatives);
        }

        /// <summary>
        /// Chooses the decision threshold that maximizes Youden's J
        /// (sensitivity + specificity - 1) on the supplied split.
        /// Ties are broken by accuracy, then by proximity to 0.5.
        /// </summary>
        public static double FindBestThreshold(
            double[] probabilities,
            int[] labels,
            out double bestYoudenJ)
        {
            Validate(probabilities, labels);

            List<double> candidates = BuildThresholdCandidates(probabilities);
            double bestThreshold = 0.5;
            double bestJ = double.NegativeInfinity;
            double bestAccuracy = double.NegativeInfinity;

            foreach (double threshold in candidates)
            {
                ClassificationReport report = BuildConfusion(probabilities, labels, threshold);
                double youdenJ = report.Recall + report.Specificity - 1.0;

                bool better = youdenJ > bestJ + 1e-12;
                bool tieBreak = Math.Abs(youdenJ - bestJ) <= 1e-12 &&
                                (report.Accuracy > bestAccuracy + 1e-12 ||
                                 (Math.Abs(report.Accuracy - bestAccuracy) <= 1e-12 &&
                                  Math.Abs(threshold - 0.5) < Math.Abs(bestThreshold - 0.5)));

                if (better || tieBreak)
                {
                    bestJ = youdenJ;
                    bestAccuracy = report.Accuracy;
                    bestThreshold = threshold;
                }
            }

            bestYoudenJ = bestJ;
            return bestThreshold;
        }

        private static ClassificationReport BuildConfusion(
            double[] probabilities,
            int[] labels,
            double threshold)
        {
            int tp = 0, fp = 0, tn = 0, fn = 0;

            for (int i = 0; i < probabilities.Length; i++)
            {
                int predicted = probabilities[i] >= threshold ? 1 : 0;
                int actual = labels[i] == 1 ? 1 : 0;

                if (predicted == 1 && actual == 1) tp++;
                else if (predicted == 1 && actual == 0) fp++;
                else if (predicted == 0 && actual == 0) tn++;
                else fn++;
            }

            int total = tp + fp + tn + fn;
            double accuracy = total > 0 ? (double)(tp + tn) / total * 100.0 : 0.0;
            double precision = (tp + fp) > 0 ? (double)tp / (tp + fp) * 100.0 : 0.0;
            double recall = (tp + fn) > 0 ? (double)tp / (tp + fn) * 100.0 : 0.0;
            double specificity = (tn + fp) > 0 ? (double)tn / (tn + fp) * 100.0 : 0.0;
            double f1 = (precision + recall) > 0
                ? 2.0 * (precision * recall) / (precision + recall)
                : 0.0;

            return new ClassificationReport
            {
                TP = tp,
                FP = fp,
                TN = tn,
                FN = fn,
                Threshold = threshold,
                Accuracy = accuracy,
                Precision = precision,
                Recall = recall,
                Specificity = specificity,
                F1 = f1
            };
        }

        private static List<double> BuildThresholdCandidates(double[] probabilities)
        {
            List<double> candidates = new List<double> { 0.0, 0.5, 1.0 };

            double[] distinct = probabilities.Distinct().OrderBy(p => p).ToArray();
            candidates.AddRange(distinct);

            for (int i = 0; i < distinct.Length - 1; i++)
            {
                candidates.Add((distinct[i] + distinct[i + 1]) / 2.0);
            }

            return candidates;
        }

        private static void Validate(double[] probabilities, int[] labels)
        {
            if (probabilities == null) throw new ArgumentNullException(nameof(probabilities));
            if (labels == null) throw new ArgumentNullException(nameof(labels));
            if (probabilities.Length != labels.Length)
            {
                throw new ArgumentException(
                    "Probabilities and labels must have the same length.");
            }
        }
    }
}
