namespace RealWorldPrediction
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblSubHeader = new System.Windows.Forms.Label();
            this.grpHyperparameters = new System.Windows.Forms.GroupBox();
            this.lblLearningRate = new System.Windows.Forms.Label();
            this.nudLearningRate = new System.Windows.Forms.NumericUpDown();
            this.lblHiddenNodes = new System.Windows.Forms.Label();
            this.nudHiddenNodes = new System.Windows.Forms.NumericUpDown();
            this.lblEpochs = new System.Windows.Forms.Label();
            this.nudEpochs = new System.Windows.Forms.NumericUpDown();
            this.lblTrainSplit = new System.Windows.Forms.Label();
            this.nudTrainSplit = new System.Windows.Forms.NumericUpDown();
            this.lblLogInterval = new System.Windows.Forms.Label();
            this.nudLogInterval = new System.Windows.Forms.NumericUpDown();
            this.grpTraining = new System.Windows.Forms.GroupBox();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnExportLog = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpMetrics = new System.Windows.Forms.GroupBox();
            this.lblAccTitle = new System.Windows.Forms.Label();
            this.lblAccValue = new System.Windows.Forms.Label();
            this.lblPrecTitle = new System.Windows.Forms.Label();
            this.lblPrecValue = new System.Windows.Forms.Label();
            this.lblRecTitle = new System.Windows.Forms.Label();
            this.lblRecValue = new System.Windows.Forms.Label();
            this.lblF1Title = new System.Windows.Forms.Label();
            this.lblF1Value = new System.Windows.Forms.Label();
            this.lblLossTitle = new System.Windows.Forms.Label();
            this.lblLossValue = new System.Windows.Forms.Label();
            this.grpDataInfo = new System.Windows.Forms.GroupBox();
            this.lblDataInfo = new System.Windows.Forms.Label();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.grpPatient = new System.Windows.Forms.GroupBox();
            this.btnPresetHigh = new System.Windows.Forms.Button();
            this.btnPresetLow = new System.Windows.Forms.Button();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblSex = new System.Windows.Forms.Label();
            this.cmbSex = new System.Windows.Forms.ComboBox();
            this.lblCp = new System.Windows.Forms.Label();
            this.cmbCp = new System.Windows.Forms.ComboBox();
            this.lblTrestbps = new System.Windows.Forms.Label();
            this.txtTrestbps = new System.Windows.Forms.TextBox();
            this.lblChol = new System.Windows.Forms.Label();
            this.txtChol = new System.Windows.Forms.TextBox();
            this.lblFbs = new System.Windows.Forms.Label();
            this.cmbFbs = new System.Windows.Forms.ComboBox();
            this.lblRestecg = new System.Windows.Forms.Label();
            this.cmbRestecg = new System.Windows.Forms.ComboBox();
            this.lblThalach = new System.Windows.Forms.Label();
            this.txtThalach = new System.Windows.Forms.TextBox();
            this.lblExang = new System.Windows.Forms.Label();
            this.cmbExang = new System.Windows.Forms.ComboBox();
            this.lblOldpeak = new System.Windows.Forms.Label();
            this.txtOldpeak = new System.Windows.Forms.TextBox();
            this.lblSlope = new System.Windows.Forms.Label();
            this.cmbSlope = new System.Windows.Forms.ComboBox();
            this.lblCa = new System.Windows.Forms.Label();
            this.cmbCa = new System.Windows.Forms.ComboBox();
            this.lblThal = new System.Windows.Forms.Label();
            this.cmbThal = new System.Windows.Forms.ComboBox();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.btnPredict = new System.Windows.Forms.Button();
            this.lblResultText = new System.Windows.Forms.Label();
            this.lblConfidence = new System.Windows.Forms.Label();
            this.pnlResultBar = new System.Windows.Forms.Panel();
            this.pnlResultFill = new System.Windows.Forms.Panel();
            this.grpHyperparameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLearningRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHiddenNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTrainSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogInterval)).BeginInit();
            this.grpTraining.SuspendLayout();
            this.grpMetrics.SuspendLayout();
            this.grpDataInfo.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.grpPatient.SuspendLayout();
            this.grpResult.SuspendLayout();
            this.pnlResultBar.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.lblHeader.Location = new System.Drawing.Point(20, 10);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(500, 35);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "❤  Heart Disease Prediction System";

            // 
            // lblSubHeader
            // 
            this.lblSubHeader.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSubHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(160)))));
            this.lblSubHeader.Location = new System.Drawing.Point(22, 42);
            this.lblSubHeader.Name = "lblSubHeader";
            this.lblSubHeader.Size = new System.Drawing.Size(700, 20);
            this.lblSubHeader.TabIndex = 1;
            this.lblSubHeader.Text = "Binary Classification using 3-Layer MLP  |  Cleveland Dataset (303 samples, 13 features)";

            // 
            // grpHyperparameters
            // 
            this.grpHyperparameters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpHyperparameters.Controls.Add(this.lblLearningRate);
            this.grpHyperparameters.Controls.Add(this.nudLearningRate);
            this.grpHyperparameters.Controls.Add(this.lblHiddenNodes);
            this.grpHyperparameters.Controls.Add(this.nudHiddenNodes);
            this.grpHyperparameters.Controls.Add(this.lblEpochs);
            this.grpHyperparameters.Controls.Add(this.nudEpochs);
            this.grpHyperparameters.Controls.Add(this.lblTrainSplit);
            this.grpHyperparameters.Controls.Add(this.nudTrainSplit);
            this.grpHyperparameters.Controls.Add(this.lblLogInterval);
            this.grpHyperparameters.Controls.Add(this.nudLogInterval);
            this.grpHyperparameters.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpHyperparameters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpHyperparameters.Location = new System.Drawing.Point(15, 68);
            this.grpHyperparameters.Name = "grpHyperparameters";
            this.grpHyperparameters.Size = new System.Drawing.Size(270, 200);
            this.grpHyperparameters.TabIndex = 2;
            this.grpHyperparameters.TabStop = false;
            this.grpHyperparameters.Text = "⚙  Hyperparameters";

            // 
            // lblLearningRate
            // 
            this.lblLearningRate.AutoSize = true;
            this.lblLearningRate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLearningRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblLearningRate.Location = new System.Drawing.Point(15, 25);
            this.lblLearningRate.Name = "lblLearningRate";
            this.lblLearningRate.Size = new System.Drawing.Size(82, 15);
            this.lblLearningRate.Text = "Learning Rate:";

            // 
            // nudLearningRate
            // 
            this.nudLearningRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.nudLearningRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudLearningRate.DecimalPlaces = 4;
            this.nudLearningRate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudLearningRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.nudLearningRate.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            this.nudLearningRate.Location = new System.Drawing.Point(150, 22);
            this.nudLearningRate.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudLearningRate.Minimum = new decimal(new int[] { 1, 0, 0, 262144 });
            this.nudLearningRate.Name = "nudLearningRate";
            this.nudLearningRate.Size = new System.Drawing.Size(100, 23);
            this.nudLearningRate.Value = new decimal(new int[] { 1, 0, 0, 65536 });

            // 
            // lblHiddenNodes
            // 
            this.lblHiddenNodes.AutoSize = true;
            this.lblHiddenNodes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHiddenNodes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblHiddenNodes.Location = new System.Drawing.Point(15, 55);
            this.lblHiddenNodes.Name = "lblHiddenNodes";
            this.lblHiddenNodes.Size = new System.Drawing.Size(86, 15);
            this.lblHiddenNodes.Text = "Hidden Nodes:";

            // 
            // nudHiddenNodes
            // 
            this.nudHiddenNodes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.nudHiddenNodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudHiddenNodes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudHiddenNodes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.nudHiddenNodes.Location = new System.Drawing.Point(150, 52);
            this.nudHiddenNodes.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            this.nudHiddenNodes.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            this.nudHiddenNodes.Name = "nudHiddenNodes";
            this.nudHiddenNodes.Size = new System.Drawing.Size(100, 23);
            this.nudHiddenNodes.Value = new decimal(new int[] { 16, 0, 0, 0 });

            // 
            // lblEpochs
            // 
            this.lblEpochs.AutoSize = true;
            this.lblEpochs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEpochs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblEpochs.Location = new System.Drawing.Point(15, 85);
            this.lblEpochs.Name = "lblEpochs";
            this.lblEpochs.Size = new System.Drawing.Size(48, 15);
            this.lblEpochs.Text = "Epochs:";

            // 
            // nudEpochs
            // 
            this.nudEpochs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.nudEpochs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudEpochs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudEpochs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.nudEpochs.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudEpochs.Location = new System.Drawing.Point(150, 82);
            this.nudEpochs.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            this.nudEpochs.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudEpochs.Name = "nudEpochs";
            this.nudEpochs.Size = new System.Drawing.Size(100, 23);
            this.nudEpochs.Value = new decimal(new int[] { 2000, 0, 0, 0 });

            // 
            // lblTrainSplit
            // 
            this.lblTrainSplit.AutoSize = true;
            this.lblTrainSplit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrainSplit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblTrainSplit.Location = new System.Drawing.Point(15, 115);
            this.lblTrainSplit.Name = "lblTrainSplit";
            this.lblTrainSplit.Size = new System.Drawing.Size(76, 15);
            this.lblTrainSplit.Text = "Train Split %:";

            // 
            // nudTrainSplit
            // 
            this.nudTrainSplit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.nudTrainSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudTrainSplit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudTrainSplit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.nudTrainSplit.Location = new System.Drawing.Point(150, 112);
            this.nudTrainSplit.Maximum = new decimal(new int[] { 95, 0, 0, 0 });
            this.nudTrainSplit.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            this.nudTrainSplit.Name = "nudTrainSplit";
            this.nudTrainSplit.Size = new System.Drawing.Size(100, 23);
            this.nudTrainSplit.Value = new decimal(new int[] { 80, 0, 0, 0 });

            // 
            // lblLogInterval
            // 
            this.lblLogInterval.AutoSize = true;
            this.lblLogInterval.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLogInterval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblLogInterval.Location = new System.Drawing.Point(15, 145);
            this.lblLogInterval.Name = "lblLogInterval";
            this.lblLogInterval.Size = new System.Drawing.Size(76, 15);
            this.lblLogInterval.Text = "Log Every N:";

            // 
            // nudLogInterval
            // 
            this.nudLogInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.nudLogInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudLogInterval.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudLogInterval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.nudLogInterval.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudLogInterval.Location = new System.Drawing.Point(150, 142);
            this.nudLogInterval.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudLogInterval.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudLogInterval.Name = "nudLogInterval";
            this.nudLogInterval.Size = new System.Drawing.Size(100, 23);
            this.nudLogInterval.Value = new decimal(new int[] { 100, 0, 0, 0 });

            // 
            // grpTraining
            // 
            this.grpTraining.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpTraining.Controls.Add(this.btnTrain);
            this.grpTraining.Controls.Add(this.btnStop);
            this.grpTraining.Controls.Add(this.btnReset);
            this.grpTraining.Controls.Add(this.btnExportLog);
            this.grpTraining.Controls.Add(this.progressBar);
            this.grpTraining.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpTraining.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpTraining.Location = new System.Drawing.Point(15, 275);
            this.grpTraining.Name = "grpTraining";
            this.grpTraining.Size = new System.Drawing.Size(270, 145);
            this.grpTraining.TabIndex = 3;
            this.grpTraining.TabStop = false;
            this.grpTraining.Text = "▶  Training Controls";

            // 
            // btnTrain
            // 
            this.btnTrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(200)))));
            this.btnTrain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrain.FlatAppearance.BorderSize = 0;
            this.btnTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrain.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTrain.ForeColor = System.Drawing.Color.White;
            this.btnTrain.Location = new System.Drawing.Point(15, 25);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(115, 34);
            this.btnTrain.TabIndex = 0;
            this.btnTrain.Text = "🚀 Train";
            this.btnTrain.UseVisualStyleBackColor = false;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);

            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(140, 25);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(115, 34);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "⏹ Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(50)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(15, 65);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(115, 30);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "🔄 Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // 
            // btnExportLog
            // 
            this.btnExportLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.btnExportLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportLog.FlatAppearance.BorderSize = 0;
            this.btnExportLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportLog.ForeColor = System.Drawing.Color.White;
            this.btnExportLog.Location = new System.Drawing.Point(140, 65);
            this.btnExportLog.Name = "btnExportLog";
            this.btnExportLog.Size = new System.Drawing.Size(115, 30);
            this.btnExportLog.TabIndex = 3;
            this.btnExportLog.Text = "📄 Export CSV";
            this.btnExportLog.UseVisualStyleBackColor = false;
            this.btnExportLog.Click += new System.EventHandler(this.btnExportLog_Click);

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 105);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(240, 22);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 4;

            // 
            // grpMetrics
            // 
            this.grpMetrics.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpMetrics.Controls.Add(this.lblAccTitle);
            this.grpMetrics.Controls.Add(this.lblAccValue);
            this.grpMetrics.Controls.Add(this.lblPrecTitle);
            this.grpMetrics.Controls.Add(this.lblPrecValue);
            this.grpMetrics.Controls.Add(this.lblRecTitle);
            this.grpMetrics.Controls.Add(this.lblRecValue);
            this.grpMetrics.Controls.Add(this.lblF1Title);
            this.grpMetrics.Controls.Add(this.lblF1Value);
            this.grpMetrics.Controls.Add(this.lblLossTitle);
            this.grpMetrics.Controls.Add(this.lblLossValue);
            this.grpMetrics.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpMetrics.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpMetrics.Location = new System.Drawing.Point(15, 427);
            this.grpMetrics.Name = "grpMetrics";
            this.grpMetrics.Size = new System.Drawing.Size(270, 195);
            this.grpMetrics.TabIndex = 4;
            this.grpMetrics.TabStop = false;
            this.grpMetrics.Text = "📊  Evaluation Metrics";

            // 
            // lblAccTitle
            // 
            this.lblAccTitle.AutoSize = true;
            this.lblAccTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAccTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblAccTitle.Location = new System.Drawing.Point(15, 28);
            this.lblAccTitle.Name = "lblAccTitle";
            this.lblAccTitle.Size = new System.Drawing.Size(59, 15);
            this.lblAccTitle.Text = "Accuracy:";

            // 
            // lblAccValue
            // 
            this.lblAccValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblAccValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lblAccValue.Location = new System.Drawing.Point(160, 28);
            this.lblAccValue.Name = "lblAccValue";
            this.lblAccValue.Size = new System.Drawing.Size(100, 22);
            this.lblAccValue.Text = "—";
            this.lblAccValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // lblPrecTitle
            // 
            this.lblPrecTitle.AutoSize = true;
            this.lblPrecTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPrecTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblPrecTitle.Location = new System.Drawing.Point(15, 58);
            this.lblPrecTitle.Name = "lblPrecTitle";
            this.lblPrecTitle.Size = new System.Drawing.Size(58, 15);
            this.lblPrecTitle.Text = "Precision:";

            // 
            // lblPrecValue
            // 
            this.lblPrecValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblPrecValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lblPrecValue.Location = new System.Drawing.Point(160, 58);
            this.lblPrecValue.Name = "lblPrecValue";
            this.lblPrecValue.Size = new System.Drawing.Size(100, 22);
            this.lblPrecValue.Text = "—";
            this.lblPrecValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // lblRecTitle
            // 
            this.lblRecTitle.AutoSize = true;
            this.lblRecTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRecTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblRecTitle.Location = new System.Drawing.Point(15, 88);
            this.lblRecTitle.Name = "lblRecTitle";
            this.lblRecTitle.Size = new System.Drawing.Size(41, 15);
            this.lblRecTitle.Text = "Recall:";

            // 
            // lblRecValue
            // 
            this.lblRecValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblRecValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lblRecValue.Location = new System.Drawing.Point(160, 88);
            this.lblRecValue.Name = "lblRecValue";
            this.lblRecValue.Size = new System.Drawing.Size(100, 22);
            this.lblRecValue.Text = "—";
            this.lblRecValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // lblF1Title
            // 
            this.lblF1Title.AutoSize = true;
            this.lblF1Title.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblF1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblF1Title.Location = new System.Drawing.Point(15, 118);
            this.lblF1Title.Name = "lblF1Title";
            this.lblF1Title.Size = new System.Drawing.Size(52, 15);
            this.lblF1Title.Text = "F1 Score:";

            // 
            // lblF1Value
            // 
            this.lblF1Value.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblF1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lblF1Value.Location = new System.Drawing.Point(160, 118);
            this.lblF1Value.Name = "lblF1Value";
            this.lblF1Value.Size = new System.Drawing.Size(100, 22);
            this.lblF1Value.Text = "—";
            this.lblF1Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // lblLossTitle
            // 
            this.lblLossTitle.AutoSize = true;
            this.lblLossTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLossTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblLossTitle.Location = new System.Drawing.Point(15, 148);
            this.lblLossTitle.Name = "lblLossTitle";
            this.lblLossTitle.Size = new System.Drawing.Size(61, 15);
            this.lblLossTitle.Text = "Final Loss:";

            // 
            // lblLossValue
            // 
            this.lblLossValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblLossValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lblLossValue.Location = new System.Drawing.Point(160, 148);
            this.lblLossValue.Name = "lblLossValue";
            this.lblLossValue.Size = new System.Drawing.Size(100, 22);
            this.lblLossValue.Text = "—";
            this.lblLossValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // grpDataInfo
            // 
            this.grpDataInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpDataInfo.Controls.Add(this.lblDataInfo);
            this.grpDataInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpDataInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpDataInfo.Location = new System.Drawing.Point(15, 629);
            this.grpDataInfo.Name = "grpDataInfo";
            this.grpDataInfo.Size = new System.Drawing.Size(270, 140);
            this.grpDataInfo.TabIndex = 5;
            this.grpDataInfo.TabStop = false;
            this.grpDataInfo.Text = "📁  Dataset Info";

            // 
            // lblDataInfo
            // 
            this.lblDataInfo.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lblDataInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(190)))));
            this.lblDataInfo.Location = new System.Drawing.Point(10, 22);
            this.lblDataInfo.Name = "lblDataInfo";
            this.lblDataInfo.Size = new System.Drawing.Size(250, 110);
            this.lblDataInfo.Text = "No data loaded yet.";

            // 
            // grpLog
            // 
            this.grpLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpLog.Controls.Add(this.rtbLog);
            this.grpLog.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpLog.Location = new System.Drawing.Point(295, 68);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(530, 700);
            this.grpLog.TabIndex = 6;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "📋  Training Log";

            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(28)))));
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtbLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.rtbLog.Location = new System.Drawing.Point(10, 22);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(508, 668);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";

            // 
            // grpPatient
            // 
            this.grpPatient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpPatient.Controls.Add(this.btnPresetHigh);
            this.grpPatient.Controls.Add(this.btnPresetLow);
            this.grpPatient.Controls.Add(this.lblAge);
            this.grpPatient.Controls.Add(this.txtAge);
            this.grpPatient.Controls.Add(this.lblSex);
            this.grpPatient.Controls.Add(this.cmbSex);
            this.grpPatient.Controls.Add(this.lblCp);
            this.grpPatient.Controls.Add(this.cmbCp);
            this.grpPatient.Controls.Add(this.lblTrestbps);
            this.grpPatient.Controls.Add(this.txtTrestbps);
            this.grpPatient.Controls.Add(this.lblChol);
            this.grpPatient.Controls.Add(this.txtChol);
            this.grpPatient.Controls.Add(this.lblFbs);
            this.grpPatient.Controls.Add(this.cmbFbs);
            this.grpPatient.Controls.Add(this.lblRestecg);
            this.grpPatient.Controls.Add(this.cmbRestecg);
            this.grpPatient.Controls.Add(this.lblThalach);
            this.grpPatient.Controls.Add(this.txtThalach);
            this.grpPatient.Controls.Add(this.lblExang);
            this.grpPatient.Controls.Add(this.cmbExang);
            this.grpPatient.Controls.Add(this.lblOldpeak);
            this.grpPatient.Controls.Add(this.txtOldpeak);
            this.grpPatient.Controls.Add(this.lblSlope);
            this.grpPatient.Controls.Add(this.cmbSlope);
            this.grpPatient.Controls.Add(this.lblCa);
            this.grpPatient.Controls.Add(this.cmbCa);
            this.grpPatient.Controls.Add(this.lblThal);
            this.grpPatient.Controls.Add(this.cmbThal);
            this.grpPatient.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpPatient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpPatient.Location = new System.Drawing.Point(835, 68);
            this.grpPatient.Name = "grpPatient";
            this.grpPatient.Size = new System.Drawing.Size(370, 530);
            this.grpPatient.TabIndex = 7;
            this.grpPatient.TabStop = false;
            this.grpPatient.Text = "🏥  Patient Data Input";

            // 
            // btnPresetHigh
            // 
            this.btnPresetHigh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.btnPresetHigh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPresetHigh.FlatAppearance.BorderSize = 0;
            this.btnPresetHigh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPresetHigh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPresetHigh.ForeColor = System.Drawing.Color.White;
            this.btnPresetHigh.Location = new System.Drawing.Point(15, 22);
            this.btnPresetHigh.Name = "btnPresetHigh";
            this.btnPresetHigh.Size = new System.Drawing.Size(165, 26);
            this.btnPresetHigh.TabIndex = 0;
            this.btnPresetHigh.Text = "🔴 High-Risk Sample";
            this.btnPresetHigh.UseVisualStyleBackColor = false;
            this.btnPresetHigh.Click += new System.EventHandler(this.btnPresetHigh_Click);

            // 
            // btnPresetLow
            // 
            this.btnPresetLow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(80)))), ((int)(((byte)(55)))));
            this.btnPresetLow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPresetLow.FlatAppearance.BorderSize = 0;
            this.btnPresetLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPresetLow.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPresetLow.ForeColor = System.Drawing.Color.White;
            this.btnPresetLow.Location = new System.Drawing.Point(190, 22);
            this.btnPresetLow.Name = "btnPresetLow";
            this.btnPresetLow.Size = new System.Drawing.Size(165, 26);
            this.btnPresetLow.TabIndex = 1;
            this.btnPresetLow.Text = "🟢 Low-Risk Sample";
            this.btnPresetLow.UseVisualStyleBackColor = false;
            this.btnPresetLow.Click += new System.EventHandler(this.btnPresetLow_Click);

            // 
            // lblAge
            // 
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblAge.Location = new System.Drawing.Point(12, 59);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(145, 20);
            this.lblAge.Text = "Age:";

            // 
            // txtAge
            // 
            this.txtAge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.txtAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.txtAge.Location = new System.Drawing.Point(160, 56);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(195, 23);
            this.txtAge.TabIndex = 2;

            // 
            // lblSex
            // 
            this.lblSex.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblSex.Location = new System.Drawing.Point(12, 95);
            this.lblSex.Name = "lblSex";
            this.lblSex.Size = new System.Drawing.Size(145, 20);
            this.lblSex.Text = "Sex:";

            // 
            // cmbSex
            // 
            this.cmbSex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSex.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbSex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbSex.FormattingEnabled = true;
            this.cmbSex.Items.AddRange(new object[] { "0 — Female", "1 — Male" });
            this.cmbSex.Location = new System.Drawing.Point(160, 92);
            this.cmbSex.Name = "cmbSex";
            this.cmbSex.Size = new System.Drawing.Size(195, 21);
            this.cmbSex.TabIndex = 3;

            // 
            // lblCp
            // 
            this.lblCp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblCp.Location = new System.Drawing.Point(12, 131);
            this.lblCp.Name = "lblCp";
            this.lblCp.Size = new System.Drawing.Size(145, 20);
            this.lblCp.Text = "Chest Pain Type:";

            // 
            // cmbCp
            // 
            this.cmbCp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbCp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCp.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbCp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbCp.FormattingEnabled = true;
            this.cmbCp.Items.AddRange(new object[] { "1 — Typical Angina", "2 — Atypical Angina", "3 — Non-Anginal Pain", "4 — Asymptomatic" });
            this.cmbCp.Location = new System.Drawing.Point(160, 128);
            this.cmbCp.Name = "cmbCp";
            this.cmbCp.Size = new System.Drawing.Size(195, 21);
            this.cmbCp.TabIndex = 4;

            // 
            // lblTrestbps
            // 
            this.lblTrestbps.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrestbps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblTrestbps.Location = new System.Drawing.Point(12, 167);
            this.lblTrestbps.Name = "lblTrestbps";
            this.lblTrestbps.Size = new System.Drawing.Size(145, 20);
            this.lblTrestbps.Text = "Resting BP (mmHg):";

            // 
            // txtTrestbps
            // 
            this.txtTrestbps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.txtTrestbps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTrestbps.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTrestbps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.txtTrestbps.Location = new System.Drawing.Point(160, 164);
            this.txtTrestbps.Name = "txtTrestbps";
            this.txtTrestbps.Size = new System.Drawing.Size(195, 23);
            this.txtTrestbps.TabIndex = 5;

            // 
            // lblChol
            // 
            this.lblChol.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblChol.Location = new System.Drawing.Point(12, 203);
            this.lblChol.Name = "lblChol";
            this.lblChol.Size = new System.Drawing.Size(145, 20);
            this.lblChol.Text = "Cholesterol (mg/dl):";

            // 
            // txtChol
            // 
            this.txtChol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.txtChol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChol.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtChol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.txtChol.Location = new System.Drawing.Point(160, 200);
            this.txtChol.Name = "txtChol";
            this.txtChol.Size = new System.Drawing.Size(195, 23);
            this.txtChol.TabIndex = 6;

            // 
            // lblFbs
            // 
            this.lblFbs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFbs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblFbs.Location = new System.Drawing.Point(12, 239);
            this.lblFbs.Name = "lblFbs";
            this.lblFbs.Size = new System.Drawing.Size(145, 20);
            this.lblFbs.Text = "Fasting BS > 120:";

            // 
            // cmbFbs
            // 
            this.cmbFbs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbFbs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFbs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFbs.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbFbs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbFbs.FormattingEnabled = true;
            this.cmbFbs.Items.AddRange(new object[] { "0 — No", "1 — Yes" });
            this.cmbFbs.Location = new System.Drawing.Point(160, 236);
            this.cmbFbs.Name = "cmbFbs";
            this.cmbFbs.Size = new System.Drawing.Size(195, 21);
            this.cmbFbs.TabIndex = 7;

            // 
            // lblRestecg
            // 
            this.lblRestecg.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRestecg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblRestecg.Location = new System.Drawing.Point(12, 275);
            this.lblRestecg.Name = "lblRestecg";
            this.lblRestecg.Size = new System.Drawing.Size(145, 20);
            this.lblRestecg.Text = "Resting ECG:";

            // 
            // cmbRestecg
            // 
            this.cmbRestecg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbRestecg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRestecg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRestecg.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbRestecg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbRestecg.FormattingEnabled = true;
            this.cmbRestecg.Items.AddRange(new object[] { "0 — Normal", "1 — ST-T Abnormality", "2 — LV Hypertrophy" });
            this.cmbRestecg.Location = new System.Drawing.Point(160, 272);
            this.cmbRestecg.Name = "cmbRestecg";
            this.cmbRestecg.Size = new System.Drawing.Size(195, 21);
            this.cmbRestecg.TabIndex = 8;

            // 
            // lblThalach
            // 
            this.lblThalach.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblThalach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblThalach.Location = new System.Drawing.Point(12, 311);
            this.lblThalach.Name = "lblThalach";
            this.lblThalach.Size = new System.Drawing.Size(145, 20);
            this.lblThalach.Text = "Max Heart Rate:";

            // 
            // txtThalach
            // 
            this.txtThalach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.txtThalach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtThalach.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtThalach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.txtThalach.Location = new System.Drawing.Point(160, 308);
            this.txtThalach.Name = "txtThalach";
            this.txtThalach.Size = new System.Drawing.Size(195, 23);
            this.txtThalach.TabIndex = 9;

            // 
            // lblExang
            // 
            this.lblExang.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblExang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblExang.Location = new System.Drawing.Point(12, 347);
            this.lblExang.Name = "lblExang";
            this.lblExang.Size = new System.Drawing.Size(145, 20);
            this.lblExang.Text = "Exercise Angina:";

            // 
            // cmbExang
            // 
            this.cmbExang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbExang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbExang.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbExang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbExang.FormattingEnabled = true;
            this.cmbExang.Items.AddRange(new object[] { "0 — No", "1 — Yes" });
            this.cmbExang.Location = new System.Drawing.Point(160, 344);
            this.cmbExang.Name = "cmbExang";
            this.cmbExang.Size = new System.Drawing.Size(195, 21);
            this.cmbExang.TabIndex = 10;

            // 
            // lblOldpeak
            // 
            this.lblOldpeak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOldpeak.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblOldpeak.Location = new System.Drawing.Point(12, 383);
            this.lblOldpeak.Name = "lblOldpeak";
            this.lblOldpeak.Size = new System.Drawing.Size(145, 20);
            this.lblOldpeak.Text = "ST Depression:";

            // 
            // txtOldpeak
            // 
            this.txtOldpeak.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.txtOldpeak.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldpeak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOldpeak.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.txtOldpeak.Location = new System.Drawing.Point(160, 380);
            this.txtOldpeak.Name = "txtOldpeak";
            this.txtOldpeak.Size = new System.Drawing.Size(195, 23);
            this.txtOldpeak.TabIndex = 11;

            // 
            // lblSlope
            // 
            this.lblSlope.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSlope.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblSlope.Location = new System.Drawing.Point(12, 419);
            this.lblSlope.Name = "lblSlope";
            this.lblSlope.Size = new System.Drawing.Size(145, 20);
            this.lblSlope.Text = "ST Slope:";

            // 
            // cmbSlope
            // 
            this.cmbSlope.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbSlope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSlope.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSlope.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbSlope.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbSlope.FormattingEnabled = true;
            this.cmbSlope.Items.AddRange(new object[] { "1 — Upsloping", "2 — Flat", "3 — Downsloping" });
            this.cmbSlope.Location = new System.Drawing.Point(160, 416);
            this.cmbSlope.Name = "cmbSlope";
            this.cmbSlope.Size = new System.Drawing.Size(195, 21);
            this.cmbSlope.TabIndex = 12;

            // 
            // lblCa
            // 
            this.lblCa.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblCa.Location = new System.Drawing.Point(12, 455);
            this.lblCa.Name = "lblCa";
            this.lblCa.Size = new System.Drawing.Size(145, 20);
            this.lblCa.Text = "Major Vessels (ca):";

            // 
            // cmbCa
            // 
            this.cmbCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbCa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCa.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbCa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbCa.FormattingEnabled = true;
            this.cmbCa.Items.AddRange(new object[] { "0", "1", "2", "3" });
            this.cmbCa.Location = new System.Drawing.Point(160, 452);
            this.cmbCa.Name = "cmbCa";
            this.cmbCa.Size = new System.Drawing.Size(195, 21);
            this.cmbCa.TabIndex = 13;

            // 
            // lblThal
            // 
            this.lblThal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblThal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblThal.Location = new System.Drawing.Point(12, 491);
            this.lblThal.Name = "lblThal";
            this.lblThal.Size = new System.Drawing.Size(145, 20);
            this.lblThal.Text = "Thal:";

            // 
            // cmbThal
            // 
            this.cmbThal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.cmbThal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbThal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbThal.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbThal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.cmbThal.FormattingEnabled = true;
            this.cmbThal.Items.AddRange(new object[] { "3 — Normal", "6 — Fixed Defect", "7 — Reversible Defect" });
            this.cmbThal.Location = new System.Drawing.Point(160, 488);
            this.cmbThal.Name = "cmbThal";
            this.cmbThal.Size = new System.Drawing.Size(195, 21);
            this.cmbThal.TabIndex = 14;

            // 
            // grpResult
            // 
            this.grpResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(42)))));
            this.grpResult.Controls.Add(this.btnPredict);
            this.grpResult.Controls.Add(this.lblResultText);
            this.grpResult.Controls.Add(this.lblConfidence);
            this.grpResult.Controls.Add(this.pnlResultBar);
            this.grpResult.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.grpResult.Location = new System.Drawing.Point(835, 605);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(370, 163);
            this.grpResult.TabIndex = 8;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "🔮  Prediction Result";

            // 
            // btnPredict
            // 
            this.btnPredict.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(160)))), ((int)(((byte)(90)))));
            this.btnPredict.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPredict.FlatAppearance.BorderSize = 0;
            this.btnPredict.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPredict.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPredict.ForeColor = System.Drawing.Color.White;
            this.btnPredict.Location = new System.Drawing.Point(15, 25);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(340, 38);
            this.btnPredict.TabIndex = 0;
            this.btnPredict.Text = "🔍 Predict";
            this.btnPredict.UseVisualStyleBackColor = false;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);

            // 
            // lblResultText
            // 
            this.lblResultText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblResultText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(170)))));
            this.lblResultText.Location = new System.Drawing.Point(15, 68);
            this.lblResultText.Name = "lblResultText";
            this.lblResultText.Size = new System.Drawing.Size(340, 38);
            this.lblResultText.TabIndex = 1;
            this.lblResultText.Text = "Train the model first, then enter patient data above.";
            this.lblResultText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblConfidence
            // 
            this.lblConfidence.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblConfidence.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(160)))));
            this.lblConfidence.Location = new System.Drawing.Point(15, 110);
            this.lblConfidence.Name = "lblConfidence";
            this.lblConfidence.Size = new System.Drawing.Size(340, 22);
            this.lblConfidence.TabIndex = 2;
            this.lblConfidence.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // pnlResultBar
            // 
            this.pnlResultBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.pnlResultBar.Controls.Add(this.pnlResultFill);
            this.pnlResultBar.Location = new System.Drawing.Point(15, 138);
            this.pnlResultBar.Name = "pnlResultBar";
            this.pnlResultBar.Size = new System.Drawing.Size(340, 8);
            this.pnlResultBar.TabIndex = 3;

            // 
            // pnlResultFill
            // 
            this.pnlResultFill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(160)))), ((int)(((byte)(90)))));
            this.pnlResultFill.Location = new System.Drawing.Point(0, 0);
            this.pnlResultFill.Name = "pnlResultFill";
            this.pnlResultFill.Size = new System.Drawing.Size(0, 8);
            this.pnlResultFill.TabIndex = 0;

            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(1220, 780);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblSubHeader);
            this.Controls.Add(this.grpHyperparameters);
            this.Controls.Add(this.grpTraining);
            this.Controls.Add(this.grpMetrics);
            this.Controls.Add(this.grpDataInfo);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpPatient);
            this.Controls.Add(this.grpResult);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1220, 780);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Heart Disease Prediction — Neural Network Decision Support System";
            this.grpHyperparameters.ResumeLayout(false);
            this.grpHyperparameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLearningRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHiddenNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTrainSplit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogInterval)).EndInit();
            this.grpTraining.ResumeLayout(false);
            this.grpMetrics.ResumeLayout(false);
            this.grpMetrics.PerformLayout();
            this.grpDataInfo.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.grpPatient.ResumeLayout(false);
            this.grpPatient.PerformLayout();
            this.grpResult.ResumeLayout(false);
            this.pnlResultBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // Top Header
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubHeader;

        // Hyperparameters Group
        private System.Windows.Forms.GroupBox grpHyperparameters;
        private System.Windows.Forms.Label lblLearningRate;
        private System.Windows.Forms.NumericUpDown nudLearningRate;
        private System.Windows.Forms.Label lblHiddenNodes;
        private System.Windows.Forms.NumericUpDown nudHiddenNodes;
        private System.Windows.Forms.Label lblEpochs;
        private System.Windows.Forms.NumericUpDown nudEpochs;
        private System.Windows.Forms.Label lblTrainSplit;
        private System.Windows.Forms.NumericUpDown nudTrainSplit;
        private System.Windows.Forms.Label lblLogInterval;
        private System.Windows.Forms.NumericUpDown nudLogInterval;

        // Training Controls Group
        private System.Windows.Forms.GroupBox grpTraining;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnExportLog;
        private System.Windows.Forms.ProgressBar progressBar;

        // Evaluation Metrics Group
        private System.Windows.Forms.GroupBox grpMetrics;
        private System.Windows.Forms.Label lblAccTitle;
        private System.Windows.Forms.Label lblAccValue;
        private System.Windows.Forms.Label lblPrecTitle;
        private System.Windows.Forms.Label lblPrecValue;
        private System.Windows.Forms.Label lblRecTitle;
        private System.Windows.Forms.Label lblRecValue;
        private System.Windows.Forms.Label lblF1Title;
        private System.Windows.Forms.Label lblF1Value;
        private System.Windows.Forms.Label lblLossTitle;
        private System.Windows.Forms.Label lblLossValue;

        // Dataset Info Group
        private System.Windows.Forms.GroupBox grpDataInfo;
        private System.Windows.Forms.Label lblDataInfo;

        // Training Log Group
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.RichTextBox rtbLog;

        // Patient Data Input Group
        private System.Windows.Forms.GroupBox grpPatient;
        private System.Windows.Forms.Button btnPresetHigh;
        private System.Windows.Forms.Button btnPresetLow;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblSex;
        private System.Windows.Forms.ComboBox cmbSex;
        private System.Windows.Forms.Label lblCp;
        private System.Windows.Forms.ComboBox cmbCp;
        private System.Windows.Forms.Label lblTrestbps;
        private System.Windows.Forms.TextBox txtTrestbps;
        private System.Windows.Forms.Label lblChol;
        private System.Windows.Forms.TextBox txtChol;
        private System.Windows.Forms.Label lblFbs;
        private System.Windows.Forms.ComboBox cmbFbs;
        private System.Windows.Forms.Label lblRestecg;
        private System.Windows.Forms.ComboBox cmbRestecg;
        private System.Windows.Forms.Label lblThalach;
        private System.Windows.Forms.TextBox txtThalach;
        private System.Windows.Forms.Label lblExang;
        private System.Windows.Forms.ComboBox cmbExang;
        private System.Windows.Forms.Label lblOldpeak;
        private System.Windows.Forms.TextBox txtOldpeak;
        private System.Windows.Forms.Label lblSlope;
        private System.Windows.Forms.ComboBox cmbSlope;
        private System.Windows.Forms.Label lblCa;
        private System.Windows.Forms.ComboBox cmbCa;
        private System.Windows.Forms.Label lblThal;
        private System.Windows.Forms.ComboBox cmbThal;

        // Prediction Result Group
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Label lblResultText;
        private System.Windows.Forms.Label lblConfidence;
        private System.Windows.Forms.Panel pnlResultBar;
        private System.Windows.Forms.Panel pnlResultFill;
    }
}
