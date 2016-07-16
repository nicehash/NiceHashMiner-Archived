namespace NiceHashMiner
{
    partial class DriverVersionConfirmationDialog
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
            this.labelWarning = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.chkBoxDontShowAgain = new System.Windows.Forms.CheckBox();
            this.linkToDriverDownloadPage = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Location = new System.Drawing.Point(7, 10);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(488, 30);
            this.labelWarning.TabIndex = 99;
            this.labelWarning.Text = "You\'re not using optimal AMD Driver version. Most stable driver for mining is the" +
    " 15.7.1 version.\r\nWe strongly suggest you to use this driver version.";
            this.labelWarning.UseCompatibleTextRendering = true;
            this.labelWarning.UseMnemonic = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(410, 67);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseCompatibleTextRendering = true;
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // chkBoxDontShowAgain
            // 
            this.chkBoxDontShowAgain.AutoSize = true;
            this.chkBoxDontShowAgain.Location = new System.Drawing.Point(7, 70);
            this.chkBoxDontShowAgain.Name = "chkBoxDontShowAgain";
            this.chkBoxDontShowAgain.Size = new System.Drawing.Size(180, 18);
            this.chkBoxDontShowAgain.TabIndex = 2;
            this.chkBoxDontShowAgain.Text = "&Do not show this warning again";
            this.chkBoxDontShowAgain.UseCompatibleTextRendering = true;
            this.chkBoxDontShowAgain.UseVisualStyleBackColor = true;
            // 
            // linkToDriverDownloadPage
            // 
            this.linkToDriverDownloadPage.AutoSize = true;
            this.linkToDriverDownloadPage.Location = new System.Drawing.Point(5, 44);
            this.linkToDriverDownloadPage.Name = "linkToDriverDownloadPage";
            this.linkToDriverDownloadPage.Size = new System.Drawing.Size(149, 13);
            this.linkToDriverDownloadPage.TabIndex = 1;
            this.linkToDriverDownloadPage.TabStop = true;
            this.linkToDriverDownloadPage.Text = "&Link to Driver Download Page";
            this.linkToDriverDownloadPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkToDriverDownloadPage_LinkClicked);
            // 
            // DriverVersionConfirmationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(497, 97);
            this.Controls.Add(this.linkToDriverDownloadPage);
            this.Controls.Add(this.chkBoxDontShowAgain);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelWarning);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.InfoText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DriverVersionConfirmationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Update AMD Driver Recommended";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox chkBoxDontShowAgain;
        private System.Windows.Forms.LinkLabel linkToDriverDownloadPage;
    }
}