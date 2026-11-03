using System;
using Backprop;

namespace RealWorldPrediction
{
    /// <summary>
    /// Multi-Layer Perceptron wrapper that integrates the core Backprop.NeuralNet engine
    /// from BackNeural 1.cs with high-level training, prediction, loss calculation,
    /// and model serialization capabilities for the Windows Forms application.
    /// </summary>
    public class NeuralNetwork
    {
        // --- Network Architecture ---
        private readonly int inputCount;
        private readonly int hiddenCount;
        private readonly int outputCount;

        // --- Core NeuralNet Engine (from Backprop namespace in BackNeural 1.cs) ---
        private NeuralNet neuralNet;

        // --- Hyperparameters ---
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets the underlying Backprop.NeuralNet instance.
        /// </summary>
        public NeuralNet InternalNet => neuralNet;

        /// <summary>
        /// Creates a new neural network with the specified layer dimensions.
        /// </summary>
        /// <param name="inputs">Number of input features (13 for Heart Disease)</param>
        /// <param name="hidden">Number of hidden layer neurons</param>
        /// <param name="outputs">Number of output neurons (1 for binary classification)</param>
        /// <param name="learningRate">Learning rate for weight updates</param>
        public NeuralNetwork(int inputs, int hidden, int outputs, double learningRate)
        {
            inputCount = inputs;
            hiddenCount = hidden;
            outputCount = outputs;
            LearningRate = learningRate;

            InitializeNetwork();
        }

        /// <summary>
        /// Initializes the Backprop.NeuralNet instance with layer dimensions.
        /// </summary>
        private void InitializeNetwork()
        {
            neuralNet = new NeuralNet(inputCount, hiddenCount, outputCount);
        }

        /// <summary>
        /// Forward pass: feeds inputs into the neural network and returns output activations.
        /// Uses Backprop.NeuralNet.run() to propagate signals through hidden and output layers.
        /// </summary>
        /// <param name="inputs">Array of normalized input features [0, 1]</param>
        /// <returns>Array of predicted probabilities [0, 1]</returns>
        public double[] ForwardPass(double[] inputs)
        {
            for (int i = 0; i < inputCount; i++)
            {
                neuralNet.setInputs(i, inputs[i]);
            }

            neuralNet.run();

            double[] result = new double[outputCount];
            for (int o = 0; o < outputCount; o++)
            {
                result[o] = neuralNet.getOuputData(o);
            }
            return result;
        }

        /// <summary>
        /// Trains the network on a single training example and computes Binary Cross-Entropy loss.
        /// Invokes Backprop.NeuralNet.learn() which executes the forward pass, calculates
        /// error derivatives, and applies gradient updates across all layers.
        /// </summary>
        /// <param name="inputs">Array of normalized input features</param>
        /// <param name="targets">Array of ground truth targets (1.0 or 0.0)</param>
        /// <returns>Binary Cross-Entropy loss for this sample</returns>
        public double TrainSample(double[] inputs, double[] targets)
        {
            for (int i = 0; i < inputCount; i++)
            {
                neuralNet.setInputs(i, inputs[i]);
            }
            for (int o = 0; o < outputCount; o++)
            {
                neuralNet.setDesiredOutput(o, targets[o]);
            }

            // Executes forward pass, error backpropagation, and weight/bias updates
            neuralNet.learn();

            // Compute Binary Cross-Entropy loss: L = -[y*log(p) + (1-y)*log(1-p)]
            double loss = 0.0;
            for (int o = 0; o < outputCount; o++)
            {
                double output = neuralNet.getOuputData(o);
                double p = Math.Max(1e-7, Math.Min(1.0 - 1e-7, output));
                loss += -(targets[o] * Math.Log(p) + (1.0 - targets[o]) * Math.Log(1.0 - p));
            }
            return loss / outputCount;
        }

        /// <summary>
        /// Forward pass for testing/inference without updating weights.
        /// </summary>
        public double[] Predict(double[] inputs)
        {
            return ForwardPass(inputs);
        }

        /// <summary>
        /// Re-initializes all weights and biases by creating a new NeuralNet instance.
        /// </summary>
        public void Reset()
        {
            InitializeNetwork();
        }

        /// <summary>
        /// Saves all network weights and biases to a text file using NeuralNet.saveWeights.
        /// </summary>
        public void SaveWeights(string path)
        {
            neuralNet.saveWeights(path);
        }

        /// <summary>
        /// Loads network weights and biases from a text file using NeuralNet.loadWeights.
        /// </summary>
        public void LoadWeights(string path)
        {
            neuralNet.loadWeights(path);
        }
    }
}
