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

        public void InitLocale(ToolTip toolTip1, string infoLabel, string infoMsg) {
            labelFieldIndicator.Text = infoLabel;
            toolTip1.SetToolTip(labelFieldIndicator, infoMsg);
            toolTip1.SetToolTip(textBox, infoMsg);
            toolTip1.SetToolTip(pictureBox1, infoMsg);
        }

        public void SetInputModeDoubleOnly() {
            textBox.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxDoubleOnly_KeyPress);
        }

        public void SetInputModeIntOnly() {
            textBox.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
        }

        public void SetOnTextChanged(EventHandler textChanged) {
            textBox.TextChanged += textChanged;
        }

        public void SetOnTextLeave(EventHandler textLeave) {
            textBox.Leave += textLeave;
        }

        public Field() {
            InitializeComponent();
        }

    }
}
