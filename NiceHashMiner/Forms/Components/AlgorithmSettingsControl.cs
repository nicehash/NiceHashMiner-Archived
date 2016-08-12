using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms.Components {
    public partial class AlgorithmSettingsControl : UserControl, AlgorithmsListView.IAlgorithmsListView {

        Algorithm _currentlySelectedAlgorithm = null;
        ListViewItem _currentlySelectedLvi = null;

        public AlgorithmSettingsControl() {
            InitializeComponent();
            fieldBoxBenchmarkSpeed.SetInputModeDoubleOnly();

            fieldBoxPassword.SetOnTextChanged(textChangedPassword);
            fieldBoxBenchmarkSpeed.SetOnTextChanged(textChangedBenchmarkSpeed);
            richTextBoxExtraLaunchParameters.TextChanged += textChangedExtraLaunchParameters;

        }

        public void Deselect() {
            labelSelectedAlgorithm.Text = "Selected Algorithm: NONE";
            Enabled = false;
            fieldBoxBenchmarkSpeed.EntryText = "";
            fieldBoxPassword.EntryText = "";
            richTextBoxExtraLaunchParameters.Text = "";
        }

        private string ParseStringDefault(string value) {
            return value == null ? "" : value;
        }

        private string ParseDoubleDefault(double value) {
            return value <= 0 ? "" : value.ToString();
        }

        public void SetCurrentlySelected(ListViewItem lvi) {
            // should not happen ever
            if (lvi == null) return;

            var algorithm = lvi.Tag as Algorithm;
            if (algorithm != null) {
                _currentlySelectedAlgorithm = algorithm;
                _currentlySelectedLvi = lvi;
                this.Enabled = lvi.Checked;

                labelSelectedAlgorithm.Text = "Selected Algorithm: " + algorithm.NiceHashName;

                fieldBoxPassword.EntryText = ParseStringDefault(algorithm.UsePassword);
                fieldBoxBenchmarkSpeed.EntryText = ParseDoubleDefault(algorithm.BenchmarkSpeed);
                richTextBoxExtraLaunchParameters.Text = ParseStringDefault(algorithm.ExtraLaunchParameters);
                this.Update();
            } else {
                // TODO this should not be null
            }
        }

        public void HandleCheck(ListViewItem lvi) {
            if (Object.ReferenceEquals(_currentlySelectedLvi, lvi)) {
                this.Enabled = lvi.Checked;
            }
        }

        #region Callbacks Events
        private void textChangedPassword(object sender, EventArgs e) {
            if (_currentlySelectedAlgorithm == null) return;
            _currentlySelectedAlgorithm.UsePassword = fieldBoxPassword.EntryText;
        }
        private void textChangedBenchmarkSpeed(object sender, EventArgs e) {
            if (_currentlySelectedAlgorithm == null) return;
            double value;
            if (Double.TryParse(fieldBoxBenchmarkSpeed.EntryText, out value)) {
                _currentlySelectedAlgorithm.BenchmarkSpeed = value;
                // update lvi speed
                if (_currentlySelectedLvi != null) {
                    _currentlySelectedLvi.SubItems[3].Text = Helpers.FormatSpeedOutput(value);
                }
            }
        }
        private void textChangedExtraLaunchParameters(object sender, EventArgs e) {
            if (_currentlySelectedAlgorithm == null) return;
            _currentlySelectedAlgorithm.ExtraLaunchParameters = richTextBoxExtraLaunchParameters.Text;
        }
        #endregion

    }
}
