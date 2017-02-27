using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner {
    static class TextBoxKeyPressEvents {
        public static void textBoxIntsOnly_KeyPress(object sender, KeyPressEventArgs e) {
            // allow only one zero
            var textBox = sender as TextBox;
            if (textBox.SelectionLength != textBox.Text.Length && IsHandleZero(e, textBox.Text)) {
                e.Handled = true;
                return;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        public static void textBoxDoubleOnly_KeyPress(object sender, KeyPressEventArgs e) {
            // allow only one zero
            var textBox = sender as TextBox;
            string checkText = textBox.Text;
            if (e.KeyChar != '.' && textBox.SelectionLength != textBox.Text.Length && IsHandleZero(e, checkText) && !checkText.Contains(".")) {
                e.Handled = true;
                return;
            }
            if (doubleInvalid(e.KeyChar)) {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) {
                e.Handled = true;
            }
        }

        private static bool doubleInvalid(char c) { return !char.IsControl(c) && !char.IsDigit(c) && (c != '.'); }

        private static bool IsHandleZero(KeyPressEventArgs e, string checkText) {
            if (!char.IsControl(e.KeyChar) && checkText.Length > 0 && checkText[0] == '0') {
                return true;
            }
            return false;
        }

    }
}
