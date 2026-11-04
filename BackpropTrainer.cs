using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Backprop;

namespace RealWorldPrediction
{
    /// <summary>
    /// Thin training orchestrator built only from the provided Backprop neuron classes
    /// (<see cref="INeuron"/>, <see cref="HNeuron"/>, and <see cref="ONeuron"/>).
    ///
    /// Why this exists:
    /// The provided <c>Backprop.NeuralNet.learn()</c> hardcodes its learning rates
    /// (LRPOUT = 0.2, LRPIN = 0.15) and multiplies the output error by the sigmoid
    /// derivative, which is the gradient of mean squared error. This class keeps the
    /// library files untouched but:
    ///   - exposes a configurable learning rate,
    ///   - optimizes binary cross-entropy (BCE) instead of MSE,
    ///   - uses seeded Xavier/Glorot weight initialization for reproducibility.
    ///
    /// The forward pass and all weight/bias updates are delegated to the library's
    /// public neuron methods, so the assignment's "only use the given libraries"
    /// constraint is still respected.
    /// </summary>
    public class BackpropTrainer
    {
        private readonly INeuron[] inputNeurons;
        private readonly HNeuron[] hiddenNeurons;
        private readonly ONeuron[] outputNeurons;
        private readonly double[] outputDelta;
        private readonly Random rng;

        /// <summary>Number of input features.</summary>
        public int InputCount => inputNeurons.Length;

        /// <summary>Number of hidden neurons.</summary>
        public int HiddenCount => hiddenNeurons.Length;

        /// <summary>Number of output neurons.</summary>
        public int OutputCount => outputNeurons.Length;

        /// <summary>
        /// Creates a new trainer with seeded Xavier/Glorot initialization.
        /// </summary>
        /// <param name="inputs">Number of input features (13 for the heart dataset).</param>
        /// <param name="hidden">Number of hidden neurons.</param>
        /// <param name="outputs">Number of output neurons (1 for binary classification).</param>
        /// <param name="seed">Seed for reproducible weight initialization.</param>
        public BackpropTrainer(int inputs, int hidden, int outputs, int seed = 42)
        {
            if (inputs <= 0) throw new ArgumentOutOfRangeException(nameof(inputs), "At least one input is required.");
            if (hidden <= 0) throw new ArgumentOutOfRangeException(nameof(hidden), "At least one hidden neuron is required.");
            if (outputs <= 0) throw new ArgumentOutOfRangeException(nameof(outputs), "At least one output is required.");

            rng = new Random(seed);
            inputNeurons = new INeuron[inputs];
            hiddenNeurons = new HNeuron[hidden];
            outputNeurons = new ONeuron[outputs];
            outputDelta = new double[outputs];

            // Xavier/Glorot uniform initialization: U(-sqrt(6/(fanIn+fanOut)), +sqrt(6/(fanIn+fanOut)))
            double limitInputHidden = Math.Sqrt(6.0 / (inputs + hidden));
            for (int i = 0; i < inputs; i++)
            {
                inputNeurons[i] = new INeuron(i, hidden);
                for (int h = 0; h < hidden; h++)
                {
                    inputNeurons[i].setWeight(h, NextUniform(-limitInputHidden, limitInputHidden));
                }
            }

            double limitHiddenOutput = Math.Sqrt(6.0 / (hidden + outputs));
            for (int h = 0; h < hidden; h++)
            {
                hiddenNeurons[h] = new HNeuron(h, outputs);
                hiddenNeurons[h].setBias(0.0);
                for (int o = 0; o < outputs; o++)
                {
                    hiddenNeurons[h].setWeight(o, NextUniform(-limitHiddenOutput, limitHiddenOutput));
                }
            }

            for (int o = 0; o < outputs; o++)
            {
                outputNeurons[o] = new ONeuron(o);
                outputNeurons[o].setBias(0.0);
            }
        }

        private double NextUniform(double min, double max)
        {
            return min + rng.NextDouble() * (max - min);
        }

        private static double Sigmoid(double z)
        {
            // Symmetric clamp keeps exp() finite and matches the library's saturation behaviour.
            if (z >= 20.0) return 1.0;
            if (z <= -20.0) return 0.0;
            return 1.0 / (1.0 + Math.Exp(-z));
        }

        /// <summary>
        /// Runs a forward pass and returns the output activations.
        /// </summary>
        /// <param name="inputs">Normalized input features in [0, 1].</param>
        /// <returns>Output probabilities in (0, 1).</returns>
        public double[] Forward(double[] inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            if (inputs.Length != InputCount)
            {
                throw new ArgumentException(
                    $"Expected {InputCount} inputs but received {inputs.Length}.", nameof(inputs));
            }

            for (int i = 0; i < inputNeurons.Length; i++)
            {
                inputNeurons[i].setInput(inputs[i]);
            }

            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                double z = hiddenNeurons[h].getBias();
                for (int i = 0; i < inputNeurons.Length; i++)
                {
                    z += inputNeurons[i].getInput() * inputNeurons[i].getWeight(h);
                }
                hiddenNeurons[h].setHactivation(Sigmoid(z));
            }

            double[] result = new double[outputNeurons.Length];
            for (int o = 0; o < outputNeurons.Length; o++)
            {
                double z = outputNeurons[o].getBias();
                for (int h = 0; h < hiddenNeurons.Length; h++)
                {
                    z += hiddenNeurons[h].getHactivation() * hiddenNeurons[h].getWeight(o);
                }
                outputNeurons[o].setOActivation(Sigmoid(z));
                result[o] = outputNeurons[o].getOActivation();
            }
            return result;
        }

