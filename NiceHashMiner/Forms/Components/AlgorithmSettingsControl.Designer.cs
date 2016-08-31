namespace NiceHashMiner.Forms.Components {
    partial class AlgorithmSettingsControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.groupBoxSelectedAlgorithmSettings = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonBenchmark = new System.Windows.Forms.Button();
            this.labelSelectedAlgorithm = new System.Windows.Forms.Label();
            this.groupBoxExtraLaunchParameters = new System.Windows.Forms.GroupBox();
            this.richTextBoxExtraLaunchParameters = new System.Windows.Forms.RichTextBox();
            this.fieldIntensity = new NiceHashMiner.Forms.Components.Field();
            this.fieldBoxBenchmarkSpeed = new NiceHashMiner.Forms.Components.Field();
            this.groupBoxSelectedAlgorithmSettings.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBoxExtraLaunchParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSelectedAlgorithmSettings
            // 
            this.groupBoxSelectedAlgorithmSettings.Controls.Add(this.flowLayoutPanel1);
            this.groupBoxSelectedAlgorithmSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSelectedAlgorithmSettings.Name = "groupBoxSelectedAlgorithmSettings";
            this.groupBoxSelectedAlgorithmSettings.Size = new System.Drawing.Size(399, 250);
            this.groupBoxSelectedAlgorithmSettings.TabIndex = 11;
            this.groupBoxSelectedAlgorithmSettings.TabStop = false;
            this.groupBoxSelectedAlgorithmSettings.Text = "Selected Algorithm Settings:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.fieldIntensity);
            this.flowLayoutPanel1.Controls.Add(this.fieldBoxBenchmarkSpeed);
            this.flowLayoutPanel1.Controls.Add(this.groupBoxExtraLaunchParameters);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(393, 231);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.buttonBenchmark);
            this.flowLayoutPanel2.Controls.Add(this.labelSelectedAlgorithm);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(384, 31);
            this.flowLayoutPanel2.TabIndex = 17;
            // 
            // buttonBenchmark
            // 
            this.buttonBenchmark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBenchmark.Location = new System.Drawing.Point(15, 3);
            this.buttonBenchmark.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.buttonBenchmark.Name = "buttonBenchmark";
            this.buttonBenchmark.Size = new System.Drawing.Size(75, 23);
            this.buttonBenchmark.TabIndex = 16;
            this.buttonBenchmark.Text = "Benchmark";
            this.buttonBenchmark.UseVisualStyleBackColor = true;
            this.buttonBenchmark.Click += new System.EventHandler(this.buttonBenchmark_Click);
            // 
            // labelSelectedAlgorithm
            // 
            this.labelSelectedAlgorithm.AutoSize = true;
            this.labelSelectedAlgorithm.Location = new System.Drawing.Point(111, 9);
            this.labelSelectedAlgorithm.Margin = new System.Windows.Forms.Padding(18, 9, 3, 3);
            this.labelSelectedAlgorithm.Name = "labelSelectedAlgorithm";
            this.labelSelectedAlgorithm.Size = new System.Drawing.Size(132, 13);
            this.labelSelectedAlgorithm.TabIndex = 15;
            this.labelSelectedAlgorithm.Text = "Selected Algorithm: NONE";
            // 
            // groupBoxExtraLaunchParameters
            // 
            this.groupBoxExtraLaunchParameters.Controls.Add(this.richTextBoxExtraLaunchParameters);
            this.groupBoxExtraLaunchParameters.Location = new System.Drawing.Point(3, 114);
            this.groupBoxExtraLaunchParameters.Name = "groupBoxExtraLaunchParameters";
            this.groupBoxExtraLaunchParameters.Size = new System.Drawing.Size(387, 95);
            this.groupBoxExtraLaunchParameters.TabIndex = 14;
            this.groupBoxExtraLaunchParameters.TabStop = false;
            this.groupBoxExtraLaunchParameters.Text = "Extra Launch Parameters:";
            // 
            // richTextBoxExtraLaunchParameters
            // 
            this.richTextBoxExtraLaunchParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxExtraLaunchParameters.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxExtraLaunchParameters.Name = "richTextBoxExtraLaunchParameters";
            this.richTextBoxExtraLaunchParameters.Size = new System.Drawing.Size(381, 76);
            this.richTextBoxExtraLaunchParameters.TabIndex = 0;
            this.richTextBoxExtraLaunchParameters.Text = "";
            // 
            // fieldIntensity
            // 
            this.fieldIntensity.AutoSize = true;
            this.fieldIntensity.BackColor = System.Drawing.Color.Transparent;
            this.fieldIntensity.EntryText = "";
            this.fieldIntensity.LabelText = "Intensity:";
            this.fieldIntensity.Location = new System.Drawing.Point(3, 40);
            this.fieldIntensity.Name = "fieldIntensity";
            this.fieldIntensity.Size = new System.Drawing.Size(389, 31);
            this.fieldIntensity.TabIndex = 2;
            // 
            // fieldBoxBenchmarkSpeed
            // 
            this.fieldBoxBenchmarkSpeed.AutoSize = true;
            this.fieldBoxBenchmarkSpeed.BackColor = System.Drawing.Color.Transparent;
            this.fieldBoxBenchmarkSpeed.EntryText = "";
            this.fieldBoxBenchmarkSpeed.LabelText = "Benchmark Speed (H/s):";
            this.fieldBoxBenchmarkSpeed.Location = new System.Drawing.Point(3, 77);
            this.fieldBoxBenchmarkSpeed.Name = "fieldBoxBenchmarkSpeed";
            this.fieldBoxBenchmarkSpeed.Size = new System.Drawing.Size(389, 31);
            this.fieldBoxBenchmarkSpeed.TabIndex = 1;
            // 
            // AlgorithmSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSelectedAlgorithmSettings);
            this.Name = "AlgorithmSettingsControl";
            this.Size = new System.Drawing.Size(406, 262);
            this.groupBoxSelectedAlgorithmSettings.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.groupBoxExtraLaunchParameters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSelectedAlgorithmSettings;
        private System.Windows.Forms.GroupBox groupBoxExtraLaunchParameters;
        private System.Windows.Forms.RichTextBox richTextBoxExtraLaunchParameters;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Field fieldIntensity;
        private Field fieldBoxBenchmarkSpeed;
        private System.Windows.Forms.Button buttonBenchmark;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label labelSelectedAlgorithm;
    }
}
