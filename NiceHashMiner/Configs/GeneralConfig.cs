using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using Newtonsoft.Json;
using System.IO;

namespace NiceHashMiner.Configs {
    [Serializable]
    public class GeneralConfig : BaseConfigFile<GeneralConfig> {

        #region Members
        public Version ConfigFileVersion { get; set; }
        public LanguageType Language { get; set; }

        private string _displayCurrency = "USD";
        public string DisplayCurrency {
            get { return _displayCurrency; }
            set { _displayCurrency = value == null ? "USD" : value; }
        }

        public bool DebugConsole { get; set; }
        public string BitcoinAddress { get; set; }
        public string WorkerName { get; set; }
        public int ServiceLocation { get; set; }
        public bool AutoStartMining { get; set; }
        public bool HideMiningWindows { get; set; }
        public bool MinimizeToTray { get; set; }
        //public int LessThreads { get; set; }
        public CPUExtensionType ForceCPUExtension { get; set; } // 0 - automatic, 1 - SSE2, 2 - AVX, 3 - AVX2

        private int _switchMinSecondsFixed = 90;
        public int SwitchMinSecondsFixed {
            get { return _switchMinSecondsFixed; }
            set {
                _switchMinSecondsFixed = value <= 0 ? 90 : value;
            }
        }
        private int _switchMinSecondsDynamic = 30;
        public int SwitchMinSecondsDynamic {
            get { return _switchMinSecondsDynamic; }
            set {
                _switchMinSecondsDynamic = value <= 0 ? 30 : value;
            }
        }

        private int _switchMinSecondsAMD = 60;
        public int SwitchMinSecondsAMD {
            get { return _switchMinSecondsAMD; }
            set { _switchMinSecondsAMD = value <= 0 ? 60 : value; }
        }

        private int _minerAPIQueryInterval = 5;
        public int MinerAPIQueryInterval {
            get { return _minerAPIQueryInterval; }
            set { _minerAPIQueryInterval = value <= 0 ? 5 : value;}
        }

        private int _minerRestartDelayMS = 500;
        public int MinerRestartDelayMS {
            get { return _minerRestartDelayMS; }
            set { _minerRestartDelayMS = value <= 0 ? 500 : value; }
        }

        public BenchmarkTimeLimitsConfig BenchmarkTimeLimits { get; set; }
        public DeviceDetectionConfig DeviceDetection { get; set; }

        public bool DisableAMDTempControl { get; set; }
        public bool DisableDefaultOptimizations { get; set; }
        
        public bool AutoScaleBTCValues { get; set; }
        public bool StartMiningWhenIdle { get; set; }

        private int _minIdleSeconds = 60;
        public int MinIdleSeconds {
            get { return _minIdleSeconds; }
            set { _minIdleSeconds = value <= 0 ? 60 : value; }
        }
        public bool LogToFile { get; set; }

        // in bytes
        private long _logMaxFileSize = 1048576;
        public long LogMaxFileSize {
            get { return _logMaxFileSize; }
            set { _logMaxFileSize = value <= 0 ? 1048576 : value; }
        }

        public bool ShowDriverVersionWarning { get; set; }
        public bool DisableWindowsErrorReporting { get; set; }
        public bool NVIDIAP0State { get; set; }

        public int ethminerDefaultBlockHeight { get; set; }
        public DagGenerationType EthminerDagGenerationType;

        private int _apiBindPortPoolStart = 5100;
        public int ApiBindPortPoolStart {
            get { return _apiBindPortPoolStart; }
            set {
                // check port start number, leave about 2000 ports pool size, huge yea!
                if (value < (65535 - 2000)) {
                    _apiBindPortPoolStart = value;
                } else {
                    // set default
                    _apiBindPortPoolStart = 5100;
                }
            }
        }
        public double MinimumProfit { get; set; }

        public bool ContinueMiningIfNoInternetAccess { get; set; }

        public string hwid { get; set; }

        public bool DownloadInit { get; set; }


        [JsonIgnore]
        public bool hwidOK { get; private set; }
        [JsonIgnore]
        public bool hwidLoadFromFile { get; private set; }
        [JsonIgnore]
        public bool IsNewVersion { get; private set; }


        // After Device initialization

        /// <summary>
        /// LastDevicesSettup field should not be manually edited
        /// The changes can be of two scenarios:
        /// #1 Detect if the device is enabled/disabled,
        /// #2 Detect hardware changes/upgrades such as CPUs and GPUs.
        /// </summary>
        public List<ComputeDevice> LastDevicesSettup { get; set; }

        #endregion //Members

        public bool IsFileExist() {
            return FileLoaded;
        }

