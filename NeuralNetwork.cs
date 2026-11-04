using System;

namespace RealWorldPrediction
{
    /// <summary>
    /// Multi-Layer Perceptron wrapper for the heart-disease binary classifier.
    ///
    /// The heavy lifting is delegated to <see cref="BackpropTrainer"/>, which composes
    /// the provided Backprop neuron classes (INeuron, HNeuron, ONeuron) directly.
    /// This keeps the library files unmodified while allowing the learning rate to be
    /// configured from the UI and optimizing binary cross-entropy.
    /// </summary>
    public class NeuralNetwork
    {
        // --- Network Architecture ---
        private readonly int inputCount;
        private readonly int hiddenCount;
        private readonly int outputCount;
        private readonly int seed;

        // --- Training Engine ---
        private BackpropTrainer trainer;

        // --- Hyperparameters ---
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets the underlying trainer for diagnostics and persistence.
        /// </summary>
        public BackpropTrainer Trainer => trainer;

        /// <summary>
        /// Creates a new neural network with the specified layer dimensions.
        /// </summary>
        /// <param name="inputs">Number of input features (13 for Heart Disease).</param>
        /// <param name="hidden">Number of hidden layer neurons.</param>
        /// <param name="outputs">Number of output neurons (1 for binary classification).</param>
        /// <param name="learningRate">Learning rate for weight updates.</param>
        /// <param name="seed">Seed for reproducible weight initialization.</param>
        public NeuralNetwork(int inputs, int hidden, int outputs, double learningRate, int seed = 42)
        {
            if (learningRate <= 0.0) throw new ArgumentOutOfRangeException(nameof(learningRate));

            inputCount = inputs;
            hiddenCount = hidden;
            outputCount = outputs;
            this.seed = seed;
            LearningRate = learningRate;

            InitializeNetwork();
        }

        /// <summary>
        /// Re-creates the trainer and re-initializes all weights and biases.
        /// </summary>
        private void InitializeNetwork()
        {
            trainer = new BackpropTrainer(inputCount, hiddenCount, outputCount, seed);
        }

        /// <summary>
        /// Forward pass: feeds inputs into the neural network and returns output activations.
        /// </summary>
        /// <param name="inputs">Array of normalized input features [0, 1].</param>
        /// <returns>Array of predicted probabilities [0, 1].</returns>
        public double[] ForwardPass(double[] inputs)
        {
            return trainer.Forward(inputs);
        }

        /// <summary>
        /// Trains the network on a single training example and returns its BCE loss.
        /// </summary>
        /// <param name="inputs">Array of normalized input features.</param>
        /// <param name="targets">Array of ground truth targets (1.0 or 0.0).</param>
        /// <returns>Binary Cross-Entropy loss for this sample.</returns>
        public double TrainSample(double[] inputs, double[] targets)
        {
            return trainer.TrainSample(inputs, targets, LearningRate);
        }

        /// <summary>
        /// Forward pass for testing/inference without updating weights.
        /// </summary>
        public double[] Predict(double[] inputs)
        {
            return trainer.Forward(inputs);
        }

        /// <summary>
        /// Re-initializes all weights and biases.
        /// </summary>
        public void Reset()
        {
            InitializeNetwork();
        }

        /// <summary>
        /// Returns a flat snapshot of all weights and biases for validation-based
        /// model selection.
        /// </summary>
        public double[] ExportWeights()
        {
            return trainer.ExportWeights();
        }

        /// <summary>
        /// Restores a snapshot produced by <see cref="ExportWeights"/>.
        /// </summary>
        public void ImportWeights(double[] snapshot)
        {
            trainer.ImportWeights(snapshot);
        }

        /// <summary>
        /// Saves all network weights and biases to a text file.
        /// </summary>
        public void SaveWeights(string path)
        {
            trainer.SaveWeights(path);
        }

        /// <summary>
        /// Loads network weights and biases from a text file.
        /// </summary>
        public void LoadWeights(string path)
        {
            trainer.LoadWeights(path);
        }
    }
}
