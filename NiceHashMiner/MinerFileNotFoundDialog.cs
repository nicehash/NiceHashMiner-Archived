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
            this.linkLabelError.Text = MinerDeviceName + ": File " + Path + " is not found!\n\n" +
                                   "Please make sure that the file is accessible and that your anti-virus is not blocking the application.\n" + 
                                   "Please refer the section \"My anti-virus is blocking the application\" at the Troubleshooting section (Link).\n\n" +
                                   "A re-download of NiceHash Miner might be needed.";
            this.linkLabelError.LinkArea = new LinkArea(this.linkLabelError.Text.IndexOf("Link"), 4);
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
