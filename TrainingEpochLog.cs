namespace RealWorldPrediction
{
    /// <summary>
    /// Represents a single training log point for graphing and CSV export.
    /// </summary>
    public class TrainingEpochLog
    {
        public int Epoch { get; set; }
        public double Loss { get; set; }
        public double Accuracy { get; set; }
    }
}

