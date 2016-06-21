using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            label_LoadingText.Text = International.GetText("form3_label_LoadingText");
        }
    }
}
