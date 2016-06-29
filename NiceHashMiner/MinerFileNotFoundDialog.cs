using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public partial class MinerFileNotFoundDialog : Form
    {
        public bool DisableDetection;

        public MinerFileNotFoundDialog(string MinerDeviceName, string Path)
        {
            InitializeComponent();

            DisableDetection = false;
            this.Text = International.GetText("MinerFileNotFoundDialog_title");
            this.linkLabelError.Text = String.Format(International.GetText("MinerFileNotFoundDialog_linkLabelError"), MinerDeviceName, Path, International.GetText("MinerFileNotFoundDialog_link"));
            this.linkLabelError.LinkArea = new LinkArea(this.linkLabelError.Text.IndexOf(International.GetText("MinerFileNotFoundDialog_link")), International.GetText("MinerFileNotFoundDialog_link").Length);
            this.chkBoxDisableDetection.Text = International.GetText("MinerFileNotFoundDialog_chkBoxDisableDetection");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (chkBoxDisableDetection.Checked)
                DisableDetection = true;

            this.Close();
        }

        private void linkLabelError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nicehash/NiceHashMiner#troubleshooting");
        }
    }
}
