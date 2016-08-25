using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Forms.Components {
    public partial class AlgorithmSettingsControl : UserControl, AlgorithmsListView.IAlgorithmsListView {

        ComputeDevice _computeDevice = null;
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

        public void SetCurrentlySelected(ListViewItem lvi, ComputeDevice computeDevice) {
            // should not happen ever
            if (lvi == null) return;

            _computeDevice = computeDevice;
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
            _currentlySelectedAlgorithm.UsePassword = fieldBoxPassword.EntryText.Trim();
        }
        private void textChangedBenchmarkSpeed(object sender, EventArgs e) {
            if (_currentlySelectedAlgorithm == null) return;
            double value;
            if (Double.TryParse(fieldBoxBenchmarkSpeed.EntryText, out value)) {
                _currentlySelectedAlgorithm.BenchmarkSpeed = value;
                // update lvi speed
                if (_currentlySelectedLvi != null) {
                    _currentlySelectedLvi.SubItems[2].Text = Helpers.FormatSpeedOutput(value);
                }
            }
        }
        private void textChangedExtraLaunchParameters(object sender, EventArgs e) {
            if (_currentlySelectedAlgorithm == null) return;
            _currentlySelectedAlgorithm.ExtraLaunchParameters = richTextBoxExtraLaunchParameters.Text.Trim();
        }
        #endregion

        private void buttonBenchmark_Click(object sender, EventArgs e) {
            var device = new List<ComputeDevice>();
            device.Add(_computeDevice);
            var BenchmarkForm = new FormBenchmark_New(
                        BenchmarkPerformanceType.Standard,
                        false, device, _currentlySelectedAlgorithm.NiceHashID);
            BenchmarkForm.ShowDialog();
            fieldBoxBenchmarkSpeed.EntryText = _currentlySelectedAlgorithm.BenchmarkSpeed.ToString();
            // update lvi speed
            if (_currentlySelectedLvi != null) {
                _currentlySelectedLvi.SubItems[2].Text = Helpers.FormatSpeedOutput(_currentlySelectedAlgorithm.BenchmarkSpeed);
            }
        }

    }
}
