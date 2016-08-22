using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;
using NiceHashMiner.Interfaces;

namespace NiceHashMiner.Forms {
    public partial class FormBenchmark_New : Form, IListItemCheckColorSetter {

        private bool _inBenchmark = false;
        private int _bechmarkCurrentIndex = 0;
        private int _bechmarkedSuccessCount = 0;
        private int _benchmarkAlgorithmsCount = 0;
        private AlgorithmBenchmarkSettings _algorithmOption = AlgorithmBenchmarkSettings.SelectedUnbenchmarkedAlgorithms;
        private Miner _currentMiner;
        private List<Tuple<ComputeDevice, Queue<Algorithm>>> _benchmarkDevicesAlgorithmQueue;

        private enum BenchmarkSettingsStatus : int {
            NONE = 0,
            TODO,
            DISABLED_NONE,
            DISABLED_TODO
        }
        private Dictionary<string, BenchmarkSettingsStatus> _benchmarkDevicesAlgorithmStatus;
        private ComputeDevice _currentDevice;
        private Algorithm _currentAlgorithm;

        private string CurrentAlgoName;

        private enum AlgorithmBenchmarkSettings : int {
            SelectedUnbenchmarkedAlgorithms,
            UnbenchmarkedAlgorithms,
            ReBecnhSelectedAlgorithms,
            AllAlgorithms
        }

        private static Color DISABLED_COLOR = Color.DarkGray;
        private static Color BENCHMARKED_COLOR = Color.LightGreen;
        private static Color UNBENCHMARKED_COLOR = Color.LightBlue;
        public void LviSetColor(ListViewItem lvi) {
            var cdvo = lvi.Tag as NiceHashMiner.Forms.Components.DevicesListViewEnableControl.ComputeDeviceEnabledOption;
            if (cdvo != null && _benchmarkDevicesAlgorithmStatus != null) {
                var uuid = cdvo.CDevice.UUID;
                if (!cdvo.IsEnabled) {
                    lvi.BackColor = DISABLED_COLOR;
                } else {
                    switch (_benchmarkDevicesAlgorithmStatus[uuid]) {
                        case BenchmarkSettingsStatus.TODO:
                        case BenchmarkSettingsStatus.DISABLED_TODO:
                            lvi.BackColor = UNBENCHMARKED_COLOR;
                            break;
                        case BenchmarkSettingsStatus.NONE:
                        case BenchmarkSettingsStatus.DISABLED_NONE:
                            lvi.BackColor = BENCHMARKED_COLOR;
                            break;
                    }
                }
                //// enable disable status, NOT needed
                //if (cdvo.IsEnabled && _benchmarkDevicesAlgorithmStatus[uuid] >= BenchmarkSettingsStatus.DISABLED_NONE) {
                //    _benchmarkDevicesAlgorithmStatus[uuid] -= 2;
                //} else if (!cdvo.IsEnabled && _benchmarkDevicesAlgorithmStatus[uuid] <= BenchmarkSettingsStatus.TODO) {
                //    _benchmarkDevicesAlgorithmStatus[uuid] += 2;
                //}
            }
        }

        public FormBenchmark_New(BenchmarkPerformanceType benchmarkPerformanceType = BenchmarkPerformanceType.Standard) {
            InitializeComponent();

            benchmarkOptions1.SetPerformanceType(benchmarkPerformanceType);

            // benchmark only unique devices
            devicesListViewEnableControl1.SetIListItemCheckColorSetter(this);
            devicesListViewEnableControl1.SetAllEnabled = true;
            devicesListViewEnableControl1.SetComputeDevices(ComputeDevice.UniqueAvaliableDevices);

            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();

            InitLocale();
        }

        private void InitLocale() {
            this.Text = International.GetText("SubmitResultDialog_title");
            //labelInstruction.Text = International.GetText("SubmitResultDialog_labelInstruction");
            StartStopBtn.Text = International.GetText("SubmitResultDialog_StartBtn");
            CloseBtn.Text = International.GetText("SubmitResultDialog_CloseBtn");

            // TODO fix locale for benchmark enabled label
            devicesListViewEnableControl1.InitLocale();
            benchmarkOptions1.InitLocale();
        }

