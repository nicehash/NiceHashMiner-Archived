namespace NiceHashMiner.Forms.Components {
    partial class BenchmarkOptions {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_PreciseBenchmark = new System.Windows.Forms.RadioButton();
            this.radioButton_StandardBenchmark = new System.Windows.Forms.RadioButton();
            this.radioButton_QuickBenchmark = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_PreciseBenchmark);
            this.groupBox1.Controls.Add(this.radioButton_StandardBenchmark);
            this.groupBox1.Controls.Add(this.radioButton_QuickBenchmark);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 103);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Benchmark Type";
            // 
            // radioButton_PreciseBenchmark
            // 
            this.radioButton_PreciseBenchmark.AutoSize = true;
            this.radioButton_PreciseBenchmark.Location = new System.Drawing.Point(6, 77);
            this.radioButton_PreciseBenchmark.Name = "radioButton_PreciseBenchmark";
            this.radioButton_PreciseBenchmark.Size = new System.Drawing.Size(139, 17);
            this.radioButton_PreciseBenchmark.TabIndex = 14;
            this.radioButton_PreciseBenchmark.Text = "&Precise (will take longer)";
            this.radioButton_PreciseBenchmark.UseVisualStyleBackColor = true;
            this.radioButton_PreciseBenchmark.CheckedChanged += new System.EventHandler(this.radioButton_PreciseBenchmark_CheckedChanged);
            // 
            // radioButton_StandardBenchmark
            // 
            this.radioButton_StandardBenchmark.AutoSize = true;
            this.radioButton_StandardBenchmark.Checked = true;
            this.radioButton_StandardBenchmark.Location = new System.Drawing.Point(6, 48);
            this.radioButton_StandardBenchmark.Name = "radioButton_StandardBenchmark";
            this.radioButton_StandardBenchmark.Size = new System.Drawing.Size(68, 17);
            this.radioButton_StandardBenchmark.TabIndex = 13;
            this.radioButton_StandardBenchmark.TabStop = true;
            this.radioButton_StandardBenchmark.Text = "&Standard";
            this.radioButton_StandardBenchmark.UseVisualStyleBackColor = true;
            this.radioButton_StandardBenchmark.CheckedChanged += new System.EventHandler(this.radioButton_StandardBenchmark_CheckedChanged);
            // 
            // radioButton_QuickBenchmark
            // 
            this.radioButton_QuickBenchmark.AutoSize = true;
            this.radioButton_QuickBenchmark.Location = new System.Drawing.Point(6, 19);
            this.radioButton_QuickBenchmark.Name = "radioButton_QuickBenchmark";
            this.radioButton_QuickBenchmark.Size = new System.Drawing.Size(148, 17);
            this.radioButton_QuickBenchmark.TabIndex = 12;
            this.radioButton_QuickBenchmark.Text = "&Quick (can be inaccurate)";
            this.radioButton_QuickBenchmark.UseVisualStyleBackColor = true;
            this.radioButton_QuickBenchmark.CheckedChanged += new System.EventHandler(this.radioButton_QuickBenchmark_CheckedChanged);
            // 
            // BenchmarkOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "BenchmarkOptions";
            this.Size = new System.Drawing.Size(208, 108);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_PreciseBenchmark;
        private System.Windows.Forms.RadioButton radioButton_StandardBenchmark;
        private System.Windows.Forms.RadioButton radioButton_QuickBenchmark;
    }
}
