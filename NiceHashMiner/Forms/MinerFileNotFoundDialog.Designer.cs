namespace NiceHashMiner
{
    partial class MinerFileNotFoundDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinerFileNotFoundDialog));
            this.linkLabelError = new System.Windows.Forms.LinkLabel();
            this.chkBoxDisableDetection = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkLabelError
            // 
            this.linkLabelError.AutoSize = true;
            this.linkLabelError.LinkArea = new System.Windows.Forms.LinkArea(253, 4);
            this.linkLabelError.Location = new System.Drawing.Point(12, 9);
            this.linkLabelError.Name = "linkLabelError";
            this.linkLabelError.Size = new System.Drawing.Size(525, 80);
            this.linkLabelError.TabIndex = 1;
            this.linkLabelError.TabStop = true;
            this.linkLabelError.Text = resources.GetString("linkLabelError.Text");
            this.linkLabelError.UseCompatibleTextRendering = true;
            this.linkLabelError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelError_LinkClicked);
            // 
            // chkBoxDisableDetection
            // 
            this.chkBoxDisableDetection.AutoSize = true;
            this.chkBoxDisableDetection.Location = new System.Drawing.Point(12, 102);
            this.chkBoxDisableDetection.Name = "chkBoxDisableDetection";
            this.chkBoxDisableDetection.Size = new System.Drawing.Size(179, 18);
            this.chkBoxDisableDetection.TabIndex = 2;
            this.chkBoxDisableDetection.Text = "&Disable detection of this device";
            this.chkBoxDisableDetection.UseCompatibleTextRendering = true;
            this.chkBoxDisableDetection.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(462, 102);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseCompatibleTextRendering = true;
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // MinerFileNotFoundDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 131);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.chkBoxDisableDetection);
            this.Controls.Add(this.linkLabelError);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MinerFileNotFoundDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File not found!!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelError;
        private System.Windows.Forms.CheckBox chkBoxDisableDetection;
        private System.Windows.Forms.Button buttonOK;

    }
}