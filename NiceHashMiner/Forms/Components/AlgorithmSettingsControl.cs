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
        // winform crappy event workarond
        bool _selected = false;

        public AlgorithmSettingsControl() {
            InitializeComponent();
            fieldBoxBenchmarkSpeed.SetInputModeDoubleOnly();

            // TODO make sure intensity accepts valid ints based on Device and algo, miner...
            fieldIntensity.SetOnTextChanged(textChangedIntensity);
            fieldBoxBenchmarkSpeed.SetOnTextChanged(textChangedBenchmarkSpeed);
            richTextBoxExtraLaunchParameters.TextChanged += textChangedExtraLaunchParameters;

        }

        public void Deselect() {
            _selected = false;
            labelSelectedAlgorithm.Text = "Selected Algorithm: NONE";
            Enabled = false;
            fieldBoxBenchmarkSpeed.EntryText = "";
            fieldIntensity.EntryText = "";
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
                _selected = true;
                _currentlySelectedAlgorithm = algorithm;
                _currentlySelectedLvi = lvi;
                this.Enabled = lvi.Checked;

                labelSelectedAlgorithm.Text = "Selected Algorithm: " + algorithm.NiceHashName;

                fieldIntensity.EntryText = ParseStringDefault(algorithm.Intensity);
                // no intensity for cpu miners and ccminer_cryptonight
                fieldIntensity.Enabled = !(_computeDevice.DeviceGroupType == DeviceGroupType.CPU
                    || _currentlySelectedAlgorithm.NiceHashID == AlgorithmType.CryptoNight
                    || _currentlySelectedAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto);

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

        private bool CanEdit() {
            return _currentlySelectedAlgorithm != null && _selected;
        }

        #region Callbacks Events
        // TODO Intensity
        private void textChangedIntensity(object sender, EventArgs e) {
            if (!CanEdit()) return;
            _currentlySelectedAlgorithm.Intensity = fieldIntensity.EntryText.Trim();
        }
        private void textChangedBenchmarkSpeed(object sender, EventArgs e) {
            if (!CanEdit()) return;
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
            if (!CanEdit()) return;
            _currentlySelectedAlgorithm.ExtraLaunchParameters = richTextBoxExtraLaunchParameters.Text;
        }
        #endregion

        private void buttonBenchmark_Click(object sender, EventArgs e) {
            var device = new List<ComputeDevice>();
            device.Add(_computeDevice);
            var BenchmarkForm = new FormBenchmark(
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
