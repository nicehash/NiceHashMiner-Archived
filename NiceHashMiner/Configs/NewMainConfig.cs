using Newtonsoft.Json;
using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceHashMiner.Configs
{
    public sealed class NewMainConfig
    {
        #region SINGLETON Stuff
        private static NewMainConfig _instance = new NewMainConfig();

        public static NewMainConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NewMainConfig();
                }
                return _instance;
            }
        }
        #endregion


        // copy Legacy Config and merge one by one
        #region OLD
#pragma warning disable 649
        public Version ConfigFileVersion;
        public int Language;
        public string DisplayCurrency;
        public bool DebugConsole;
        public string BitcoinAddress;
        public string WorkerName;
        public int ServiceLocation;
        public bool AutoStartMining;
        public bool HideMiningWindows;
        public bool MinimizeToTray;
        public int LessThreads;
        public int ForceCPUExtension; // 0 - automatic, 1 - SSE2, 2 - AVX, 3 - AVX2
        public int SwitchMinSecondsFixed;
        public int SwitchMinSecondsDynamic;
        public int SwitchMinSecondsAMD;
        public int MinerAPIQueryInterval;
        public int MinerRestartDelayMS;
        public int MinerAPIGraceSeconds;
        public int MinerAPIGraceSecondsAMD;
        public int[] BenchmarkTimeLimitsCPU;
        public int[] BenchmarkTimeLimitsNVIDIA;
        public int[] BenchmarkTimeLimitsAMD;
        public bool DisableDetectionNVidia5X;
        public bool DisableDetectionNVidia3X;
        public bool DisableDetectionNVidia2X;
        public bool DisableDetectionAMD;
        public bool DisableAMDTempControl;
        public bool AutoScaleBTCValues;
        public bool StartMiningWhenIdle;
        public int MinIdleSeconds;
        public bool LogToFile;
        public long LogMaxFileSize;  // in bytes
        public bool ShowDriverVersionWarning;
        public bool DisableWindowsErrorReporting;
        public bool UseNewSettingsPage;
        public bool NVIDIAP0State;
        public int ethminerAPIPortNvidia;
        public int ethminerAPIPortAMD;
        public int ethminerDefaultBlockHeight;
