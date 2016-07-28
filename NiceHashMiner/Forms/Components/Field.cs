using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms.Components {
    public partial class Field : UserControl {
        public string LabelText {
            get {
                return labelFieldIndicator.Text;
            }
            set {
                if(value != null) {
                    labelFieldIndicator.Text = value;
                }
            }
        }

        public string EntryText {
            get {
                return textBox.Text;
            }
            set {
                if (value != null) {
                    textBox.Text = value;
                }
            }
        }

        public Field() {
            InitializeComponent();
        }
    }
}