        private void StartStopBtn_Click(object sender, EventArgs e) {
            if (_inBenchmark) {
                StopButonClick();
                StartStopBtn.Text = International.GetText("form2_buttonStartBenchmark");
            } else if (StartButonClick()) {
                StartStopBtn.Text = International.GetText("form2_buttonStopBenchmark");
            }
        }

        private void StopButonClick() {
            _inBenchmark = false;
            Helpers.ConsolePrint("FormBenchmark", "StopButonClick() benchmark routine stopped");
            if (_currentMiner != null) {
                _currentMiner.BenchmarkSignalQuit = true;
            }
        }

        private bool StartButonClick() {
            CalcBenchmarkDevicesAlgorithmQueue();
            // device selection check scope
            {
                bool noneSelected = true;
                foreach (var option in devicesListViewEnableControl1.Options) {
                    if (option.IsEnabled) {
                        noneSelected = false;
                        break;
                    }
                }
                if (noneSelected) {
                    MessageBox.Show("No device has been selected there is nothing to benchmark", "No device selected", MessageBoxButtons.OK);
                    return false;
                }
            }
            // device todo benchmark check scope
            {
                bool nothingToBench = true;
                foreach (var statusKpv in _benchmarkDevicesAlgorithmStatus) {
                    if (statusKpv.Value == BenchmarkSettingsStatus.TODO) {
                        nothingToBench = false;
                        break;
                    }
                }
                if (nothingToBench) {
                    MessageBox.Show("Current benchmark settings are already executed. There is nothing to do.", "Nothing to benchmark", MessageBoxButtons.OK);
                    return false;
                }
            }


            BenchmarkProgressBar.Maximum = _benchmarkAlgorithmsCount;
            StartBenchmark();

            return true;
        }

        private void CalcBenchmarkDevicesAlgorithmQueue() {
            _benchmarkAlgorithmsCount = 0;
            _benchmarkDevicesAlgorithmStatus = new Dictionary<string, BenchmarkSettingsStatus>();
            _benchmarkDevicesAlgorithmQueue = new List<Tuple<ComputeDevice, Queue<Algorithm>>>();
            foreach (var option in devicesListViewEnableControl1.Options) {
                var algorithmQueue = new Queue<Algorithm>();
                foreach (var kvpAlgorithm in option.CDevice.DeviceBenchmarkConfig.AlgorithmSettings) {
                    if (ShoulBenchmark(kvpAlgorithm.Value)) {
                        ++_benchmarkAlgorithmsCount;
                        algorithmQueue.Enqueue(kvpAlgorithm.Value);
                    }
                }

                BenchmarkSettingsStatus status;
                if (option.IsEnabled) {
                    status = algorithmQueue.Count == 0 ? BenchmarkSettingsStatus.NONE : BenchmarkSettingsStatus.TODO;
                    _benchmarkDevicesAlgorithmQueue.Add(
                    new Tuple<ComputeDevice, Queue<Algorithm>>(option.CDevice, algorithmQueue)
                    );
                } else {
                    status = algorithmQueue.Count == 0 ? BenchmarkSettingsStatus.DISABLED_NONE : BenchmarkSettingsStatus.DISABLED_TODO;
                }
                _benchmarkDevicesAlgorithmStatus.Add(option.CDevice.UUID, status);
            }
        }

        private bool ShoulBenchmark(Algorithm algorithm) {
            bool isBenchmarked = algorithm.BenchmarkSpeed > 0 ? true : false;
            if (_algorithmOption == AlgorithmBenchmarkSettings.SelectedUnbenchmarkedAlgorithms
                && !isBenchmarked && !algorithm.Skip) {
                    return true;
            }
            if (_algorithmOption == AlgorithmBenchmarkSettings.UnbenchmarkedAlgorithms && !isBenchmarked) {
                return true;
            }
            if (_algorithmOption == AlgorithmBenchmarkSettings.ReBecnhSelectedAlgorithms && !algorithm.Skip) {
                return true;
            }
            if (_algorithmOption == AlgorithmBenchmarkSettings.AllAlgorithms) {
                return true;
            }

            return false;
        }

