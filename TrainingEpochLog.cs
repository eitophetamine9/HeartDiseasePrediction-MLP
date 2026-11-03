namespace RealWorldPrediction
{
    /// <summary>
    /// Represents a single training log point for graphing and CSV export.
    /// </summary>
    public class TrainingEpochLog
    {
        /// <summary>1-based epoch number.</summary>
        public int Epoch { get; set; }

        /// <summary>Mean binary cross-entropy loss on the training split.</summary>
        public double Loss { get; set; }

        /// <summary>Classification accuracy on the training split, in percent.</summary>
        public double Accuracy { get; set; }

        /// <summary>Mean binary cross-entropy loss on the held-out test split.</summary>
        public double TestLoss { get; set; }

        /// <summary>Classification accuracy on the held-out test split, in percent.</summary>
        public double TestAccuracy { get; set; }
    }
}
