using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

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
            //fieldIntensity.SetInputModeDoubleOnly();
            field_LessThreads.SetInputModeIntOnly();

            // TODO make sure intensity accepts valid ints based on Device and algo, miner...
            //fieldIntensity.SetOnTextLeave(textLeaveIntensity);
            field_LessThreads.SetOnTextLeave(LessThreads_Leave);
            fieldBoxBenchmarkSpeed.SetOnTextChanged(textChangedBenchmarkSpeed);
            richTextBoxExtraLaunchParameters.TextChanged += textChangedExtraLaunchParameters;

        }

        public void Deselect() {
            _selected = false;
            groupBoxSelectedAlgorithmSettings.Text = String.Format(International.GetText("AlgorithmsListView_GroupBox"),
                International.GetText("AlgorithmsListView_GroupBox_NONE"));
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
            //fieldIntensity.InitLocale(toolTip1,
            //    International.GetText("AlgorithmSettingsControl_Label_Intensity") + ":",
            //    International.GetText("AlgorithmSettingsControl_ToolTip_Intensity"));
            fieldBoxBenchmarkSpeed.InitLocale(toolTip1,
                International.GetText("Form_Settings_Algo_BenchmarkSpeed") + ":",
                International.GetText("Form_Settings_ToolTip_AlgoBenchmarkSpeed"));
            groupBoxExtraLaunchParameters.Text = International.GetText("Form_Settings_General_ExtraLaunchParameters");
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

                groupBoxSelectedAlgorithmSettings.Text = String.Format(International.GetText("AlgorithmsListView_GroupBox"),
                algorithm.NiceHashName); ;

                
                //// no intensity for cpu miners and ccminer_cryptonight
                //fieldIntensity.Enabled = !(
                //    _computeDevice.DeviceGroupType == DeviceGroupType.CPU
                //    || _computeDevice.DeviceGroupType == DeviceGroupType.AMD_OpenCL
                //    || _currentlySelectedAlgorithm.NiceHashID == AlgorithmType.CryptoNight
                //    || _currentlySelectedAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto);
                //if (fieldIntensity.Enabled) {
                //    if (algorithm.Intensity == 0) {
                //        fieldIntensity.EntryText = "0";
                //    } else {
                //        fieldIntensity.EntryText = algorithm.Intensity.ToString("F8");
                //    }
                //} else {
                //    fieldIntensity.EntryText = "";
                //}

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
        //private void textLeaveIntensity(object sender, EventArgs e) {
        //    if (!CanEdit()) return;
        //    var deviceType = _computeDevice.DeviceGroupType;
        //    var minerPath = MinersManager.Instance.MinerPathChecker[deviceType].GetOptimizedMinerPath(_currentlySelectedAlgorithm.NiceHashID);
        //    if (MinersManager.Instance.CCMinersIntensitiesBoundries.ContainsKey(minerPath)) {
        //        double min = MinersManager.Instance.CCMinersIntensitiesBoundries[minerPath].Item1;
        //        double max = MinersManager.Instance.CCMinersIntensitiesBoundries[minerPath].Item2;
        //        // parse 
        //        double value;
        //        if (Double.TryParse(fieldIntensity.EntryText, out value)) {
        //            if (min <= value && value <= max) {
        //                _currentlySelectedAlgorithm.Intensity = value;
        //            } else if (value != 0) {
        //                _currentlySelectedAlgorithm.Intensity = 0;
        //                MessageBox.Show(
        //                    String.Format(International.GetText("AlgorithmSettingsControl_IntensityWaringn"),
        //                    min.ToString("F2"),
        //                    max.ToString("F2")),
        //                                International.GetText("Warning_with_Exclamation"),
        //                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //        } else {
        //            _currentlySelectedAlgorithm.Intensity = 0;
        //        }
        //        //_currentlySelectedAlgorithm.Intensity = fieldIntensity.EntryText.Trim();
        //    }
        //}
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
                    MessageBox.Show(International.GetText("Form_Main_msgbox_CPUMiningLessThreadMsg"),
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
            var ExtraLaunchParams = richTextBoxExtraLaunchParameters.Text.Replace("\r\n", " ");
            ExtraLaunchParams = ExtraLaunchParams.Replace("\n", " ");
            _currentlySelectedAlgorithm.ExtraLaunchParameters = ExtraLaunchParams;
            //// if dagger copy for all devices group CUDA or AMD
            //if (_currentlySelectedAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto) {
            //    foreach (var cDev in ComputeDevice.AllAvaliableDevices) {
            //        if (_computeDevice.DeviceType == cDev.DeviceType) {
            //            if (cDev.DeviceBenchmarkConfig.AlgorithmSettings.ContainsKey(AlgorithmType.DaggerHashimoto)) {
            //                cDev.DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.DaggerHashimoto].ExtraLaunchParameters = ExtraLaunchParams;
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        private void buttonBenchmark_Click(object sender, EventArgs e) {
            var device = new List<ComputeDevice>();
            device.Add(_computeDevice);
            var BenchmarkForm = new Form_Benchmark(
                        BenchmarkPerformanceType.Standard,
                        false, _currentlySelectedAlgorithm.NiceHashID);
            BenchmarkForm.ShowDialog();
            fieldBoxBenchmarkSpeed.EntryText = _currentlySelectedAlgorithm.BenchmarkSpeed.ToString();
            // update lvi speed
            if (_currentlySelectedLvi != null) {
                _currentlySelectedLvi.SubItems[2].Text = Helpers.FormatSpeedOutput(_currentlySelectedAlgorithm.BenchmarkSpeed);
            }
        }

    }
}