        /// <summary>
        /// Performs one stochastic gradient-descent step with binary cross-entropy loss.
        ///
        /// BCE: L = -[y*ln(p) + (1-y)*ln(1-p)]
        /// dL/dz = p - y
        /// The library's update methods add <c>lr * delta * activation</c>, so we pass
        /// <c>delta = y - p</c> to move in the negative-gradient direction.
        /// </summary>
        /// <param name="inputs">Normalized input features.</param>
        /// <param name="targets">Ground-truth targets (1.0 or 0.0).</param>
        /// <param name="learningRate">Step size for this update.</param>
        /// <returns>Mean BCE loss for this sample before the weight update.</returns>
        public double TrainSample(double[] inputs, double[] targets, double learningRate)
        {
            if (targets == null) throw new ArgumentNullException(nameof(targets));
            if (targets.Length != OutputCount)
            {
                throw new ArgumentException(
                    $"Expected {OutputCount} targets but received {targets.Length}.", nameof(targets));
            }
            if (learningRate <= 0.0) throw new ArgumentOutOfRangeException(nameof(learningRate));

            double[] predictions = Forward(inputs);
            double loss = 0.0;

            for (int o = 0; o < outputNeurons.Length; o++)
            {
                double p = Math.Min(1.0 - 1e-7, Math.Max(1e-7, predictions[o]));
                loss += -(targets[o] * Math.Log(p) + (1.0 - targets[o]) * Math.Log(1.0 - p));
                outputDelta[o] = targets[o] - predictions[o];
            }

            // Hidden deltas must be computed with the pre-update hidden->output weights.
            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                hiddenNeurons[h].calculateErr(outputDelta);
            }

            // Update hidden -> output weights.
            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                hiddenNeurons[h].setWeight(learningRate, outputDelta);
            }

            // Update input -> hidden weights.
            for (int i = 0; i < inputNeurons.Length; i++)
            {
                for (int h = 0; h < hiddenNeurons.Length; h++)
                {
                    inputNeurons[i].setWeight(h, hiddenNeurons[h].getErr(), learningRate);
                }
            }

            // Update output and hidden biases.
            for (int o = 0; o < outputNeurons.Length; o++)
            {
                outputNeurons[o].changeBias(learningRate, outputDelta);
            }
            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                hiddenNeurons[h].changeBias(learningRate);
            }

            return loss / outputNeurons.Length;
        }

        /// <summary>
        /// Returns a flat snapshot of all weights and biases in a stable order:
        /// input-hidden weights, hidden-output weights, hidden biases, output biases.
        /// </summary>
        public double[] ExportWeights()
        {
            List<double> values = new List<double>(
                InputCount * HiddenCount + HiddenCount * OutputCount + HiddenCount + OutputCount);

            for (int i = 0; i < inputNeurons.Length; i++)
            {
                for (int h = 0; h < hiddenNeurons.Length; h++)
                {
                    values.Add(inputNeurons[i].getWeight(h));
                }
            }

            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                for (int o = 0; o < outputNeurons.Length; o++)
                {
                    values.Add(hiddenNeurons[h].getWeight(o));
                }
            }

            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                values.Add(hiddenNeurons[h].getBias());
            }

            for (int o = 0; o < outputNeurons.Length; o++)
            {
                values.Add(outputNeurons[o].getBias());
            }

            return values.ToArray();
        }

        /// <summary>
        /// Restores a snapshot produced by <see cref="ExportWeights"/>.
        /// </summary>
        public void ImportWeights(double[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            int expected = InputCount * HiddenCount + HiddenCount * OutputCount + HiddenCount + OutputCount;
            if (values.Length != expected)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture,
                        "Expected {0} weight values but found {1}.", expected, values.Length),
                    nameof(values));
            }

            int index = 0;
            for (int i = 0; i < inputNeurons.Length; i++)
            {
                for (int h = 0; h < hiddenNeurons.Length; h++)
                {
                    inputNeurons[i].setWeight(h, values[index++]);
                }
            }

            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                for (int o = 0; o < outputNeurons.Length; o++)
                {
                    hiddenNeurons[h].setWeight(o, values[index++]);
                }
            }

            for (int h = 0; h < hiddenNeurons.Length; h++)
            {
                hiddenNeurons[h].setBias(values[index++]);
            }

            for (int o = 0; o < outputNeurons.Length; o++)
            {
                outputNeurons[o].setBias(values[index++]);
            }
        }

        /// <summary>
        /// Saves all weights and biases in a stable, culture-invariant text format.
        /// The first two lines are comments describing the architecture.
        /// </summary>
        public void SaveWeights(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("# BackpropTrainer weights");
                sw.WriteLine(string.Format(CultureInfo.InvariantCulture,
                    "# inputs={0} hidden={1} outputs={2}", InputCount, HiddenCount, OutputCount));

                foreach (double value in ExportWeights())
                {
                    sw.WriteLine(value.ToString("R", CultureInfo.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Loads weights and biases written by <see cref="SaveWeights"/>.
        /// </summary>
        public void LoadWeights(string path)
        {
            List<double> values = new List<double>();
            foreach (string line in File.ReadAllLines(path))
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith("#", StringComparison.Ordinal)) continue;
                values.Add(double.Parse(trimmed, CultureInfo.InvariantCulture));
            }

            ImportWeights(values.ToArray());
        }
    }
}
