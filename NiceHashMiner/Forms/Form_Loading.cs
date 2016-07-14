using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public partial class Form_Loading : Form
    {
        public Form_Loading()
        {
            InitializeComponent();

            label_LoadingText.Text = International.GetText("form3_label_LoadingText");
            label_LoadingText.Location = new Point((this.Size.Width - label_LoadingText.Size.Width) / 2, label_LoadingText.Location.Y);
        }
    }
}
