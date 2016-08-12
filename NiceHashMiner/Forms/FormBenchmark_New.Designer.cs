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
            this.devicesListViewEnableControl1 = new NiceHashMiner.Forms.Components.DevicesListViewEnableControl();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelProgressPercentage = new System.Windows.Forms.Label();
            this.StartStopBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.BenchmarkProgressBar = new System.Windows.Forms.ProgressBar();
            this.radioButton_SelectedUnbenchmarked = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_All = new System.Windows.Forms.RadioButton();
            this.radioButton_ReOnlySelected = new System.Windows.Forms.RadioButton();
            this.radioButton_Unbenchmarked = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select devices to benchmark.";
            // 
            // LabelProgressPercentage
            // 
            this.LabelProgressPercentage.BackColor = System.Drawing.SystemColors.Control;
            this.LabelProgressPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelProgressPercentage.Location = new System.Drawing.Point(12, 459);
            this.LabelProgressPercentage.Name = "LabelProgressPercentage";
            this.LabelProgressPercentage.Size = new System.Drawing.Size(293, 23);
            this.LabelProgressPercentage.TabIndex = 102;
            this.LabelProgressPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StartStopBtn
            // 
            this.StartStopBtn.Location = new System.Drawing.Point(387, 456);
            this.StartStopBtn.Name = "StartStopBtn";
            this.StartStopBtn.Size = new System.Drawing.Size(75, 23);
            this.StartStopBtn.TabIndex = 100;
            this.StartStopBtn.Text = "&Start";
            this.StartStopBtn.UseVisualStyleBackColor = true;
            this.StartStopBtn.Click += new System.EventHandler(this.StartStopBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(468, 456);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 101;
            this.CloseBtn.Text = "&Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // BenchmarkProgressBar
            // 
            this.BenchmarkProgressBar.Location = new System.Drawing.Point(12, 427);
            this.BenchmarkProgressBar.Maximum = 10;
            this.BenchmarkProgressBar.Name = "BenchmarkProgressBar";
            this.BenchmarkProgressBar.Size = new System.Drawing.Size(531, 23);
            this.BenchmarkProgressBar.Step = 1;
            this.BenchmarkProgressBar.TabIndex = 103;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_All);
            this.groupBox1.Controls.Add(this.radioButton_ReOnlySelected);
            this.groupBox1.Controls.Add(this.radioButton_Unbenchmarked);
            this.groupBox1.Controls.Add(this.radioButton_SelectedUnbenchmarked);
            this.groupBox1.Location = new System.Drawing.Point(12, 219);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 146);
            this.groupBox1.TabIndex = 105;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Benchmark Algoruthms Settings:";
            // 
            // radioButton_All
            // 
            this.radioButton_All.AutoSize = true;
            this.radioButton_All.Location = new System.Drawing.Point(6, 89);
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
            this.radioButton_ReOnlySelected.Location = new System.Drawing.Point(7, 66);
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
            // FormBenchmark_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 494);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LabelProgressPercentage);
            this.Controls.Add(this.StartStopBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.BenchmarkProgressBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.devicesListViewEnableControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBenchmark_New";
            this.Text = "Benchmark";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.DevicesListViewEnableControl devicesListViewEnableControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelProgressPercentage;
        private System.Windows.Forms.Button StartStopBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.ProgressBar BenchmarkProgressBar;
        private System.Windows.Forms.RadioButton radioButton_SelectedUnbenchmarked;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_All;
        private System.Windows.Forms.RadioButton radioButton_ReOnlySelected;
        private System.Windows.Forms.RadioButton radioButton_Unbenchmarked;


    }
}