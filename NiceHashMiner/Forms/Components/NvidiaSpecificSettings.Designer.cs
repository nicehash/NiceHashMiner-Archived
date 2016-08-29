namespace NiceHashMiner.Forms.Components {
    partial class NvidiaSpecificSettings {
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
            this.comboBox_DagLoadMode = new System.Windows.Forms.ComboBox();
            this.label_DagGeneration = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_DagLoadMode
            // 
            this.comboBox_DagLoadMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_DagLoadMode.FormattingEnabled = true;
            this.comboBox_DagLoadMode.Items.AddRange(new object[] {
            "Automatic",
            "SSE2",
            "AVX",
            "AVX2"});
            this.comboBox_DagLoadMode.Location = new System.Drawing.Point(130, 12);
            this.comboBox_DagLoadMode.Name = "comboBox_DagLoadMode";
            this.comboBox_DagLoadMode.Size = new System.Drawing.Size(121, 21);
            this.comboBox_DagLoadMode.TabIndex = 106;
            this.comboBox_DagLoadMode.Leave += new System.EventHandler(this.comboBox_DagLoadMode_Leave);
            // 
            // label_DagGeneration
            // 
            this.label_DagGeneration.AutoSize = true;
            this.label_DagGeneration.Location = new System.Drawing.Point(13, 15);
            this.label_DagGeneration.Name = "label_DagGeneration";
            this.label_DagGeneration.Size = new System.Drawing.Size(87, 13);
            this.label_DagGeneration.TabIndex = 107;
            this.label_DagGeneration.Text = "Dag Load Mode:";
            this.label_DagGeneration.Leave += new System.EventHandler(this.comboBox_DagLoadMode_Leave);
            // 
            // NvidiaSpecificSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_DagLoadMode);
            this.Controls.Add(this.label_DagGeneration);
            this.Name = "NvidiaSpecificSettings";
            this.Size = new System.Drawing.Size(377, 71);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_DagLoadMode;
        private System.Windows.Forms.Label label_DagGeneration;
    }
}
