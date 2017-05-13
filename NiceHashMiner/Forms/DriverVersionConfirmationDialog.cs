using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;

namespace NiceHashMiner
{
    public partial class DriverVersionConfirmationDialog : Form
    {
        public DriverVersionConfirmationDialog()
        {
            InitializeComponent();

            this.Text = International.GetText("DriverVersionConfirmationDialog_title");
            labelWarning.Text = International.GetText("DriverVersionConfirmationDialog_labelWarning");
            linkToDriverDownloadPage.Text = International.GetText("DriverVersionConfirmationDialog_linkToDriverDownloadPage");
            chkBoxDontShowAgain.Text = International.GetText("DriverVersionConfirmationDialog_chkBoxDontShowAgain");
            buttonOK.Text = International.GetText("Global_OK");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (chkBoxDontShowAgain.Checked)
            {
                Helpers.ConsolePrint("NICEHASH", "Setting ShowDriverVersionWarning to false");
                ConfigManager.GeneralConfig.ShowDriverVersionWarning = false;
            }

            this.Close();
        }

        private void linkToDriverDownloadPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://support.amd.com/en-us/download/desktop/legacy?product=legacy3&os=Windows+7+-+64");
        }
    }
}
