using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms
{
    public partial class Form_DeviceRename : Form
    {
        public string NewName;

        public Form_DeviceRename()
        {
            InitializeComponent();
        }

        private void tbNewName_TextChanged(object sender, EventArgs e)
        {
            string s = tbNewName.Text.Trim();

            btnOK.Enabled = !String.IsNullOrEmpty(s) && lblCurrentName.Text != s;
        }

        public void SetCurrentName(string s)
        {
            string first = s.Split(' ')[0];

            lblDeviceType.Text = first;

            first = first.Contains("#") ? s.Remove(0, first.Length) : s;

            lblCurrentName.Text = tbNewName.Text = first.Trim();
            tbNewName_TextChanged(null, null);
        }

        private void tbNewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnOK.Enabled)
                btnOK_Click(sender, e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            NewName = $"{lblDeviceType.Text} {tbNewName.Text.Trim()}";

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
