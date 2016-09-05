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
    public partial class FormBenchmark : Form, IListItemCheckColorSetter, IBenchmarkComunicator {

        private bool _inBenchmark = false;
        private int _bechmarkCurrentIndex = 0;
        private int _bechmarkedSuccessCount = 0;
        private int _benchmarkAlgorithmsCount = 0;
        private AlgorithmBenchmarkSettingsType _algorithmOption = AlgorithmBenchmarkSettingsType.SelectedUnbenchmarkedAlgorithms;

        private List<Miner> _benchmarkMiners;
        private Miner _currentMiner;
        private List<Tuple<ComputeDevice, Queue<Algorithm>>> _benchmarkDevicesAlgorithmQueue;

        private bool ExitWhenFinished = false;
        private AlgorithmType _singleBenchmarkType = AlgorithmType.NONE;

        private struct DeviceAlgo {
            public string Device { get; set; }
            public string Algorithm { get; set; }
        }
        private List<DeviceAlgo> _benchmarkFailedAlgoPerDev;

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

        public FormBenchmark(BenchmarkPerformanceType benchmarkPerformanceType = BenchmarkPerformanceType.Standard,
            bool autostart = false,
            List<ComputeDevice> enabledDevices = null,
            AlgorithmType singleBenchmarkType = AlgorithmType.NONE) {
            InitializeComponent();

            _singleBenchmarkType = singleBenchmarkType;

            benchmarkOptions1.SetPerformanceType(benchmarkPerformanceType);
            
            // benchmark only unique devices
            devicesListViewEnableControl1.SetIListItemCheckColorSetter(this);
            //devicesListViewEnableControl1.SetAllEnabled = true;
            if (enabledDevices == null) {
                devicesListViewEnableControl1.SetComputeDevices(ComputeDevice.AllAvaliableDevices);
            } else {
                devicesListViewEnableControl1.SetComputeDevices(enabledDevices);
            }

            //groupBoxAlgorithmBenchmarkSettings.Enabled = _singleBenchmarkType == AlgorithmType.NONE;
            devicesListViewEnableControl1.Enabled = _singleBenchmarkType == AlgorithmType.NONE;
            devicesListViewEnableControl1.SetDeviceSelectionChangedCallback(devicesListView1_ItemSelectionChanged);

            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();

            devicesListViewEnableControl1.SetAlgorithmsListView(algorithmsListView1);

            // use this to track miner benchmark statuses
            _benchmarkMiners = new List<Miner>();

            ResetBenchmarkProgressStatus();

            InitLocale();

            if (autostart) {
                ExitWhenFinished = true;
                StartStopBtn_Click(null, null);
            }
        }

        private void InitLocale() {
            this.Text = International.GetText("form2_title"); //International.GetText("SubmitResultDialog_title");
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
                BenchmarkStoppedGUISettings();
            } else if (StartButonClick()) {
                StartStopBtn.Text = International.GetText("form2_buttonStopBenchmark");
            }
        }

        private void BenchmarkStoppedGUISettings() {
            StartStopBtn.Text = International.GetText("form2_buttonStartBenchmark");
            // clear benchmark pending status
            if (_currentAlgorithm != null) _currentAlgorithm.ClearBenchmarkPending();
            foreach (var deviceAlgosTuple in _benchmarkDevicesAlgorithmQueue) {
                foreach (var algo in deviceAlgosTuple.Item2) {
                    algo.ClearBenchmarkPending();
                }
            }
            ResetBenchmarkProgressStatus();
            CalcBenchmarkDevicesAlgorithmQueue();
            //groupBoxAlgorithmBenchmarkSettings.Enabled = true && _singleBenchmarkType == AlgorithmType.NONE;
            benchmarkOptions1.Enabled = true;

            algorithmsListView1.IsInBenchmark = false;
            // TODO make scrolable but not checkable
            //devicesListViewEnableControl1.Enabled = true && _singleBenchmarkType == AlgorithmType.NONE;
            if (_currentDevice != null) {
                algorithmsListView1.RepaintStatus(_currentDevice.ComputeDeviceEnabledOption.IsEnabled, _currentDevice.UUID);
            }

            CloseBtn.Enabled = true;
        }

        // TODO add list for safety and kill all miners
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

            // current failed new list
            _benchmarkFailedAlgoPerDev = new List<DeviceAlgo>();
            // disable gui controls
            //groupBoxAlgorithmBenchmarkSettings.Enabled = false;
            benchmarkOptions1.Enabled = false;
            // TODO make scrolable but not checkable
            //devicesListViewEnableControl1.Enabled = false;
            CloseBtn.Enabled = false;
            algorithmsListView1.IsInBenchmark = true;
            // set benchmark pending status
            foreach (var deviceAlgosTuple in _benchmarkDevicesAlgorithmQueue) {
                foreach (var algo in deviceAlgosTuple.Item2) {
                    algo.SetBenchmarkPending();
                }
            }
            if (_currentDevice != null) {
                algorithmsListView1.RepaintStatus(_currentDevice.ComputeDeviceEnabledOption.IsEnabled, _currentDevice.UUID);
            }

            StartBenchmark();

            return true;
        }

        private void CalcBenchmarkDevicesAlgorithmQueue() {

            _benchmarkAlgorithmsCount = 0;
            _benchmarkDevicesAlgorithmStatus = new Dictionary<string, BenchmarkSettingsStatus>();
            _benchmarkDevicesAlgorithmQueue = new List<Tuple<ComputeDevice, Queue<Algorithm>>>();
            foreach (var option in devicesListViewEnableControl1.Options) {
                var algorithmQueue = new Queue<Algorithm>();
                if (_singleBenchmarkType == AlgorithmType.NONE) {
                    foreach (var kvpAlgorithm in option.CDevice.DeviceBenchmarkConfig.AlgorithmSettings) {
                        if (ShoulBenchmark(kvpAlgorithm.Value)) {
                            algorithmQueue.Enqueue(kvpAlgorithm.Value);
                        }
                    }
                } else { // single bench
                    var algo = option.CDevice.DeviceBenchmarkConfig.AlgorithmSettings[_singleBenchmarkType];
                    algorithmQueue.Enqueue(algo);
                }
                

                BenchmarkSettingsStatus status;
                if (option.IsEnabled) {
                    _benchmarkAlgorithmsCount += algorithmQueue.Count;
                    status = algorithmQueue.Count == 0 ? BenchmarkSettingsStatus.NONE : BenchmarkSettingsStatus.TODO;
                    _benchmarkDevicesAlgorithmQueue.Add(
                    new Tuple<ComputeDevice, Queue<Algorithm>>(option.CDevice, algorithmQueue)
                    );
                } else {
                    status = algorithmQueue.Count == 0 ? BenchmarkSettingsStatus.DISABLED_NONE : BenchmarkSettingsStatus.DISABLED_TODO;
                }
                _benchmarkDevicesAlgorithmStatus.Add(option.CDevice.UUID, status);
            }
            // GUI stuff
            progressBarBenchmarkSteps.Maximum = _benchmarkAlgorithmsCount;
            progressBarBenchmarkSteps.Value = 0;
            SetLabelBenchmarkSteps(0, _benchmarkAlgorithmsCount);
        }

        private bool ShoulBenchmark(Algorithm algorithm) {
            bool isBenchmarked = algorithm.BenchmarkSpeed > 0 ? true : false;
            if (_algorithmOption == AlgorithmBenchmarkSettingsType.SelectedUnbenchmarkedAlgorithms
                && !isBenchmarked && !algorithm.Skip) {
                    return true;
            }
            if (_algorithmOption == AlgorithmBenchmarkSettingsType.UnbenchmarkedAlgorithms && !isBenchmarked) {
                return true;
            }
            if (_algorithmOption == AlgorithmBenchmarkSettingsType.ReBecnhSelectedAlgorithms && !algorithm.Skip) {
                return true;
            }
            if (_algorithmOption == AlgorithmBenchmarkSettingsType.AllAlgorithms) {
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
            if (_bechmarkCurrentIndex > -1) {
                StepUpBenchmarkStepProgress();
            } 
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
                _benchmarkMiners.Add(_currentMiner);
                CurrentAlgoName = AlgorithmNiceHashNames.GetName(_currentAlgorithm.NiceHashID);
                // this has no effect for CPU miners
                _currentMiner.SetCDevs(new string[] { _currentDevice.UUID });

                var time = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits
                    .GetBenchamrktime(benchmarkOptions1.PerformanceType, _currentDevice.DeviceGroupType);
                
                // dagger about 4 minutes
                var showWaitTime = _currentAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto ? 4 * 60 : time;
                
                _currentMiner.BenchmarkStart(_currentDevice, _currentAlgorithm, time, this);
                algorithmsListView1.SetSpeedStatus(_currentDevice, _currentAlgorithm.NiceHashID,
                    GetBenchmarWaitString(GetAlgorithmWaitString(showWaitTime)));
            } else {
                NextBenchmark();
            }

        }

        void EndBenchmark() {
            _inBenchmark = false;
            Helpers.ConsolePrint("FormBenchmark", "EndBenchmark() benchmark routine finished");
            BenchmarkStoppedGUISettings();
            // check if all ok
            if(_benchmarkFailedAlgoPerDev.Count == 0) {
                MessageBox.Show("All benchmakrs have been successful", "Benchmark finished report", MessageBoxButtons.OK);
            } else {
                string msg = "Not all benchmarks finished successfuly. Failed list: " + Environment.NewLine;
                foreach (var failed in _benchmarkFailedAlgoPerDev) {
                    msg += String.Format("{0} : {1}{2}", failed.Device, failed.Algorithm, Environment.NewLine);
                }
                msg += "Retry to rebenchmark unbenchmarked algos or Cancel to disable unbenchmarked.";
                var result = MessageBox.Show(msg, "Benchmark finished report", MessageBoxButtons.RetryCancel);

                if (result == System.Windows.Forms.DialogResult.Retry) {
                    StartButonClick();
                    return;
                } else /*Cancel*/ {
                    // get unbenchmarked from criteria and disable
                    CalcBenchmarkDevicesAlgorithmQueue();
                    foreach (var deviceAlgoQueue in _benchmarkDevicesAlgorithmQueue) {
                        foreach (var algorithm in deviceAlgoQueue.Item2) {
                            algorithm.Skip = true;
                        }
                    }
                }
            }
            if (ExitWhenFinished) {
                this.Close();
            }
        }


        public void SetCurrentStatus(string status) {
            this.Invoke((MethodInvoker)delegate {
                algorithmsListView1.SetSpeedStatus(_currentDevice, _currentAlgorithm.NiceHashID, status);
            });
        }

        public void OnBenchmarkComplete(bool success, string status) {
            if (!_inBenchmark) return;
            this.Invoke((MethodInvoker)delegate {
                _bechmarkedSuccessCount += success ? 1 : 0;
                if (!success) {
                    // add new failed list
                    _benchmarkFailedAlgoPerDev.Add(
                        new DeviceAlgo() {
                            Device = _currentDevice.Name,
                            Algorithm = _currentAlgorithm.NiceHashName
                        } );
                    algorithmsListView1.SetSpeedStatus(_currentDevice, _currentAlgorithm.NiceHashID, status);
                } else {
                    // set status to empty string it will return speed
                    _currentAlgorithm.ClearBenchmarkPending();
                    algorithmsListView1.SetSpeedStatus(_currentDevice, _currentAlgorithm.NiceHashID, "");
                }
                NextBenchmark();
            });
        }

        #region Benchmark progress GUI stuff

        private void SetLabelBenchmarkSteps(int current, int max) {
            labelBenchmarkSteps.Text = String.Format("Benchmark step ( {0} / {1} )", current, max);
        }

        private string GetBenchmarWaitString(string timeExtraString = "") {
            return String.Format("Benchmarking {0}", timeExtraString);
        }

        private string GetAlgorithmWaitString(int timeInSeconds) {
            int num;
            string metric;
            // expect seconds or minutes
            if (timeInSeconds < 60) {
                metric = "seconds";
                num = timeInSeconds;
            } else {
                metric = "minutes";
                num = timeInSeconds / 60;
            }
            return String.Format("(Please wait about {0} {1})", num.ToString(), metric);
        }

        private void StepUpBenchmarkStepProgress() {
            SetLabelBenchmarkSteps(_bechmarkCurrentIndex + 1, _benchmarkAlgorithmsCount);
            progressBarBenchmarkSteps.Value = _bechmarkCurrentIndex + 1;
        }

        private void ResetBenchmarkProgressStatus() {
            progressBarBenchmarkSteps.Value = 0;
        }

        #endregion // Benchmark progress GUI stuff

        private void CloseBtn_Click(object sender, EventArgs e) {
            this.Close();
        }

        //private void radioButton_SelectedUnbenchmarked_CheckedChanged(object sender, EventArgs e) {
        //    _algorithmOption = AlgorithmBenchmarkSettingsType.SelectedUnbenchmarkedAlgorithms;
        //    CalcBenchmarkDevicesAlgorithmQueue();
        //    devicesListViewEnableControl1.ResetListItemColors();
        //}

        //private void radioButton_Unbenchmarked_CheckedChanged(object sender, EventArgs e) {
        //    _algorithmOption = AlgorithmBenchmarkSettingsType.UnbenchmarkedAlgorithms;
        //    CalcBenchmarkDevicesAlgorithmQueue();
        //    devicesListViewEnableControl1.ResetListItemColors();
        //}

        //private void radioButton_ReOnlySelected_CheckedChanged(object sender, EventArgs e) {
        //    _algorithmOption = AlgorithmBenchmarkSettingsType.ReBecnhSelectedAlgorithms;
        //    CalcBenchmarkDevicesAlgorithmQueue();
        //    devicesListViewEnableControl1.ResetListItemColors();
        //}

        //private void radioButton_All_CheckedChanged(object sender, EventArgs e) {
        //    _algorithmOption = AlgorithmBenchmarkSettingsType.AllAlgorithms;
        //    CalcBenchmarkDevicesAlgorithmQueue();
        //    devicesListViewEnableControl1.ResetListItemColors();
        //}

        private void FormBenchmark_New_FormClosing(object sender, FormClosingEventArgs e) {
            if (_inBenchmark) {
                e.Cancel = true;
                return;
            }
            // save already benchmarked algorithms
            ConfigManager.Instance.CommitBenchmarks();
            devicesListViewEnableControl1.SaveOptions();
        }

        private void devicesListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {

            //algorithmSettingsControl1.Deselect();
            // show algorithms
            var _selectedComputeDevice = ComputeDevice.GetCurrentlySelectedComputeDevice(e.ItemIndex, true);
            algorithmsListView1.SetAlgorithms(_selectedComputeDevice, _selectedComputeDevice.ComputeDeviceEnabledOption.IsEnabled);
            //groupBoxAlgorithmSettings.Text = String.Format("Algorithm settings for {0} :", _selectedComputeDevice.Name);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {

        }

        private void radioButton_SelectedUnbenchmarked_CheckedChanged_1(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettingsType.SelectedUnbenchmarkedAlgorithms;
            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();
        }

        private void radioButton_RE_SelectedUnbenchmarked_CheckedChanged(object sender, EventArgs e) {
            _algorithmOption = AlgorithmBenchmarkSettingsType.ReBecnhSelectedAlgorithms;
            CalcBenchmarkDevicesAlgorithmQueue();
            devicesListViewEnableControl1.ResetListItemColors();
        }

    }
}