#pragma warning restore 649
        #endregion

        #region NEW
        /// <summary>
        /// LastDevicesSettup field should not be manually edited
        /// </summary>
        public ComputeDevicesSettup LastDevicesSettup;
        //List<ComputeDevicesSettup> ComputeDevicesSettupList;
        #endregion

        // TODO temporary 
        public static NewMainConfig ConfigData;
        private const string configFileLegacy = "config.json";
        private const string configFile = "new_config.json";
        private static bool MigrationConfig = false;

        public static void InitializeConfig()
        {
            // Set defaults
            ConfigData = new NewMainConfig();
            SetDefaults();

            try
            {
                // TODO read from legacy, change if needed
                if (new FileInfo(configFileLegacy).Length > 17000)
                    ConfigData = JsonConvert.DeserializeObject<NewMainConfig>(File.ReadAllText(configFile));
                else
                {
                    File.Delete(configFile);
                }
            }
            catch { }

            // check migration
            if (ConfigData.ConfigFileVersion != null)
            {
                MigrationConfig = ConfigData.ConfigFileVersion.Major <= 1 &&
                    ConfigData.ConfigFileVersion.Minor <= 6 &&
                    ConfigData.ConfigFileVersion.Build < 2;
            }

            if (ConfigData.ConfigFileVersion == null ||
                ConfigData.ConfigFileVersion.CompareTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) != 0)
            {
                Helpers.ConsolePrint("CONFIG", "Config file is from an older version of NiceHashMiner..");
                Helpers.ConsolePrint("CONFIG", "Backing up config.json to config_old.json..");
                try
                {
                    if (File.Exists("config_old.json"))
                        File.Delete("config_old.json");
                    File.Move(configFile, "config_old.json");
                }
                catch { }

                SetDefaults();
            }

            if (ConfigData.SwitchMinSecondsFixed <= 0)
                ConfigData.SwitchMinSecondsFixed = 90;
            if (ConfigData.SwitchMinSecondsDynamic <= 0)
                ConfigData.SwitchMinSecondsDynamic = 30;
            if (ConfigData.SwitchMinSecondsAMD <= 0)
                ConfigData.SwitchMinSecondsAMD = 60;
            if (ConfigData.MinerAPIQueryInterval <= 0)
                ConfigData.MinerAPIQueryInterval = 5;
            if (ConfigData.MinerRestartDelayMS <= 0)
                ConfigData.MinerRestartDelayMS = 500;
            if (ConfigData.MinerAPIGraceSeconds < 0)
                ConfigData.MinerAPIGraceSeconds = 0;
            if (ConfigData.MinerAPIGraceSecondsAMD < 0)
                ConfigData.MinerAPIGraceSecondsAMD = 0;
            if (ConfigData.BenchmarkTimeLimitsCPU == null || ConfigData.BenchmarkTimeLimitsCPU.Length < 3)
                ConfigData.BenchmarkTimeLimitsCPU = new int[] { 10, 20, 60 };
            if (ConfigData.BenchmarkTimeLimitsNVIDIA == null || ConfigData.BenchmarkTimeLimitsNVIDIA.Length < 3)
                ConfigData.BenchmarkTimeLimitsNVIDIA = new int[] { 10, 20, 60 };
            if (ConfigData.BenchmarkTimeLimitsAMD == null || ConfigData.BenchmarkTimeLimitsAMD.Length < 3)
                ConfigData.BenchmarkTimeLimitsAMD = new int[] { 120, 180, 240 };
            if (ConfigData.MinIdleSeconds <= 0)
                ConfigData.MinIdleSeconds = 60;
            if (ConfigData.LogMaxFileSize <= 0)
                ConfigData.LogMaxFileSize = 1048576;
            if (ConfigData.DisplayCurrency == null)
                ConfigData.DisplayCurrency = "USD";
        }

        
        /// <summary>
        /// SetComputeDeviceConfig should be called after Miners are initialized
        /// and then we have our devices.
        /// TODO !!!!! Find a way to change this, make the config stepwise, things are all over the place
        /// </summary>
        public static void SetComputeDeviceConfig()
        {
            if (MigrationConfig && ConfigData.LastDevicesSettup == null)
            {
                // Here is the migration step
                // get LastDevicesSettup settings from groups
                // we have a new settup
                // get device configs settings from legacy config
                Config.SetComputeDeviceConfig();
                // TODO probablly not neded to check the configs settup here
                // since we have config migration migration
                ConfigData.LastDevicesSettup = new ComputeDevicesSettup(0, ComputeDevice.AllAvaliableDevices);
            }
            if (ConfigData.LastDevicesSettup == null) {
                Helpers.ConsolePrint("NewMainConfig", "error in SetComputeDeviceConfig ConfigData.LastDevicesSettup == null");
            }
            else {
                // TODO check device settups
                // if same device settup set enabled/disabled
                // else create new settup
                if (ConfigData.LastDevicesSettup.IsSameDeviceSettup()) {
                    for (int i = 0; i < ComputeDevice.AllAvaliableDevices.Count; ++i) {
                        var usedDevice = ComputeDevice.AllAvaliableDevices[i];
                        var configDevice = ConfigData.LastDevicesSettup.DevicesSettup[i];
                        usedDevice.Enabled = configDevice.Enabled;
                    }
                    // reset config with usedDevices references
                    var ID = ConfigData.LastDevicesSettup.SettupID;
                    ConfigData.LastDevicesSettup = new ComputeDevicesSettup(ID, ComputeDevice.AllAvaliableDevices);
                }
                else {
                    // we have a new device settup, ignore last config
                    // TODO increment ID or mark change
                    ConfigData.LastDevicesSettup = new ComputeDevicesSettup(0, ComputeDevice.AllAvaliableDevices);
                }
            }
        }

        public static void SetDefaults()
        {
            // TODO temp while migration phase
            if (ConfigData == null) return;
            ConfigData.ConfigFileVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ConfigData.Language = 0;
            ConfigData.BitcoinAddress = "";
            ConfigData.WorkerName = "worker1";
            ConfigData.ServiceLocation = 0;
            ConfigData.LessThreads = 0;
            ConfigData.DebugConsole = false;
            ConfigData.HideMiningWindows = false;
            ConfigData.MinimizeToTray = false;
            ConfigData.AutoStartMining = false;
            ConfigData.DisableDetectionNVidia5X = false;
            ConfigData.DisableDetectionNVidia3X = false;
            ConfigData.DisableDetectionNVidia2X = false;
            ConfigData.DisableDetectionAMD = false;
            ConfigData.DisableAMDTempControl = false;
            ConfigData.AutoScaleBTCValues = true;
            ConfigData.StartMiningWhenIdle = false;
            ConfigData.LogToFile = true;
            ConfigData.LogMaxFileSize = 1048576;
            ConfigData.ShowDriverVersionWarning = true;
            ConfigData.DisableWindowsErrorReporting = true;
            ConfigData.UseNewSettingsPage = true;
            ConfigData.NVIDIAP0State = false;
            ConfigData.MinerRestartDelayMS = 500;
            ConfigData.ethminerAPIPortNvidia = 34561;
            ConfigData.ethminerAPIPortAMD = 34562;
            ConfigData.ethminerDefaultBlockHeight = 1700000;
            ConfigData.MinerAPIGraceSeconds = 30;
            ConfigData.MinerAPIGraceSecondsAMD = 60;
            ConfigData.SwitchMinSecondsFixed = 90;
            ConfigData.SwitchMinSecondsDynamic = 30;
            ConfigData.SwitchMinSecondsAMD = 90;
            ConfigData.MinIdleSeconds = 60;
            ConfigData.DisplayCurrency = "USD";
        }

        public static void Commit()
        {
            // TODO temp while migration phase
            if (ConfigData == null) return;
            try { File.WriteAllText(configFile, JsonConvert.SerializeObject(ConfigData, Formatting.Indented)); }
            catch { }
        }

        public static bool ConfigFileExist()
        {
            return File.Exists(configFile);
        }
    }
}
