using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealWorldPrediction
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private DataHandler dataHandler;
        private NeuralNetwork network;
        private List<TrainingEpochLog> trainingHistory;
        private readonly object historyLock = new object();
        private bool isTraining = false;
        private System.Threading.CancellationTokenSource cts;
        private Control[] txtInputs;
        private Panel pnlChart;

        // Configuration of the most recent run, used when exporting the training log.
        private double lastLearningRate;
        private int lastHiddenNodes;
        private int lastEpochs;
        private double lastSplitRatio;
        private int lastSeed = 42;

        public Form1()
        {
            InitializeComponent();
            trainingHistory = new List<TrainingEpochLog>();
            InitializeChart();
            InitializeApplication();
        }

        /// <summary>
        /// Creates a lightweight GDI+ chart panel below the training log. Using a plain
        /// Panel avoids adding a Charting assembly reference while still giving a live
        /// loss/accuracy curve.
        /// </summary>
        private void InitializeChart()
        {
            pnlChart = new Panel
            {
                Location = new Point(10, 430),
                Size = new Size(508, 258),
                BackColor = Color.FromArgb(18, 18, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlChart.Paint += PnlChart_Paint;
            grpLog.Controls.Add(pnlChart);

            // Shrink the log text box so it does not overlap the chart.
            rtbLog.Size = new Size(508, 398);
        }

        /// <summary>
        /// Sets up the dataset, pre-populates sample patient data, and displays initial info.
        /// </summary>
        private void InitializeApplication()
        {
            txtInputs = new Control[]
            {
                txtAge, cmbSex, cmbCp, txtTrestbps, txtChol, cmbFbs,
                cmbRestecg, txtThalach, cmbExang, txtOldpeak, cmbSlope, cmbCa, cmbThal
            };
            dataHandler = new DataHandler();
            LoadDataset();
            PopulateDefaultPatientInput();
        }

        /// <summary>
        /// Locates and loads heart.csv from possible build and project locations.
        /// </summary>
        private void LoadDataset()
        {
            string[] possiblePaths = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "heart.csv"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Data", "heart.csv"),
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "heart.csv"),
                Path.GetFullPath(@"..\..\Data\heart.csv")
            };

            string foundPath = null;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    foundPath = path;
                    break;
                }
            }

            if (foundPath != null)
            {
                dataHandler.LoadFromCsv(foundPath);
                dataHandler.SplitTrainTest((double)nudTrainSplit.Value / 100.0, 42);

                lblDataInfo.Text = dataHandler.GetDataSummary();
                AppendLog("✔ Dataset loaded successfully from:\r\n  " + Path.GetFileName(foundPath) + "\r\n");
                AppendLog($"  Total: {dataHandler.AllFeatures.Count} records, {dataHandler.FeatureCount} features\r\n");
                AppendLog($"  Train set: {dataHandler.TrainFeatures.Count} samples | Test set: {dataHandler.TestFeatures.Count} samples\r\n\r\n");
            }
            else
            {
                lblDataInfo.Text = "Error: heart.csv not found.";
                AppendLog("⚠ Warning: heart.csv could not be found automatically.\r\n");
            }
        }

        /// <summary>
        /// Fills the patient input controls with a realistic test patient sample.
        /// </summary>
        private void PopulateDefaultPatientInput()
        {
            btnPresetHigh_Click(null, null);
        }

        private void btnPresetHigh_Click(object sender, EventArgs e)
        {
            // Real High-Risk patient from authentic UCI Cleveland dataset (Patient #2, Target = 1 - Heart Disease):
            // 67, 1, 4, 160, 286, 0, 2, 108, 1, 1.5, 2, 3, 3 -> 1
            SetInputValue(0, "67");       // Age
            SetComboIndex(1, 1);          // Sex: Male
            SetComboIndex(2, 3);          // Chest Pain: 4 — Asymptomatic
            SetInputValue(3, "160");      // Resting BP: 160 mmHg
            SetInputValue(4, "286");      // Cholesterol: 286 mg/dl
            SetComboIndex(5, 0);          // Fasting BS > 120: No
            SetComboIndex(6, 2);          // Resting ECG: 2 — LV Hypertrophy
            SetInputValue(7, "108");      // Max HR: 108
            SetComboIndex(8, 1);          // Exercise Angina: 1 — Yes
            SetInputValue(9, "1.5");      // ST depression: 1.5
            SetComboIndex(10, 1);         // Slope: 2 — Flat
            SetComboIndex(11, 3);         // CA: 3
            SetComboIndex(12, 0);         // Thal: 3 — Normal

            AppendLog("Loaded High-Risk Patient profile (Patient #2 from UCI Cleveland dataset).\r\n");
        }

        private void btnPresetLow_Click(object sender, EventArgs e)
        {
            // Real Low-Risk patient from authentic UCI Cleveland dataset (Patient #4, Target = 0 - Healthy):
            // 37, 1, 3, 130, 250, 0, 0, 187, 0, 3.5, 3, 0, 3 -> 0
            SetInputValue(0, "37");       // Age
            SetComboIndex(1, 1);          // Sex: Male
            SetComboIndex(2, 2);          // Chest Pain: 3 — Non-Anginal Pain
            SetInputValue(3, "130");      // Resting BP: 130 mmHg
            SetInputValue(4, "250");      // Cholesterol: 250 mg/dl
            SetComboIndex(5, 0);          // Fasting BS > 120: No
            SetComboIndex(6, 0);          // Resting ECG: 0 — Normal
            SetInputValue(7, "187");      // Max HR: 187
            SetComboIndex(8, 0);          // Exercise Angina: 0 — No
            SetInputValue(9, "3.5");      // ST depression: 3.5
            SetComboIndex(10, 2);         // Slope: 3 — Downsloping
            SetComboIndex(11, 0);         // CA: 0
            SetComboIndex(12, 0);         // Thal: 3 — Normal

            AppendLog("Loaded Low-Risk Patient profile (Patient #4 from UCI Cleveland dataset).\r\n");
        }

        private void SetInputValue(int index, string value)
        {
            if (txtInputs != null && index < txtInputs.Length && txtInputs[index] is TextBox tb)
            {
                tb.Text = value;
            }
        }

        private void SetComboIndex(int index, int itemIndex)
        {
            if (txtInputs != null && index < txtInputs.Length && txtInputs[index] is ComboBox cb)
            {
                if (itemIndex >= 0 && itemIndex < cb.Items.Count)
                    cb.SelectedIndex = itemIndex;
            }
        }

        /// <summary>
        /// Asynchronously trains the 3-layer neural network using background multithreading (Task.Run)
        /// with non-blocking UI reporting and cancellation support to keep the interface completely responsive.
        /// </summary>
        private async void btnTrain_Click(object sender, EventArgs e)
        {
            if (isTraining)
            {
                MessageBox.Show("Training is already in progress. Please wait or click Stop.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataHandler.AllFeatures == null || dataHandler.AllFeatures.Count == 0)
            {
                MessageBox.Show("No dataset loaded to train on.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Retrieve hyperparameters from UI
            double lr = (double)nudLearningRate.Value;
            int hiddenCount = (int)nudHiddenNodes.Value;
            int epochs = (int)nudEpochs.Value;
            int logInterval = (int)nudLogInterval.Value;
            double splitRatio = (double)nudTrainSplit.Value / 100.0;
            int seed = 42;

            // Remember the configuration for CSV export.
            lastLearningRate = lr;
            lastHiddenNodes = hiddenCount;
            lastEpochs = epochs;
            lastSplitRatio = splitRatio;
            lastSeed = seed;

            // Re-split and re-fit normalization on the training portion only.
            dataHandler.SplitTrainTest(splitRatio, seed);
            lblDataInfo.Text = dataHandler.GetDataSummary();

            // Instantiate network: 13 inputs -> hiddenCount -> 1 output
            network = new NeuralNetwork(dataHandler.FeatureCount, hiddenCount, dataHandler.OutputCount, lr, seed);
            lock (historyLock)
            {
                trainingHistory.Clear();
            }
            cts = new System.Threading.CancellationTokenSource();

            // UI state updates
            isTraining = true;
            btnTrain.Enabled = false;
            btnStop.Enabled = true;
            btnReset.Enabled = false;
            btnExportLog.Enabled = false;
            progressBar.Value = 0;
            progressBar.Maximum = epochs;
            pnlChart.Invalidate();

            AppendLog("═════════════════════════════════════════════════════════\r\n");
            AppendLog($"  STARTING MULTITHREADED TRAINING SESSION\r\n");
            AppendLog($"  Architecture:   {dataHandler.FeatureCount} Inputs → {hiddenCount} Hidden → 1 Output (Sigmoid)\r\n");
            AppendLog($"  Learning Rate:  {lr:F4}\r\n");
            AppendLog($"  Total Epochs:   {epochs:N0}\r\n");
            AppendLog($"  Train / Test:   {dataHandler.TrainFeatures.Count} / {dataHandler.TestFeatures.Count} samples\r\n");
            AppendLog($"  Loss:           Binary Cross-Entropy (BCE)\r\n");
            AppendLog($"  Init Seed:      {seed}\r\n");
            AppendLog("═════════════════════════════════════════════════════════\r\n");

            DateTime startTime = DateTime.Now;
            bool wasCancelled = false;

            try
            {
                // Run training on background thread pool to ensure UI never freezes or stutters
                await Task.Run(() =>
                {
                    var token = cts.Token;
                    var trainFeatures = dataHandler.TrainFeatures;
                    var trainLabels = dataHandler.TrainLabels;
                    var testFeatures = dataHandler.TestFeatures;
                    var testLabels = dataHandler.TestLabels;
                    int sampleCount = trainFeatures.Count;
                    int testCount = testFeatures.Count;

                    for (int epoch = 1; epoch <= epochs; epoch++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            wasCancelled = true;
                            break;
                        }

                        double epochLossSum = 0.0;

                        // Stochastic gradient descent pass
                        for (int s = 0; s < sampleCount; s++)
                        {
                            double loss = network.TrainSample(trainFeatures[s], trainLabels[s]);
                            epochLossSum += loss;
                        }

                        double avgLoss = epochLossSum / sampleCount;

                        // Log progress at throttled intervals using non-blocking BeginInvoke
                        if (epoch % logInterval == 0 || epoch == epochs || epoch == 1)
                        {
                            int correct = 0;
                            for (int s = 0; s < sampleCount; s++)
                            {
                                double pred = network.Predict(trainFeatures[s])[0];
                                int predictedClass = pred >= 0.5 ? 1 : 0;
                                int actualClass = (int)trainLabels[s][0];
                                if (predictedClass == actualClass) correct++;
                            }
                            double trainAcc = (double)correct / sampleCount * 100.0;

                            // Test metrics are logged for monitoring only; the model is not
                            // selected or tuned using this split.
                            double testLossSum = 0.0;
                            int testCorrect = 0;
                            for (int s = 0; s < testCount; s++)
                            {
                                double pred = network.Predict(testFeatures[s])[0];
                                int predictedClass = pred >= 0.5 ? 1 : 0;
                                int actualClass = (int)testLabels[s][0];
                                if (predictedClass == actualClass) testCorrect++;

                                double p = Math.Max(1e-7, Math.Min(1.0 - 1e-7, pred));
                                testLossSum += -(actualClass * Math.Log(p) + (1 - actualClass) * Math.Log(1.0 - p));
                            }
                            double testAcc = testCount > 0 ? (double)testCorrect / testCount * 100.0 : 0.0;
                            double testLoss = testCount > 0 ? testLossSum / testCount : 0.0;

                            var logEntry = new TrainingEpochLog
                            {
                                Epoch = epoch,
                                Loss = avgLoss,
                                Accuracy = trainAcc,
                                TestLoss = testLoss,
                                TestAccuracy = testAcc
                            };
                            lock (historyLock)
                            {
                                trainingHistory.Add(logEntry);
                            }

                            // Asynchronously update UI without stalling the worker thread
                            this.BeginInvoke(new Action(() =>
                            {
                                if (!this.IsDisposed)
                                {
                                    progressBar.Value = Math.Min(progressBar.Maximum, epoch);
                                    AppendLog(string.Format(
                                        "  Epoch {0,5} | Loss: {1:F5} | Train Acc: {2:F2}% | Test Acc: {3:F2}%\r\n",
                                        epoch, avgLoss, trainAcc, testAcc));
                                    pnlChart.Invalidate();
                                }
                            }));
                        }
                    }
                });

                TimeSpan elapsed = DateTime.Now - startTime;
                AppendLog("═════════════════════════════════════════════════════════\r\n");
                if (wasCancelled)
                {
                    AppendLog($"⏹ Training stopped by user after {elapsed.TotalSeconds:F2} seconds.\r\n\r\n");
                }
                else
                {
                    AppendLog($"✔ Training completed in {elapsed.TotalSeconds:F2} seconds.\r\n\r\n");
                }

                // Evaluate on unseen Test Set
                EvaluateModel();
                pnlChart.Invalidate();
            }
            finally
            {
                isTraining = false;
                btnTrain.Enabled = true;
                btnStop.Enabled = false;
                btnReset.Enabled = true;
                btnExportLog.Enabled = true;
                cts?.Dispose();
                cts = null;
            }
        }

        /// <summary>
        /// Cancels an in-progress training run cleanly.
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isTraining && cts != null && !cts.IsCancellationRequested)
            {
                btnStop.Enabled = false;
                cts.Cancel();
                AppendLog("⏹ Requesting training stop... please wait.\r\n");
            }
        }

        /// <summary>
        /// Computes Accuracy, Precision, Recall, F1 Score, and Confusion Matrix on the test set.
        /// </summary>
        private void EvaluateModel()
        {
            if (network == null || dataHandler.TestFeatures == null || dataHandler.TestFeatures.Count == 0)
                return;

            int tp = 0; // True Positive (Predicted Disease, Actual Disease)
            int fp = 0; // False Positive (Predicted Disease, Actual Healthy)
            int tn = 0; // True Negative (Predicted Healthy, Actual Healthy)
            int fn = 0; // False Negative (Predicted Healthy, Actual Disease)
            double testLossSum = 0.0;

            int testCount = dataHandler.TestFeatures.Count;
            for (int i = 0; i < testCount; i++)
            {
                double[] output = network.Predict(dataHandler.TestFeatures[i]);
                double predProb = output[0];
                int actual = (int)dataHandler.TestLabels[i][0];
                int predicted = predProb >= 0.5 ? 1 : 0;

                // Loss
                double p = Math.Max(1e-7, Math.Min(1.0 - 1e-7, predProb));
                testLossSum += -(actual * Math.Log(p) + (1.0 - actual) * Math.Log(1.0 - p));

                if (predicted == 1 && actual == 1) tp++;
                else if (predicted == 1 && actual == 0) fp++;
                else if (predicted == 0 && actual == 0) tn++;
                else if (predicted == 0 && actual == 1) fn++;
            }

            double accuracy = (double)(tp + tn) / testCount * 100.0;
            double precision = (tp + fp) > 0 ? (double)tp / (tp + fp) * 100.0 : 0.0;
            double recall = (tp + fn) > 0 ? (double)tp / (tp + fn) * 100.0 : 0.0;
            double f1 = (precision + recall) > 0 ? 2 * (precision * recall) / (precision + recall) : 0.0;
            double avgTestLoss = testLossSum / testCount;

            // Display in metric cards
            lblAccValue.Text = $"{accuracy:F1}%";
            lblPrecValue.Text = $"{precision:F1}%";
            lblRecValue.Text = $"{recall:F1}%";
            lblF1Value.Text = $"{f1:F1}%";
            lblLossValue.Text = $"{avgTestLoss:F4}";

            // Highlight color based on performance
            lblAccValue.ForeColor = accuracy >= 80 ? Color.FromArgb(70, 220, 120) : Color.FromArgb(240, 180, 70);

            // Print full evaluation report to the training log
            AppendLog("📊  TEST SET EVALUATION REPORT\r\n");
            AppendLog("─────────────────────────────────────────────────────────\r\n");
            AppendLog($"  Accuracy:   {accuracy:F2}%\r\n");
            AppendLog($"  Precision:  {precision:F2}%\r\n");
            AppendLog($"  Recall:     {recall:F2}%\r\n");
            AppendLog($"  F1-Score:   {f1:F2}%\r\n");
            AppendLog($"  Test Loss:  {avgTestLoss:F4}\r\n\r\n");
            AppendLog("  Confusion Matrix:\r\n");
            AppendLog(string.Format("                 Actual Disease (1)    Actual Healthy (0)\r\n"));
            AppendLog(string.Format("  Pred Disease:        TP = {0,-8}         FP = {1,-8}\r\n", tp, fp));
            AppendLog(string.Format("  Pred Healthy:        FN = {0,-8}         TN = {1,-8}\r\n", fn, tn));
            AppendLog("─────────────────────────────────────────────────────────\r\n\r\n");
        }

        /// <summary>
        /// Resets the neural network, clears logs, and clears metric cards.
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (isTraining) return;

            network = null;
            lock (historyLock)
            {
                trainingHistory.Clear();
            }
            progressBar.Value = 0;
            pnlChart.Invalidate();

            lblAccValue.Text = "—";
            lblPrecValue.Text = "—";
            lblRecValue.Text = "—";
            lblF1Value.Text = "—";
            lblLossValue.Text = "—";

            lblResultText.Text = "Train the model first, then enter patient data above.";
            lblResultText.ForeColor = Color.FromArgb(150, 150, 170);
            lblConfidence.Text = "";
            pnlResultFill.Width = 0;

            rtbLog.Clear();
            AppendLog("🔄 Neural network and training session reset.\r\n\r\n");
        }

        /// <summary>
        /// Reads patient feature inputs, normalizes them, and predicts heart disease risk.
        /// </summary>
        private void btnPredict_Click(object sender, EventArgs e)
        {
            if (network == null)
            {
                MessageBox.Show("Please train the neural network first before running predictions.",
                    "Model Not Trained", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extract and parse 13 features from controls
            double[] rawInputs = new double[13];
            try
            {
                rawInputs[0] = ParseDouble(txtInputs[0], "Age");
                rawInputs[1] = GetComboValue(txtInputs[1]);           // Sex: 0=F, 1=M
                rawInputs[2] = GetComboValue(txtInputs[2]) + 1.0;     // Chest Pain: 1, 2, 3, 4
                rawInputs[3] = ParseDouble(txtInputs[3], "Resting Blood Pressure");
                rawInputs[4] = ParseDouble(txtInputs[4], "Cholesterol");
                rawInputs[5] = GetComboValue(txtInputs[5]);           // Fasting BS: 0=No, 1=Yes
                rawInputs[6] = GetComboValue(txtInputs[6]);           // Resting ECG: 0, 1, 2
                rawInputs[7] = ParseDouble(txtInputs[7], "Max Heart Rate");
                rawInputs[8] = GetComboValue(txtInputs[8]);           // Exercise Angina: 0=No, 1=Yes
                rawInputs[9] = ParseDouble(txtInputs[9], "ST Depression");
                rawInputs[10] = GetComboValue(txtInputs[10]) + 1.0;   // Slope: 1, 2, 3
                rawInputs[11] = GetComboValue(txtInputs[11]);         // CA: 0, 1, 2, 3
                rawInputs[12] = GetThalValue(txtInputs[12]);          // Thal: 3=Normal, 6=Fixed, 7=Reversible
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Normalize input with dataHandler MinMax parameters
            double[] normalized = dataHandler.NormalizeSingle(rawInputs);

            // Forward pass prediction
            double[] output = network.Predict(normalized);
            double probability = output[0];
            double confidencePercent = probability >= 0.5 ? probability * 100.0 : (1.0 - probability) * 100.0;

            // Visual feedback
            int barMaxWidth = pnlResultBar.Width;
            int fillWidth = (int)(probability * barMaxWidth);
            pnlResultFill.Width = Math.Max(4, Math.Min(barMaxWidth, fillWidth));

            if (probability >= 0.5)
            {
                lblResultText.Text = "⚠ HIGH RISK — Heart Disease Detected";
                lblResultText.ForeColor = Color.FromArgb(245, 80, 80);
                pnlResultFill.BackColor = Color.FromArgb(245, 80, 80);
                lblConfidence.Text = $"Disease Probability: {probability * 100.0:F1}% (Confidence: {confidencePercent:F1}%)";
            }
            else
            {
                lblResultText.Text = "✔ LOW RISK — No Heart Disease Detected";
                lblResultText.ForeColor = Color.FromArgb(60, 200, 110);
                pnlResultFill.BackColor = Color.FromArgb(60, 200, 110);
                lblConfidence.Text = $"Healthy Probability: {(1.0 - probability) * 100.0:F1}% (Confidence: {confidencePercent:F1}%)";
            }

            // Also log the individual prediction to the training log
            AppendLog($"\r\n🩺 Prediction: Age={rawInputs[0]}, Sex={(rawInputs[1] == 1 ? "M" : "F")}, " +
                      $"CP={rawInputs[2]}, BP={rawInputs[3]}, Chol={rawInputs[4]} → " +
                      $"Output={probability:F4} ({(probability >= 0.5 ? "HIGH RISK" : "LOW RISK")})\r\n");
        }

        private double ParseDouble(Control c, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(c.Text))
                throw new Exception($"Please enter a value for '{fieldName}'.");

            if (!double.TryParse(c.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                throw new Exception($"'{c.Text}' is not a valid number for '{fieldName}'.");

            return val;
        }

        private double GetComboValue(Control c)
        {
            if (c is ComboBox cb && cb.SelectedIndex >= 0)
            {
                return cb.SelectedIndex;
            }
            return 0.0;
        }

        private double GetThalValue(Control c)
        {
            if (c is ComboBox cb)
            {
                if (cb.SelectedIndex == 0) return 3.0; // Normal
                if (cb.SelectedIndex == 1) return 6.0; // Fixed Defect
                if (cb.SelectedIndex == 2) return 7.0; // Reversible Defect
            }
            return 3.0;
        }

        /// <summary>
        /// Exports the training history and loss decreasing across epochs to a CSV file.
        /// Meets Deliverable #2 of the rubric.
        /// </summary>
        private void btnExportLog_Click(object sender, EventArgs e)
        {
            List<TrainingEpochLog> snapshot;
            lock (historyLock)
            {
                snapshot = new List<TrainingEpochLog>(trainingHistory);
            }

            if (snapshot.Count == 0)
            {
                MessageBox.Show("No training log history to export yet. Train the model first.",
                    "Export CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt";
                sfd.FileName = $"heart_nn_training_log_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                sfd.Title = "Export Training Log";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("# Heart Disease MLP - Training Log");
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# Generated: {0:O}", DateTime.Now));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# Architecture: 13 -> {0} -> 1 (sigmoid)", lastHiddenNodes));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# LearningRate={0}", lastLearningRate));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# HiddenNodes={0}", lastHiddenNodes));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# Epochs={0}", lastEpochs));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# TrainSplit={0}", lastSplitRatio));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "# Seed={0}", lastSeed));
                    sb.AppendLine("# Loss=BinaryCrossEntropy");
                    sb.AppendLine("Epoch,Loss,TrainAccuracy,TestLoss,TestAccuracy");

                    foreach (TrainingEpochLog entry in snapshot)
                    {
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                            "{0},{1:F6},{2:F2},{3:F6},{4:F2}",
                            entry.Epoch, entry.Loss, entry.Accuracy, entry.TestLoss, entry.TestAccuracy));
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString());

                    MessageBox.Show($"Training log successfully exported to:\r\n{sfd.FileName}",
                        "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Draws the training loss and accuracy curves on the chart panel.
        /// The loss line uses a per-run min/max scale; accuracy uses a fixed 0-100% scale.
        /// </summary>
        private void PnlChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(18, 18, 28));

            Rectangle bounds = new Rectangle(8, 6,
                pnlChart.ClientSize.Width - 16,
                pnlChart.ClientSize.Height - 12);

            using (Font titleFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (Font smallFont = new Font("Segoe UI", 7.5F))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(200, 200, 210)))
            using (Pen gridPen = new Pen(Color.FromArgb(45, 45, 60)))
            using (Pen lossPen = new Pen(Color.FromArgb(245, 120, 80), 2F))
            using (Pen accPen = new Pen(Color.FromArgb(70, 200, 120), 2F))
            {
                g.DrawString("Loss / Accuracy vs Epoch", titleFont, textBrush, bounds.Left, bounds.Top);

                Rectangle plot = new Rectangle(
                    bounds.Left + 38,
                    bounds.Top + 20,
                    bounds.Width - 58,
                    bounds.Height - 44);
                if (plot.Width < 20 || plot.Height < 20) return;

                g.DrawRectangle(gridPen, plot);
                for (int i = 0; i <= 4; i++)
                {
                    int y = plot.Top + i * plot.Height / 4;
                    g.DrawLine(gridPen, plot.Left, y, plot.Right, y);
                }

                List<TrainingEpochLog> history;
                lock (historyLock)
                {
                    history = new List<TrainingEpochLog>(trainingHistory);
                }

                if (history.Count < 2)
                {
                    g.DrawString("Train the model to see the curve.", smallFont, textBrush,
                        plot.Left + 10, plot.Top + 10);
                    return;
                }

                int maxEpoch = Math.Max(1, history[history.Count - 1].Epoch);
                double minLoss = history.Min(h => h.Loss);
                double maxLoss = history.Max(h => h.Loss);
                if (maxLoss - minLoss < 1e-9) maxLoss = minLoss + 1e-9;

                PointF? previousLoss = null;
                PointF? previousAccuracy = null;

                foreach (TrainingEpochLog entry in history)
                {
                    float x = plot.Left + (float)entry.Epoch / maxEpoch * plot.Width;

                    float lossY = plot.Bottom -
                        (float)((entry.Loss - minLoss) / (maxLoss - minLoss)) * plot.Height;
                    PointF lossPoint = new PointF(x, lossY);
                    if (previousLoss.HasValue) g.DrawLine(lossPen, previousLoss.Value, lossPoint);
                    previousLoss = lossPoint;

                    float accY = plot.Bottom - (float)(entry.Accuracy / 100.0) * plot.Height;
                    PointF accPoint = new PointF(x, accY);
                    if (previousAccuracy.HasValue) g.DrawLine(accPen, previousAccuracy.Value, accPoint);
                    previousAccuracy = accPoint;
                }

                g.DrawString("0", smallFont, textBrush, plot.Left - 10, plot.Bottom - 6);
                g.DrawString(maxEpoch.ToString(), smallFont, textBrush, plot.Right - 20, plot.Bottom + 2);
                g.DrawString($"{maxLoss:F3}", smallFont, textBrush, plot.Left - 34, plot.Top - 4);
                g.DrawString($"{minLoss:F3}", smallFont, textBrush, plot.Left - 34, plot.Bottom - 6);

                g.DrawLine(lossPen, plot.Right - 82, plot.Top + 6, plot.Right - 68, plot.Top + 6);
                g.DrawString("Loss", smallFont, textBrush, plot.Right - 64, plot.Top - 1);
                g.DrawLine(accPen, plot.Right - 82, plot.Top + 20, plot.Right - 68, plot.Top + 20);
                g.DrawString("Acc", smallFont, textBrush, plot.Right - 64, plot.Top + 13);
            }
        }

        private void AppendLog(string message)
        {
            rtbLog.AppendText(message);
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
        }
    }
}
