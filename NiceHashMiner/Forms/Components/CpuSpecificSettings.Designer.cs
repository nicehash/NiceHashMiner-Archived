namespace NiceHashMiner.Forms.Components {
    partial class CpuSpecificSettings {
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
            this.textBox_CPU0_LessThreads = new System.Windows.Forms.TextBox();
            this.label_CPU0_LessThreads = new System.Windows.Forms.Label();
            this.comboBox_CPU0_ForceCPUExtension = new System.Windows.Forms.ComboBox();
            this.label_CPU0_ForceCPUExtension = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_CPU0_LessThreads
            // 
            this.textBox_CPU0_LessThreads.Location = new System.Drawing.Point(324, 9);
            this.textBox_CPU0_LessThreads.Name = "textBox_CPU0_LessThreads";
            this.textBox_CPU0_LessThreads.Size = new System.Drawing.Size(100, 20);
            this.textBox_CPU0_LessThreads.TabIndex = 101;
            // 
            // label_CPU0_LessThreads
            // 
            this.label_CPU0_LessThreads.AutoSize = true;
            this.label_CPU0_LessThreads.Location = new System.Drawing.Point(250, 12);
            this.label_CPU0_LessThreads.Name = "label_CPU0_LessThreads";
            this.label_CPU0_LessThreads.Size = new System.Drawing.Size(71, 13);
            this.label_CPU0_LessThreads.TabIndex = 102;
            this.label_CPU0_LessThreads.Text = "LessThreads:";
            // 
            // comboBox_CPU0_ForceCPUExtension
            // 
            this.comboBox_CPU0_ForceCPUExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CPU0_ForceCPUExtension.FormattingEnabled = true;
            this.comboBox_CPU0_ForceCPUExtension.Items.AddRange(new object[] {
            "Automatic",
            "SSE2",
            "AVX",
            "AVX2"});
            this.comboBox_CPU0_ForceCPUExtension.Location = new System.Drawing.Point(120, 9);
            this.comboBox_CPU0_ForceCPUExtension.Name = "comboBox_CPU0_ForceCPUExtension";
            this.comboBox_CPU0_ForceCPUExtension.Size = new System.Drawing.Size(121, 21);
            this.comboBox_CPU0_ForceCPUExtension.TabIndex = 100;
            // 
            // label_CPU0_ForceCPUExtension
            // 
            this.label_CPU0_ForceCPUExtension.AutoSize = true;
            this.label_CPU0_ForceCPUExtension.Location = new System.Drawing.Point(3, 12);
            this.label_CPU0_ForceCPUExtension.Name = "label_CPU0_ForceCPUExtension";
            this.label_CPU0_ForceCPUExtension.Size = new System.Drawing.Size(105, 13);
            this.label_CPU0_ForceCPUExtension.TabIndex = 103;
            this.label_CPU0_ForceCPUExtension.Text = "ForceCPUExtension:";
            // 
            // CpuSpecificSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_CPU0_LessThreads);
            this.Controls.Add(this.label_CPU0_LessThreads);
            this.Controls.Add(this.comboBox_CPU0_ForceCPUExtension);
            this.Controls.Add(this.label_CPU0_ForceCPUExtension);
            this.Name = "CpuSpecificSettings";
            this.Size = new System.Drawing.Size(435, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_CPU0_LessThreads;
        private System.Windows.Forms.Label label_CPU0_LessThreads;
        private System.Windows.Forms.ComboBox comboBox_CPU0_ForceCPUExtension;
        private System.Windows.Forms.Label label_CPU0_ForceCPUExtension;
    }
}
