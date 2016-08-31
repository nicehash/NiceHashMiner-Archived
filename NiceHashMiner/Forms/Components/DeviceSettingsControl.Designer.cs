namespace NiceHashMiner.Forms.Components {
    partial class DeviceSettingsControl {
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
            this.labelSelectedDeviceName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSelectedDeviceGroup = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nvidiaSpecificSettings1 = new NiceHashMiner.Forms.Components.NvidiaSpecificSettings();
            this.amdSpecificSettings1 = new NiceHashMiner.Forms.Components.AmdSpecificSettings();
            this.cpuSpecificSettings1 = new NiceHashMiner.Forms.Components.CpuSpecificSettings();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSelectedDeviceName
            // 
            this.labelSelectedDeviceName.AutoSize = true;
            this.labelSelectedDeviceName.Location = new System.Drawing.Point(21, 20);
            this.labelSelectedDeviceName.Name = "labelSelectedDeviceName";
            this.labelSelectedDeviceName.Size = new System.Drawing.Size(151, 13);
            this.labelSelectedDeviceName.TabIndex = 1;
            this.labelSelectedDeviceName.Text = "Name: Selected Device Name";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.labelSelectedDeviceGroup);
            this.groupBox1.Controls.Add(this.labelSelectedDeviceName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 67);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Device Info";
            // 
            // labelSelectedDeviceGroup
            // 
            this.labelSelectedDeviceGroup.AutoSize = true;
            this.labelSelectedDeviceGroup.Location = new System.Drawing.Point(21, 38);
            this.labelSelectedDeviceGroup.Name = "labelSelectedDeviceGroup";
            this.labelSelectedDeviceGroup.Size = new System.Drawing.Size(153, 13);
            this.labelSelectedDeviceGroup.TabIndex = 2;
            this.labelSelectedDeviceGroup.Text = "Group: Selected Device Group";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nvidiaSpecificSettings1);
            this.groupBox3.Controls.Add(this.amdSpecificSettings1);
            this.groupBox3.Controls.Add(this.cpuSpecificSettings1);
            this.groupBox3.Location = new System.Drawing.Point(3, 76);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(392, 115);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Specific Device Group Settings";
            // 
            // nvidiaSpecificSettings1
            // 
            this.nvidiaSpecificSettings1.Location = new System.Drawing.Point(12, 19);
            this.nvidiaSpecificSettings1.Name = "nvidiaSpecificSettings1";
            this.nvidiaSpecificSettings1.Size = new System.Drawing.Size(377, 61);
            this.nvidiaSpecificSettings1.TabIndex = 6;
            // 
            // amdSpecificSettings1
            // 
            this.amdSpecificSettings1.Location = new System.Drawing.Point(9, 19);
            this.amdSpecificSettings1.Name = "amdSpecificSettings1";
            this.amdSpecificSettings1.Size = new System.Drawing.Size(380, 67);
            this.amdSpecificSettings1.TabIndex = 5;
            // 
            // cpuSpecificSettings1
            // 
            this.cpuSpecificSettings1.Location = new System.Drawing.Point(9, 19);
            this.cpuSpecificSettings1.Name = "cpuSpecificSettings1";
            this.cpuSpecificSettings1.Size = new System.Drawing.Size(377, 71);
            this.cpuSpecificSettings1.TabIndex = 0;
            // 
            // DeviceSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "DeviceSettingsControl";
            this.Size = new System.Drawing.Size(403, 207);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSelectedDeviceName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSelectedDeviceGroup;
        private System.Windows.Forms.GroupBox groupBox3;
        private CpuSpecificSettings cpuSpecificSettings1;
        private AmdSpecificSettings amdSpecificSettings1;
        private NvidiaSpecificSettings nvidiaSpecificSettings1;

    }
}