        void StartBenchmark() {
            _inBenchmark = true;
            _bechmarkCurrentIndex = -1;
            NextBenchmark();
        }

        void NextBenchmark() {
            ++_bechmarkCurrentIndex;
            if (_bechmarkCurrentIndex >= _benchmarkAlgorithmsCount) {
                EndBenchmark();
                return;
            }

            Tuple<ComputeDevice, Queue<Algorithm>> currentDeviceAlgosTuple;
            Queue<Algorithm> algorithmBenchmarkQueue;
            while (_benchmarkDevicesAlgorithmQueue.Count > 0) {
                currentDeviceAlgosTuple = _benchmarkDevicesAlgorithmQueue[0];
                _currentDevice = currentDeviceAlgosTuple.Item1;
                algorithmBenchmarkQueue = currentDeviceAlgosTuple.Item2;
                if(algorithmBenchmarkQueue.Count != 0) {
                    _currentAlgorithm = algorithmBenchmarkQueue.Dequeue();
                    break;
                } else {
                    _benchmarkDevicesAlgorithmQueue.RemoveAt(0);
                }
            }

            var currentConfig = _currentDevice.DeviceBenchmarkConfig;
            if (_currentDevice.DeviceGroupType == DeviceGroupType.CPU) {
                _currentMiner = MinersManager.GetCpuMiner(_currentDevice.Group);
            } else {
                _currentMiner = MinersManager.CreateMiner(currentConfig.DeviceGroupType, _currentAlgorithm.NiceHashID);
            }

            if (_currentMiner != null && _currentAlgorithm != null) {
                CurrentAlgoName = AlgorithmNiceHashNames.GetName(_currentAlgorithm.NiceHashID);
                UpdateProgressBar(false);
                // this has no effect for CPU miners
                _currentMiner.SetCDevs(new string[] { _currentDevice.UUID });

                var time = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits
                    .GetBenchamrktime(benchmarkOptions1.PerformanceType, _currentDevice.DeviceGroupType);
                //time = 5;
                // TODO tmp
                _currentMiner.BenchmarkStart(currentConfig, _currentAlgorithm, time, BenchmarkCompleted, null);
            } else {
                NextBenchmark();
            }

        }

        void EndBenchmark() {
            _inBenchmark = false;
            Helpers.ConsolePrint("FormBenchmark", "EndBenchmark() benchmark routine finished");
        }

        private void BenchmarkCompleted(bool success, string text, object tag) {
            if (!_inBenchmark) return;

            if (this.InvokeRequired) {
                BenchmarkComplete d = new BenchmarkComplete(BenchmarkCompleted);
                this.Invoke(d, new object[] { success, text, tag });
            } else {
                _bechmarkedSuccessCount += success ? 1 : 0;
                UpdateProgressBar(true);
                NextBenchmark();
            }
        }

        private void UpdateProgressBar(bool step) {
            if (step) BenchmarkProgressBar.PerformStep();
            LabelProgressPercentage.Text = String.Format(International.GetText("SubmitResultDialog_LabelProgressPercentageInProgress"),
                                           ((double)((double)BenchmarkProgressBar.Value / (double)BenchmarkProgressBar.Maximum) * 100).ToString("F2"), CurrentAlgoName);
        }

        private void CloseBtn_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void radioButton_SelectedUnbenchmarked_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.SelectedUnbenchmarkedAlgorithms;
            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();
        }

        private void radioButton_Unbenchmarked_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.UnbenchmarkedAlgorithms;
            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();
        }

        private void radioButton_ReOnlySelected_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.ReBecnhSelectedAlgorithms;
            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();
        }

        private void radioButton_All_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.AllAlgorithms;
            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();
        }


    }
}
