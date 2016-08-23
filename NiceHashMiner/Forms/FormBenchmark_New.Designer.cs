namespace NiceHashMiner.Forms {
    partial class FormBenchmark_New {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.StartStopBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.radioButton_SelectedUnbenchmarked = new System.Windows.Forms.RadioButton();
            this.groupBoxAlgorithmBenchmarkSettings = new System.Windows.Forms.GroupBox();
            this.radioButton_All = new System.Windows.Forms.RadioButton();
            this.radioButton_ReOnlySelected = new System.Windows.Forms.RadioButton();
            this.radioButton_Unbenchmarked = new System.Windows.Forms.RadioButton();
            this.labelBenchmarkDevice = new System.Windows.Forms.Label();
            this.groupBoxBenchmarkProgress = new System.Windows.Forms.GroupBox();
            this.groupBoxBenchmarkingAlgoStats = new System.Windows.Forms.GroupBox();
            this.labelAlgorithmStatus = new System.Windows.Forms.Label();
            this.labelWaitTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBarAlgorithmTime = new System.Windows.Forms.ProgressBar();
            this.labelBenchmarkSteps = new System.Windows.Forms.Label();
            this.progressBarBenchmarkSteps = new System.Windows.Forms.ProgressBar();
            this.labelPreviousAlgorithmStatus = new System.Windows.Forms.Label();
            this.benchmarkOptions1 = new NiceHashMiner.Forms.Components.BenchmarkOptions();
            this.devicesListViewEnableControl1 = new NiceHashMiner.Forms.Components.DevicesListViewEnableControl();
            this.groupBoxAlgorithmBenchmarkSettings.SuspendLayout();
            this.groupBoxBenchmarkProgress.SuspendLayout();
            this.groupBoxBenchmarkingAlgoStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select devices to benchmark.";
            // 
            // StartStopBtn
            // 
            this.StartStopBtn.Location = new System.Drawing.Point(387, 559);
            this.StartStopBtn.Name = "StartStopBtn";
            this.StartStopBtn.Size = new System.Drawing.Size(75, 23);
            this.StartStopBtn.TabIndex = 100;
            this.StartStopBtn.Text = "&Start";
            this.StartStopBtn.UseVisualStyleBackColor = true;
            this.StartStopBtn.Click += new System.EventHandler(this.StartStopBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(468, 559);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 101;
            this.CloseBtn.Text = "&Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // radioButton_SelectedUnbenchmarked
            // 
            this.radioButton_SelectedUnbenchmarked.AutoSize = true;
            this.radioButton_SelectedUnbenchmarked.Checked = true;
            this.radioButton_SelectedUnbenchmarked.Location = new System.Drawing.Point(6, 19);
            this.radioButton_SelectedUnbenchmarked.Name = "radioButton_SelectedUnbenchmarked";
            this.radioButton_SelectedUnbenchmarked.Size = new System.Drawing.Size(275, 17);
            this.radioButton_SelectedUnbenchmarked.TabIndex = 104;
            this.radioButton_SelectedUnbenchmarked.TabStop = true;
            this.radioButton_SelectedUnbenchmarked.Text = "Benchmark only selected unbenchmarked Algorithms";
            this.radioButton_SelectedUnbenchmarked.UseVisualStyleBackColor = true;
            this.radioButton_SelectedUnbenchmarked.CheckedChanged += new System.EventHandler(this.radioButton_SelectedUnbenchmarked_CheckedChanged);
            // 
            // groupBoxAlgorithmBenchmarkSettings
            // 
            this.groupBoxAlgorithmBenchmarkSettings.Controls.Add(this.radioButton_All);
            this.groupBoxAlgorithmBenchmarkSettings.Controls.Add(this.radioButton_ReOnlySelected);
            this.groupBoxAlgorithmBenchmarkSettings.Controls.Add(this.radioButton_Unbenchmarked);
            this.groupBoxAlgorithmBenchmarkSettings.Controls.Add(this.radioButton_SelectedUnbenchmarked);
            this.groupBoxAlgorithmBenchmarkSettings.Location = new System.Drawing.Point(12, 219);
            this.groupBoxAlgorithmBenchmarkSettings.Name = "groupBoxAlgorithmBenchmarkSettings";
            this.groupBoxAlgorithmBenchmarkSettings.Size = new System.Drawing.Size(325, 115);
            this.groupBoxAlgorithmBenchmarkSettings.TabIndex = 105;
            this.groupBoxAlgorithmBenchmarkSettings.TabStop = false;
            this.groupBoxAlgorithmBenchmarkSettings.Text = "Benchmark Algoruthms Settings:";
            // 
            // radioButton_All
            // 
            this.radioButton_All.AutoSize = true;
            this.radioButton_All.Location = new System.Drawing.Point(6, 88);
            this.radioButton_All.Name = "radioButton_All";
            this.radioButton_All.Size = new System.Drawing.Size(144, 17);
            this.radioButton_All.TabIndex = 106;
            this.radioButton_All.TabStop = true;
            this.radioButton_All.Text = "Benchmark All Algorithms";
            this.radioButton_All.UseVisualStyleBackColor = true;
            this.radioButton_All.CheckedChanged += new System.EventHandler(this.radioButton_All_CheckedChanged);
            // 
            // radioButton_ReOnlySelected
            // 
            this.radioButton_ReOnlySelected.AutoSize = true;
            this.radioButton_ReOnlySelected.Location = new System.Drawing.Point(6, 65);
            this.radioButton_ReOnlySelected.Name = "radioButton_ReOnlySelected";
            this.radioButton_ReOnlySelected.Size = new System.Drawing.Size(205, 17);
            this.radioButton_ReOnlySelected.TabIndex = 106;
            this.radioButton_ReOnlySelected.TabStop = true;
            this.radioButton_ReOnlySelected.Text = "Re-bencmark only selected Algorithms";
            this.radioButton_ReOnlySelected.UseVisualStyleBackColor = true;
            this.radioButton_ReOnlySelected.CheckedChanged += new System.EventHandler(this.radioButton_ReOnlySelected_CheckedChanged);
            // 
            // radioButton_Unbenchmarked
            // 
            this.radioButton_Unbenchmarked.AutoSize = true;
            this.radioButton_Unbenchmarked.Location = new System.Drawing.Point(6, 42);
            this.radioButton_Unbenchmarked.Name = "radioButton_Unbenchmarked";
            this.radioButton_Unbenchmarked.Size = new System.Drawing.Size(223, 17);
            this.radioButton_Unbenchmarked.TabIndex = 105;
            this.radioButton_Unbenchmarked.Text = "Benchmark all unbenchmarked Algorithms";
            this.radioButton_Unbenchmarked.UseVisualStyleBackColor = true;
            this.radioButton_Unbenchmarked.CheckedChanged += new System.EventHandler(this.radioButton_Unbenchmarked_CheckedChanged);
            // 
            // labelBenchmarkDevice
            // 
            this.labelBenchmarkDevice.AutoSize = true;
            this.labelBenchmarkDevice.Location = new System.Drawing.Point(6, 54);
            this.labelBenchmarkDevice.Name = "labelBenchmarkDevice";
            this.labelBenchmarkDevice.Size = new System.Drawing.Size(186, 13);
            this.labelBenchmarkDevice.TabIndex = 107;
            this.labelBenchmarkDevice.Text = "Current Benchmarking Device: NONE";
            // 
            // groupBoxBenchmarkProgress
            // 
            this.groupBoxBenchmarkProgress.Controls.Add(this.groupBoxBenchmarkingAlgoStats);
            this.groupBoxBenchmarkProgress.Controls.Add(this.labelBenchmarkSteps);
            this.groupBoxBenchmarkProgress.Controls.Add(this.progressBarBenchmarkSteps);
            this.groupBoxBenchmarkProgress.Controls.Add(this.labelPreviousAlgorithmStatus);
            this.groupBoxBenchmarkProgress.Controls.Add(this.labelBenchmarkDevice);
            this.groupBoxBenchmarkProgress.Location = new System.Drawing.Point(12, 340);
            this.groupBoxBenchmarkProgress.Name = "groupBoxBenchmarkProgress";
            this.groupBoxBenchmarkProgress.Size = new System.Drawing.Size(531, 213);
            this.groupBoxBenchmarkProgress.TabIndex = 108;
            this.groupBoxBenchmarkProgress.TabStop = false;
            this.groupBoxBenchmarkProgress.Text = "Benchmark progress status:";
            // 
            // groupBoxBenchmarkingAlgoStats
            // 
            this.groupBoxBenchmarkingAlgoStats.Controls.Add(this.labelAlgorithmStatus);
            this.groupBoxBenchmarkingAlgoStats.Controls.Add(this.labelWaitTime);
            this.groupBoxBenchmarkingAlgoStats.Controls.Add(this.label2);
            this.groupBoxBenchmarkingAlgoStats.Controls.Add(this.progressBarAlgorithmTime);
            this.groupBoxBenchmarkingAlgoStats.Location = new System.Drawing.Point(0, 106);
            this.groupBoxBenchmarkingAlgoStats.Name = "groupBoxBenchmarkingAlgoStats";
            this.groupBoxBenchmarkingAlgoStats.Size = new System.Drawing.Size(525, 100);
            this.groupBoxBenchmarkingAlgoStats.TabIndex = 112;
            this.groupBoxBenchmarkingAlgoStats.TabStop = false;
            this.groupBoxBenchmarkingAlgoStats.Text = "Current Benchmarking Algorithm: NONE";
            // 
            // labelAlgorithmStatus
            // 
            this.labelAlgorithmStatus.AutoSize = true;
            this.labelAlgorithmStatus.Location = new System.Drawing.Point(9, 44);
            this.labelAlgorithmStatus.Name = "labelAlgorithmStatus";
            this.labelAlgorithmStatus.Size = new System.Drawing.Size(74, 13);
            this.labelAlgorithmStatus.TabIndex = 113;
            this.labelAlgorithmStatus.Text = "Status: NONE";
            // 
            // labelWaitTime
            // 
            this.labelWaitTime.AutoSize = true;
            this.labelWaitTime.Location = new System.Drawing.Point(9, 20);
            this.labelWaitTime.Name = "labelWaitTime";
            this.labelWaitTime.Size = new System.Drawing.Size(88, 13);
            this.labelWaitTime.TabIndex = 112;
            this.labelWaitTime.Text = "Wait time: NONE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 110;
            this.label2.Text = "Progress: ";
            // 
            // progressBarAlgorithmTime
            // 
            this.progressBarAlgorithmTime.Location = new System.Drawing.Point(69, 69);
            this.progressBarAlgorithmTime.Name = "progressBarAlgorithmTime";
            this.progressBarAlgorithmTime.Size = new System.Drawing.Size(168, 23);
            this.progressBarAlgorithmTime.TabIndex = 111;
            // 
            // labelBenchmarkSteps
            // 
            this.labelBenchmarkSteps.AutoSize = true;
            this.labelBenchmarkSteps.Location = new System.Drawing.Point(33, 19);
            this.labelBenchmarkSteps.Name = "labelBenchmarkSteps";
            this.labelBenchmarkSteps.Size = new System.Drawing.Size(116, 13);
            this.labelBenchmarkSteps.TabIndex = 109;
            this.labelBenchmarkSteps.Text = "Benchmark step (0/10)";
            // 
            // progressBarBenchmarkSteps
            // 
            this.progressBarBenchmarkSteps.Location = new System.Drawing.Point(163, 19);
            this.progressBarBenchmarkSteps.Name = "progressBarBenchmarkSteps";
            this.progressBarBenchmarkSteps.Size = new System.Drawing.Size(362, 23);
            this.progressBarBenchmarkSteps.TabIndex = 108;
            // 
            // labelPreviousAlgorithmStatus
            // 
            this.labelPreviousAlgorithmStatus.AutoSize = true;
            this.labelPreviousAlgorithmStatus.Location = new System.Drawing.Point(6, 79);
            this.labelPreviousAlgorithmStatus.Name = "labelPreviousAlgorithmStatus";
            this.labelPreviousAlgorithmStatus.Size = new System.Drawing.Size(227, 13);
            this.labelPreviousAlgorithmStatus.TabIndex = 107;
            this.labelPreviousAlgorithmStatus.Text = "Previous Benchmared Algorithm Status: NONE";
            // 
            // benchmarkOptions1
            // 
            this.benchmarkOptions1.Location = new System.Drawing.Point(343, 219);
            this.benchmarkOptions1.Name = "benchmarkOptions1";
            this.benchmarkOptions1.Size = new System.Drawing.Size(208, 115);
            this.benchmarkOptions1.TabIndex = 106;
            // 
            // devicesListViewEnableControl1
            // 
            this.devicesListViewEnableControl1.AutoSaveChange = false;
            this.devicesListViewEnableControl1.FirstColumnText = "Benckmark";
            this.devicesListViewEnableControl1.Location = new System.Drawing.Point(12, 45);
            this.devicesListViewEnableControl1.Name = "devicesListViewEnableControl1";
            this.devicesListViewEnableControl1.SaveToGeneralConfig = false;
            this.devicesListViewEnableControl1.SetAllEnabled = false;
            this.devicesListViewEnableControl1.Size = new System.Drawing.Size(531, 156);
            this.devicesListViewEnableControl1.TabIndex = 0;
            // 
            // FormBenchmark_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 590);
            this.Controls.Add(this.groupBoxBenchmarkProgress);
            this.Controls.Add(this.benchmarkOptions1);
            this.Controls.Add(this.groupBoxAlgorithmBenchmarkSettings);
            this.Controls.Add(this.StartStopBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.devicesListViewEnableControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBenchmark_New";
            this.Text = "Benchmark";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBenchmark_New_FormClosing);
            this.groupBoxAlgorithmBenchmarkSettings.ResumeLayout(false);
            this.groupBoxAlgorithmBenchmarkSettings.PerformLayout();
            this.groupBoxBenchmarkProgress.ResumeLayout(false);
            this.groupBoxBenchmarkProgress.PerformLayout();
            this.groupBoxBenchmarkingAlgoStats.ResumeLayout(false);
            this.groupBoxBenchmarkingAlgoStats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.DevicesListViewEnableControl devicesListViewEnableControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartStopBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.RadioButton radioButton_SelectedUnbenchmarked;
        private System.Windows.Forms.GroupBox groupBoxAlgorithmBenchmarkSettings;
        private System.Windows.Forms.RadioButton radioButton_All;
        private System.Windows.Forms.RadioButton radioButton_ReOnlySelected;
        private System.Windows.Forms.RadioButton radioButton_Unbenchmarked;
        private Components.BenchmarkOptions benchmarkOptions1;
        private System.Windows.Forms.Label labelBenchmarkDevice;
        private System.Windows.Forms.GroupBox groupBoxBenchmarkProgress;
        private System.Windows.Forms.Label labelBenchmarkSteps;
        private System.Windows.Forms.ProgressBar progressBarBenchmarkSteps;
        private System.Windows.Forms.ProgressBar progressBarAlgorithmTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxBenchmarkingAlgoStats;
        private System.Windows.Forms.Label labelAlgorithmStatus;
        private System.Windows.Forms.Label labelWaitTime;
        private System.Windows.Forms.Label labelPreviousAlgorithmStatus;


    }
}