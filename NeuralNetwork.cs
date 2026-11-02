using System;
using System.Collections.Generic;
using System.Linq;
using Backprop;

namespace RealWorldPrediction
{
    /// <summary>
    /// 3-Layer Multi-Layer Perceptron (Input → Hidden → Output) using the
    /// unmodified backneural library classes: INeuron, HNeuron, ONeuron.
    /// Supports binary classification with Sigmoid activation.
    /// </summary>
    public class NeuralNetwork
    {
        // --- Network Architecture ---
        private readonly int inputCount;
        private readonly int hiddenCount;
        private readonly int outputCount;

        // --- Neuron Arrays (from Backprop namespace) ---
        private INeuron[] inputNeurons;
        private HNeuron[] hiddenNeurons;
        private ONeuron[] outputNeurons;

        // --- Hyperparameters ---
        public double LearningRate { get; set; }

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
        /// Initializes all neuron layers with random weights and appropriate sizes.
        /// </summary>
        private void InitializeNetwork()
        {
            // Input layer: each INeuron has weights connected to all hidden neurons
            inputNeurons = new INeuron[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                inputNeurons[i] = new INeuron(i, hiddenCount);
            }

            // Hidden layer: each HNeuron has weights connected to all output neurons
            hiddenNeurons = new HNeuron[hiddenCount];
            for (int h = 0; h < hiddenCount; h++)
            {
                hiddenNeurons[h] = new HNeuron(h, outputCount);
            }

            // Output layer: each ONeuron stores its bias and output activation
            outputNeurons = new ONeuron[outputCount];
            for (int o = 0; o < outputCount; o++)
            {
                outputNeurons[o] = new ONeuron(o);
            }
        }

        /// <summary>
        /// Standard Sigmoid activation function: σ(z) = 1 / (1 + e^(-z))
        /// </summary>
        private double Sigmoid(double z)
        {
            // Clamp to avoid numerical overflow in Math.Exp
            if (z < -45.0) return 0.0;
            if (z > 45.0) return 1.0;
            return 1.0 / (1.0 + Math.Exp(-z));
        }

        /// <summary>
        /// Forward pass: feeds inputs through the 3 layers and returns the final output activations.
        /// </summary>
        /// <param name="inputs">Array of normalized input features [0, 1]</param>
        /// <returns>Array of predicted probabilities [0, 1]</returns>
        public double[] ForwardPass(double[] inputs)
        {
            // 1. Feed inputs into the input layer
            for (int i = 0; i < inputCount; i++)
            {
                inputNeurons[i].setInput(inputs[i]);
            }

            // 2. Compute hidden layer activations: a_h = σ(bias_h + Σ x_i * w_ih)
            for (int h = 0; h < hiddenCount; h++)
            {
                double sum = hiddenNeurons[h].getBias();
                for (int i = 0; i < inputCount; i++)
                {
                    sum += inputNeurons[i].getInput() * inputNeurons[i].getWeight(h);
                }
                hiddenNeurons[h].setHactivation(Sigmoid(sum));
            }

            // 3. Compute output layer activations: a_o = σ(bias_o + Σ a_h * w_ho)
            for (int o = 0; o < outputCount; o++)
            {
                double sum = outputNeurons[o].getBias();
                for (int h = 0; h < hiddenCount; h++)
                {
                    sum += hiddenNeurons[h].getHactivation() * hiddenNeurons[h].getWeight(o);
                }
                outputNeurons[o].setOActivation(Sigmoid(sum));
            }

            // 4. Return output values
            double[] result = new double[outputCount];
            for (int o = 0; o < outputCount; o++)
            {
                result[o] = outputNeurons[o].getOActivation();
            }
            return result;
        }

        /// <summary>
        /// Backward pass (Backpropagation):
        /// Computes error gradients and applies weight updates through the network.
        /// Compatible with INeuron, HNeuron, and ONeuron methods.
        /// </summary>
        /// <param name="targets">Ground-truth label array</param>
        public void Backpropagate(double[] targets)
        {
            // Step 1: Compute output layer error signal: δ_o = (target - output) * output * (1 - output)
            double[] outputErrors = new double[outputCount];
            for (int o = 0; o < outputCount; o++)
            {
                double output = outputNeurons[o].getOActivation();
                outputErrors[o] = (targets[o] - output) * output * (1.0 - output);
            }

            // Step 2: Calculate hidden layer errors BEFORE modifying hidden weights
            for (int h = 0; h < hiddenCount; h++)
            {
                hiddenNeurons[h].calculateErr(outputErrors);
            }

            // Step 3: Update output layer biases: bias_o += lr * δ_o
            for (int o = 0; o < outputCount; o++)
            {
                outputNeurons[o].changeBias(LearningRate, outputErrors);
            }

            // Step 4: Update hidden-to-output weights: w_ho += lr * a_h * δ_o
            for (int h = 0; h < hiddenCount; h++)
            {
                hiddenNeurons[h].setWeight(LearningRate, outputErrors);
            }

            // Step 5: Update hidden layer biases: bias_h += lr * δ_h
            for (int h = 0; h < hiddenCount; h++)
            {
                hiddenNeurons[h].changeBias(LearningRate);
            }

            // Step 6: Update input-to-hidden weights: w_ih += lr * δ_h * x_i
            for (int i = 0; i < inputCount; i++)
            {
                for (int h = 0; h < hiddenCount; h++)
                {
                    double hiddenError = hiddenNeurons[h].getErr();
                    inputNeurons[i].setWeight(h, hiddenError, LearningRate);
                }
            }
        }

        /// <summary>
        /// Trains the network on a single training example and computes Binary Cross-Entropy loss.
        /// </summary>
        public double TrainSample(double[] inputs, double[] targets)
        {
            double[] outputs = ForwardPass(inputs);
            Backpropagate(targets);

            // Binary Cross-Entropy loss: L = -[y*log(p) + (1-y)*log(1-p)]
            double loss = 0.0;
            for (int o = 0; o < outputCount; o++)
            {
                double p = Math.Max(1e-7, Math.Min(1.0 - 1e-7, outputs[o]));
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
        /// Re-initializes all weights and biases.
        /// </summary>
        public void Reset()
        {
            InitializeNetwork();
        }
    }
}
