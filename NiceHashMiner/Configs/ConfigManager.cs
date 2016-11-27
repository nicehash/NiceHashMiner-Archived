using NiceHashMiner.Configs.ConfigJsonFile;
using NiceHashMiner.Configs.Data;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Configs {
    public static class ConfigManager {
        private static readonly string TAG = "ConfigManager";
        public static GeneralConfig GeneralConfig = new GeneralConfig();

        // helper variables
        private static bool IsGeneralConfigFileInit = false;
        private static bool hwidLoadFromFile = false;
        private static bool hwidOK = false;
        private static bool IsNewVersion = false;

        // for loading and saving
        private static GeneralConfigFile GeneralConfigFile = new GeneralConfigFile();
        private static Dictionary<string, DeviceBenchmarkConfigFile> BenchmarkConfigFiles = new Dictionary<string, DeviceBenchmarkConfigFile>();

        // backups
        private static GeneralConfig GeneralConfigBackup = new GeneralConfig();
        private static Dictionary<string, DeviceBenchmarkConfig> BenchmarkConfigsBackup = new Dictionary<string, DeviceBenchmarkConfig>();

        public static void InitializeConfig() {
            // init defaults
            ConfigManager.GeneralConfig.SetDefaults();
            ConfigManager.GeneralConfig.hwid = Helpers.GetCpuID();
            // if exists load file
            GeneralConfig fromFile = null;
            if(GeneralConfigFile.IsFileExists()) {
                fromFile = GeneralConfigFile.ReadFile();
            }
            // just in case
            if (fromFile != null) {
                // set config loaded from file
                IsGeneralConfigFileInit = true;
                ConfigManager.GeneralConfig = fromFile;
                if (ConfigManager.GeneralConfig.ConfigFileVersion == null
                    || ConfigManager.GeneralConfig.ConfigFileVersion.CompareTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) != 0) {
                    Helpers.ConsolePrint(TAG, "Config file is from an older version of NiceHashMiner..");
                    IsNewVersion = true;
                    GeneralConfigFile.CreateBackup();
                } else {
                    Helpers.ConsolePrint(TAG, "Loaded Config file no version detected falling back to defaults.");
                    ConfigManager.GeneralConfig.SetDefaults();
                }
                ConfigManager.GeneralConfig.FixSettingBounds();
                // check vars
                hwidLoadFromFile = true;
                hwidOK = Helpers.GetCpuID() == ConfigManager.GeneralConfig.hwid;
            } else {
                GeneralConfigFileCommit();
            }
        }

        public static bool GeneralConfigIsFileExist() {
            return IsGeneralConfigFileInit;
        }

        public static bool GeneralConfigHwidLoadFromFile() {
            return hwidLoadFromFile;
        }

        public static bool GeneralConfigHwidOK() {
            return hwidOK;
        }

        public static void CreateBackup() {
            GeneralConfigBackup = MemoryHelper.DeepClone(ConfigManager.GeneralConfig);
            BenchmarkConfigsBackup = new Dictionary<string, DeviceBenchmarkConfig>();
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                BenchmarkConfigsBackup[CDev.UUID] = CDev.GetAlgorithmDeviceConfig();
            }
        }

        public static void RestoreBackup() {
            GeneralConfig = GeneralConfigBackup;
            BenchmarkConfigsBackup = new Dictionary<string, DeviceBenchmarkConfig>();
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                if (BenchmarkConfigsBackup.ContainsKey(CDev.UUID)) {
                    CDev.SetAlgorithmDeviceConfig(BenchmarkConfigsBackup[CDev.UUID]);
                }
            }
        }

        public static bool IsRestartNeeded() {
            return ConfigManager.GeneralConfig.DebugConsole != GeneralConfigBackup.DebugConsole
                || ConfigManager.GeneralConfig.NVIDIAP0State != GeneralConfigBackup.NVIDIAP0State
                || ConfigManager.GeneralConfig.LogToFile != GeneralConfigBackup.LogToFile
                || ConfigManager.GeneralConfig.SwitchMinSecondsFixed != GeneralConfigBackup.SwitchMinSecondsFixed
                || ConfigManager.GeneralConfig.SwitchMinSecondsAMD != GeneralConfigBackup.SwitchMinSecondsAMD
                || ConfigManager.GeneralConfig.SwitchMinSecondsDynamic != GeneralConfigBackup.SwitchMinSecondsDynamic
                || ConfigManager.GeneralConfig.MinerAPIQueryInterval != GeneralConfigBackup.MinerAPIQueryInterval;
        }

        public static void GeneralConfigFileCommit() {
            GeneralConfig.LastDevicesSettup.Clear();
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                GeneralConfig.LastDevicesSettup.Add(CDev.GetComputeDeviceConfig());
            }
            GeneralConfigFile.Commit(GeneralConfig);
        }

        public static void CommitBenchmarks() {
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                string devUUID = CDev.UUID;
                if (BenchmarkConfigFiles.ContainsKey(devUUID)) {
                    BenchmarkConfigFiles[devUUID].Commit(CDev.GetAlgorithmDeviceConfig());
                } else {
                    BenchmarkConfigFiles[devUUID] = new DeviceBenchmarkConfigFile(devUUID);
                    BenchmarkConfigFiles[devUUID].Commit(CDev.GetAlgorithmDeviceConfig());
                }
            }
        }

        public static void AfterDeviceQueryInitialization() {
            // extra check (probably will never happen but just in case)
            {
                List<ComputeDevice> invalidDevices = new List<ComputeDevice>();
                foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                    if (CDev.AlgorithmSettings == null) {
                        Helpers.ConsolePrint(TAG, "CRITICAL ISSUE!!! Device has AlgorithmSettings == null. Will remove");
                        invalidDevices.Add(CDev);
                    }
                }
                // remove invalids
                foreach (var invalid in invalidDevices) {
                    ComputeDeviceManager.Avaliable.AllAvaliableDevices.Remove(invalid);
                }
            }
            // set enabled/disabled devs
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                foreach (var devConf in GeneralConfig.LastDevicesSettup) {
                    CDev.SetFromComputeDeviceConfig(devConf);
                }
            }
            // create/init device benchmark configs files and configs
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                string keyUUID = CDev.UUID;
                BenchmarkConfigFiles[keyUUID] = new DeviceBenchmarkConfigFile(keyUUID);
                // init 
                {
                    DeviceBenchmarkConfig currentConfig = null;
                    if (BenchmarkConfigFiles[keyUUID].IsFileExists()) {
                        currentConfig = BenchmarkConfigFiles[keyUUID].ReadFile();
                    }
                    // config exists and file load success set from file
                    if (currentConfig != null) {
                        CDev.SetAlgorithmDeviceConfig(currentConfig);
                        // if new version create backup
                        if (IsNewVersion) {
                            BenchmarkConfigFiles[keyUUID].CreateBackup();
                        }
                    } else {
                        // no config file or not loaded, create new
                        BenchmarkConfigFiles[keyUUID].Commit(CDev.GetAlgorithmDeviceConfig());
                    }
                }
            }
        }


    }
}