        public void SetDefaults() {
            ConfigFileVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Language = LanguageType.En;
            BitcoinAddress = "";
            WorkerName = "worker1";
            ServiceLocation = 0;
            AutoStartMining = false;
            //LessThreads = 0;
            DebugConsole = false;
            HideMiningWindows = false;
            MinimizeToTray = false;
            BenchmarkTimeLimits = new BenchmarkTimeLimitsConfig();
            DeviceDetection = new DeviceDetectionConfig();
            DisableAMDTempControl = false;
            DisableDefaultOptimizations = false;
            AutoScaleBTCValues = true;
            StartMiningWhenIdle = false;
            LogToFile = true;
            LogMaxFileSize = 1048576;
            ShowDriverVersionWarning = true;
            DisableWindowsErrorReporting = true;
            NVIDIAP0State = false;
            MinerRestartDelayMS = 500;
            ethminerDefaultBlockHeight = 2000000;
            SwitchMinSecondsFixed = 90;
            SwitchMinSecondsDynamic = 30;
            SwitchMinSecondsAMD = 90;
            MinIdleSeconds = 60;
            DisplayCurrency = "USD";
            ApiBindPortPoolStart = 4000;
            MinimumProfit = 0;
            EthminerDagGenerationType = DagGenerationType.SingleKeep;
            DownloadInit = false;
            ContinueMiningIfNoInternetAccess = false;
        }

        public GeneralConfig(bool initDefaults = false) {
            ConfigFileVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            BenchmarkTimeLimits = new BenchmarkTimeLimitsConfig();
            DeviceDetection = new DeviceDetectionConfig();

            hwidOK = false;
            hwidLoadFromFile = false;
            IsNewVersion = false;

            // only init defaults for created config not read
            if (initDefaults) {
                SetDefaults();
                hwid = Helpers.GetCpuID();
            }
        }

        protected override void InitializePaths() {
            FilePath = "General.json";
            FilePathOld = "General_old.json";
        }
        protected override void InitializeObject() {
            if (_file.ConfigFileVersion == null ||
                _file.ConfigFileVersion.CompareTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) != 0) {
                IsNewVersion = true;
                Helpers.ConsolePrint("GeneralConfig", "Config file is from an older version of NiceHashMiner..");
                Helpers.ConsolePrint("GeneralConfig", String.Format("Backing up {0} to {1}..", FilePath, FilePathOld));
                try {
                    if (File.Exists(FilePathOld))
                        File.Delete(FilePathOld);
                    File.Move(FilePath, FilePathOld);
                } catch { }

                SetDefaults();
            }


            // init fields
            Language = _file.Language;
            DisplayCurrency = _file.DisplayCurrency;
            DebugConsole = _file.DebugConsole;
            BitcoinAddress = _file.BitcoinAddress;
            WorkerName = _file.WorkerName;
            ServiceLocation = _file.ServiceLocation;
            AutoStartMining = _file.AutoStartMining;
            HideMiningWindows = _file.HideMiningWindows;
            MinimizeToTray = _file.MinimizeToTray;
            //LessThreads = _file.LessThreads;
            ForceCPUExtension = _file.ForceCPUExtension;
            SwitchMinSecondsFixed  = _file.SwitchMinSecondsFixed;
            SwitchMinSecondsDynamic = _file.SwitchMinSecondsDynamic;
            SwitchMinSecondsAMD = _file.SwitchMinSecondsAMD;
            MinerAPIQueryInterval = _file.MinerAPIQueryInterval;
            MinerRestartDelayMS = _file.MinerRestartDelayMS;
            if (_file.BenchmarkTimeLimits != null) {
                this.BenchmarkTimeLimits = _file.BenchmarkTimeLimits;
            }
            if (_file.DeviceDetection != null) {
                this.DeviceDetection = _file.DeviceDetection;
            }
            AutoScaleBTCValues = _file.AutoScaleBTCValues;
            StartMiningWhenIdle = _file.StartMiningWhenIdle;
            MinIdleSeconds = _file.MinIdleSeconds;
            LogToFile = _file.LogToFile;
            LogMaxFileSize = _file.LogMaxFileSize;
            ShowDriverVersionWarning  = _file.ShowDriverVersionWarning;
            DisableWindowsErrorReporting  = _file.DisableWindowsErrorReporting;
            NVIDIAP0State = _file.NVIDIAP0State;
            ethminerDefaultBlockHeight = _file.ethminerDefaultBlockHeight;
            EthminerDagGenerationType = _file.EthminerDagGenerationType;
            ApiBindPortPoolStart = _file.ApiBindPortPoolStart;
            MinimumProfit = _file.MinimumProfit;

            DisableAMDTempControl = _file.DisableAMDTempControl;
            DisableDefaultOptimizations = _file.DisableDefaultOptimizations;

            ContinueMiningIfNoInternetAccess = _file.ContinueMiningIfNoInternetAccess;
            if (!IsNewVersion) {
                DownloadInit = _file.DownloadInit;
            }

            hwidLoadFromFile = true;
            hwidOK = this.hwid == _file.hwid;
        }

        public void AfterDeviceQueryInitialization() {

            if (_file != null && _file.LastDevicesSettup != null) {
                foreach (var configDevice in _file.LastDevicesSettup) {
                    foreach (var usedDevice in ComputeDevice.AllAvaliableDevices) {
                        if (configDevice.UUID == usedDevice.UUID) {
                            usedDevice.Enabled = configDevice.Enabled;
                            continue;
                        }
                    }
                }
            }

            LastDevicesSettup = ComputeDevice.AllAvaliableDevices;
        }
    }
}
