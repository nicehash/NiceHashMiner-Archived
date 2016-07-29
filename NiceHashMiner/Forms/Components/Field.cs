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

        public void SetInputModeDoubleOnly() {
            textBox.KeyPress += new KeyPressEventHandler(textBoxDoubleOnly_KeyPress);
        }

        public void SetInputModeIntOnly() {
            textBox.KeyPress += new KeyPressEventHandler(textBoxIntOnly_KeyPress);
        }

        public void SetOnTextChanged(EventHandler textChanged) {
            textBox.TextChanged += textChanged;
        }

        public Field() {
            InitializeComponent();
        }

        private void textBoxDoubleOnly_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && (e.KeyChar != '.')) {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) {
                e.Handled = true;
            }
        }

        private void textBoxIntOnly_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }
    }
}
