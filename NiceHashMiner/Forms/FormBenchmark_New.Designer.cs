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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.benchmarkAlgorithmSettup1 = new NiceHashMiner.Forms.Components.BenchmarkAlgorithmSettup();
            this.algorithmsListView1 = new NiceHashMiner.Forms.Components.AlgorithmsListView();
            this.benchmarkOptions1 = new NiceHashMiner.Forms.Components.BenchmarkOptions();
            this.devicesListView1 = new NiceHashMiner.Forms.Components.DevicesListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(938, 632);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.benchmarkAlgorithmSettup1);
            this.tabPage1.Controls.Add(this.algorithmsListView1);
            this.tabPage1.Controls.Add(this.benchmarkOptions1);
            this.tabPage1.Controls.Add(this.devicesListView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(930, 606);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Device Benchmark";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(777, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // benchmarkAlgorithmSettup1
            // 
            this.benchmarkAlgorithmSettup1.Location = new System.Drawing.Point(489, 150);
            this.benchmarkAlgorithmSettup1.Name = "benchmarkAlgorithmSettup1";
            this.benchmarkAlgorithmSettup1.Size = new System.Drawing.Size(411, 389);
            this.benchmarkAlgorithmSettup1.TabIndex = 7;
            // 
            // algorithmsListView1
            // 
            this.algorithmsListView1.ComunicationInterface = null;
            this.algorithmsListView1.Location = new System.Drawing.Point(25, 150);
            this.algorithmsListView1.Name = "algorithmsListView1";
            this.algorithmsListView1.Size = new System.Drawing.Size(458, 380);
            this.algorithmsListView1.TabIndex = 6;
            // 
            // benchmarkOptions1
            // 
            this.benchmarkOptions1.Location = new System.Drawing.Point(460, 26);
            this.benchmarkOptions1.Name = "benchmarkOptions1";
            this.benchmarkOptions1.Size = new System.Drawing.Size(286, 108);
            this.benchmarkOptions1.TabIndex = 5;
            // 
            // devicesListView1
            // 
            this.devicesListView1.Location = new System.Drawing.Point(25, 26);
            this.devicesListView1.Name = "devicesListView1";
            this.devicesListView1.Size = new System.Drawing.Size(429, 108);
            this.devicesListView1.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(930, 606);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Group Benchmark";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FormBenchmark_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 632);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormBenchmark_New";
            this.Text = "FormBenchmark_New";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Components.BenchmarkAlgorithmSettup benchmarkAlgorithmSettup1;
        private Components.AlgorithmsListView algorithmsListView1;
        private Components.BenchmarkOptions benchmarkOptions1;
        private Components.DevicesListView devicesListView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;

    }
}