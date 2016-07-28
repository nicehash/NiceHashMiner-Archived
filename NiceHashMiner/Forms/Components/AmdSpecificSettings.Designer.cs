namespace NiceHashMiner.Forms.Components {
    partial class AmdSpecificSettings {
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
            this.checkBox_AMD_DisableAMDTempControl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_AMD_DisableAMDTempControl
            // 
            this.checkBox_AMD_DisableAMDTempControl.AutoSize = true;
            this.checkBox_AMD_DisableAMDTempControl.Location = new System.Drawing.Point(3, 12);
            this.checkBox_AMD_DisableAMDTempControl.Name = "checkBox_AMD_DisableAMDTempControl";
            this.checkBox_AMD_DisableAMDTempControl.Size = new System.Drawing.Size(145, 17);
            this.checkBox_AMD_DisableAMDTempControl.TabIndex = 5;
            this.checkBox_AMD_DisableAMDTempControl.Text = "DisableAMDTempControl";
            this.checkBox_AMD_DisableAMDTempControl.UseVisualStyleBackColor = true;
            // 
            // AmdSpecificSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_AMD_DisableAMDTempControl);
            this.Name = "AmdSpecificSettings";
            this.Size = new System.Drawing.Size(435, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_AMD_DisableAMDTempControl;

    }
}
