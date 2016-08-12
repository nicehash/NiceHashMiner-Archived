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

namespace NiceHashMiner.Forms {
    public partial class FormBenchmark_New : Form {

        private bool _inBenchmark = false;
        private int _bechmarkCurrentIndex = 0;
        private int _bechmarkedSuccessCount = 0;
        private int _benchmarkAlgorithmsCount = 0;
        private AlgorithmBenchmarkSettings _algorithmOption = AlgorithmBenchmarkSettings.SelectedUnbenchmarkedAlgorithms;
        private Miner _currentMiner;
        private List<Tuple<ComputeDevice, Queue<Algorithm>>> _benchmarkDevicesAlgorithmQueue;
        private ComputeDevice _currentDevice;
        private Algorithm _currentAlgorithm;

        BenchmarkPerformanceType _benchmarkPerformanceType;

        private string CurrentAlgoName;

        private enum AlgorithmBenchmarkSettings : int {
            SelectedUnbenchmarkedAlgorithms,
            UnbenchmarkedAlgorithms,
            ReBecnhSelectedAlgorithms,
            AllAlgorithms
        }

        public FormBenchmark_New(BenchmarkPerformanceType benchmarkPerformanceType = BenchmarkPerformanceType.Standard) {
            InitializeComponent();

            _benchmarkPerformanceType = benchmarkPerformanceType;

            // benchmark only unique devices
            devicesListViewEnableControl1.SetAllEnabled = true;
            devicesListViewEnableControl1.SetComputeDevices(ComputeDevice.UniqueAvaliableDevices);
        }

        private void InitLocale() {
            this.Text = International.GetText("SubmitResultDialog_title");
            //labelInstruction.Text = International.GetText("SubmitResultDialog_labelInstruction");
            StartStopBtn.Text = International.GetText("SubmitResultDialog_StartBtn");
            CloseBtn.Text = International.GetText("SubmitResultDialog_CloseBtn");

            // TODO fix locale for benchmark enabled label
            devicesListViewEnableControl1.InitLocale();
        }

        private void StartStopBtn_Click(object sender, EventArgs e) {

            if (_inBenchmark) {
                _inBenchmark = false;
            } else {
                _benchmarkAlgorithmsCount = 0;
                bool noneSelected = true;
                _benchmarkDevicesAlgorithmQueue = new List<Tuple<ComputeDevice, Queue<Algorithm>>>();
                foreach (var option in devicesListViewEnableControl1.Options) {
                    if (option.IsEnabled) {
                        noneSelected = false;
                        var algorithmQueue = new Queue<Algorithm>();
                        foreach (var kvpAlgorithm in option.CDevice.DeviceBenchmarkConfig.AlgorithmSettings) {
                            if(ShoulBenchmark(kvpAlgorithm.Value)) {
                                ++_benchmarkAlgorithmsCount;
                                algorithmQueue.Enqueue(kvpAlgorithm.Value);
                            }
                        }
                        _benchmarkDevicesAlgorithmQueue.Add(
                            new Tuple<ComputeDevice, Queue<Algorithm>>(option.CDevice, algorithmQueue)
                            );
                    }
                }
                if (noneSelected && _benchmarkAlgorithmsCount != 0) {
                    // TODO msg box none selected
                    return;
                }

                BenchmarkProgressBar.Maximum = _benchmarkAlgorithmsCount;
                StartBenchmark();
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
                    .GetBenchamrktime(_benchmarkPerformanceType, _currentDevice.DeviceGroupType);
                time = 5;
                // TODO tmp
                _currentMiner.BenchmarkStart(currentConfig, _currentAlgorithm, time, BenchmarkCompleted, null);
            } else {
                NextBenchmark();
            }

        }

        void EndBenchmark() {
            _inBenchmark = false;
            //SplitGroupColors();
        }

        private void BenchmarkCompleted(bool success, string text, object tag) {
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
        }

        private void radioButton_Unbenchmarked_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.UnbenchmarkedAlgorithms;
        }

        private void radioButton_ReOnlySelected_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.ReBecnhSelectedAlgorithms;
        }

        private void radioButton_All_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettings.AllAlgorithms;
        }


    }
}
