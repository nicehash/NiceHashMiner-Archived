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
            field_LessThreads.SetInputModeIntOnly();

            // TODO make sure intensity accepts valid ints based on Device and algo, miner...
            //fieldIntensity.SetOnTextChanged(textChangedIntensity);
            field_LessThreads.SetOnTextLeave(LessThreads_Leave);
            fieldBoxBenchmarkSpeed.SetOnTextChanged(textChangedBenchmarkSpeed);
            richTextBoxExtraLaunchParameters.TextChanged += textChangedExtraLaunchParameters;

        }

        public void Deselect() {
            _selected = false;
            groupBoxSelectedAlgorithmSettings.Text = "Selected Algorithm: NONE";
            Enabled = false;
            fieldBoxBenchmarkSpeed.EntryText = "";
            //fieldIntensity.EntryText = "";
            field_LessThreads.EntryText = "";
            richTextBoxExtraLaunchParameters.Text = "";
        }

        public void InitLocale(ToolTip toolTip1) {
            field_LessThreads.InitLocale(toolTip1,
                International.GetText("Form_Settings_General_CPU_LessThreads") + ":",
                International.GetText("Form_Settings_ToolTip_CPU_LessThreads"));
            fieldBoxBenchmarkSpeed.InitLocale(toolTip1,
                International.GetText("Form_Settings_Algo_BenchmarkSpeed") + ":",
                International.GetText("Form_Settings_ToolTip_AlgoBenchmarkSpeed"));
            toolTip1.SetToolTip(groupBoxExtraLaunchParameters, International.GetText("Form_Settings_ToolTip_AlgoExtraLaunchParameters"));
            toolTip1.SetToolTip(pictureBox1, International.GetText("Form_Settings_ToolTip_AlgoExtraLaunchParameters"));
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

                groupBoxSelectedAlgorithmSettings.Text = "Selected Algorithm: " + algorithm.NiceHashName;

                //fieldIntensity.EntryText = ParseStringDefault(algorithm.Intensity);
                //// no intensity for cpu miners and ccminer_cryptonight
                //fieldIntensity.Enabled = !(_computeDevice.DeviceGroupType == DeviceGroupType.CPU
                //    || _currentlySelectedAlgorithm.NiceHashID == AlgorithmType.CryptoNight
                //    || _currentlySelectedAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto);
                field_LessThreads.Enabled = _computeDevice.DeviceGroupType == DeviceGroupType.CPU;
                if (field_LessThreads.Enabled) {
                    field_LessThreads.EntryText = algorithm.LessThreads.ToString();
                }
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
            //_currentlySelectedAlgorithm.Intensity = fieldIntensity.EntryText.Trim();
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

        private void LessThreads_Leave(object sender, EventArgs e) {
            TextBox txtbox = (TextBox)sender;
            int val;
            if (Int32.TryParse(txtbox.Text, out val)) {
                if (Globals.ThreadsPerCPU - val < 1) {
                    MessageBox.Show(International.GetText("form1_msgbox_CPUMiningLessThreadMsg"),
                                    International.GetText("Warning_with_Exclamation"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else {
                    _currentlySelectedAlgorithm.LessThreads = val;
                }
                txtbox.Text = _currentlySelectedAlgorithm.LessThreads.ToString();
            } else {
                MessageBox.Show(International.GetText("Form_Settings_LessThreadWarningMsg"),
                                International.GetText("Form_Settings_LessThreadWarningTitle"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbox.Text = _currentlySelectedAlgorithm.LessThreads.ToString();
                txtbox.Focus();
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
