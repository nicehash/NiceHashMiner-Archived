using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Configs {
    public partial class ConfigManager : BaseLazySingleton<ConfigManager> {
        public GeneralConfig GeneralConfig { get; set; }
        public Dictionary<string, DeviceBenchmarkConfig> BenchmarkConfigs { get; set; }

        private readonly string TAG;

        // constructor has references don't delete it
        protected ConfigManager() {
            TAG = this.GetType().Name;
            GeneralConfig = new GeneralConfig(true);
            BenchmarkConfigs = new Dictionary<string, DeviceBenchmarkConfig>();
        }

        // TODO remove this eventually, keep for now
        public void LegacyConfigMigration() {
            // CHECK LEGACY config, migration logic
            if (Config.ConfigFileExist()) {
                Helpers.ConsolePrint(TAG, "Migrating LEGACY config");
                Config.InitializeConfig();

                // migrate relevant data
                GeneralConfig.Language = (LanguageType)Config.ConfigData.Language;
                GeneralConfig.BitcoinAddress = Config.ConfigData.BitcoinAddress;
                GeneralConfig.WorkerName = Config.ConfigData.WorkerName;
                GeneralConfig.ServiceLocation = Config.ConfigData.ServiceLocation;
                //GeneralConfig.LessThreads = Config.ConfigData.LessThreads;
                //GeneralConfig.Groups = new Group[0] = Config.ConfigData.Groups = new Group[0];
                GeneralConfig.DebugConsole = Config.ConfigData.DebugConsole;
                GeneralConfig.HideMiningWindows = Config.ConfigData.HideMiningWindows;
                GeneralConfig.MinimizeToTray = Config.ConfigData.MinimizeToTray;
                //GeneralConfig.AutoStartMining = Config.ConfigData.AutoStartMining;
                GeneralConfig.DeviceDetection.DisableDetectionNVidia5X = Config.ConfigData.DisableDetectionNVidia5X;
                GeneralConfig.DeviceDetection.DisableDetectionNVidia3X = Config.ConfigData.DisableDetectionNVidia3X;
                GeneralConfig.DeviceDetection.DisableDetectionNVidia2X = Config.ConfigData.DisableDetectionNVidia2X;
                GeneralConfig.DeviceDetection.DisableDetectionAMD = Config.ConfigData.DisableDetectionAMD;
                //GeneralConfig.DisableAMDTempControl = Config.ConfigData.DisableAMDTempControl;
                GeneralConfig.AutoScaleBTCValues = Config.ConfigData.AutoScaleBTCValues;
                GeneralConfig.StartMiningWhenIdle = Config.ConfigData.StartMiningWhenIdle;
                GeneralConfig.LogToFile = Config.ConfigData.LogToFile;
                GeneralConfig.LogMaxFileSize = Config.ConfigData.LogMaxFileSize;
                GeneralConfig.ShowDriverVersionWarning = Config.ConfigData.ShowDriverVersionWarning;
                GeneralConfig.DisableWindowsErrorReporting = Config.ConfigData.DisableWindowsErrorReporting;
                //GeneralConfig.UseNewSettingsPage = Config.ConfigData.UseNewSettingsPage;
                GeneralConfig.NVIDIAP0State = Config.ConfigData.NVIDIAP0State;
                GeneralConfig.MinerRestartDelayMS = Config.ConfigData.MinerRestartDelayMS;
                GeneralConfig.ethminerDefaultBlockHeight = Config.ConfigData.ethminerDefaultBlockHeight;
                //GeneralConfig.MinerAPIGraceSeconds = Config.ConfigData.MinerAPIGraceSeconds;
                //GeneralConfig.MinerAPIGraceSecondsAMD = Config.ConfigData.MinerAPIGraceSecondsAMD;
                GeneralConfig.SwitchMinSecondsFixed = Config.ConfigData.SwitchMinSecondsFixed;
                GeneralConfig.SwitchMinSecondsDynamic = Config.ConfigData.SwitchMinSecondsDynamic;
                GeneralConfig.SwitchMinSecondsAMD = Config.ConfigData.SwitchMinSecondsAMD;
                GeneralConfig.MinIdleSeconds = Config.ConfigData.MinIdleSeconds;
                GeneralConfig.DisplayCurrency = Config.ConfigData.DisplayCurrency;

                // save migration
                GeneralConfig.Commit();

                Config.DeleteLegacy();
            }
        }

        public void CommitBenchmarks() {
            foreach (var benchConfig in BenchmarkConfigs) {
                benchConfig.Value.Commit();
            }
        }

        private void LoadBenchmarks() {
            foreach (var CDev in ComputeDevice.AllAvaliableDevices) {
                // doubly check if not added
                if (BenchmarkConfigs.ContainsKey(CDev.UUID)) {
                    Helpers.ConsolePrint(TAG, "BUG - LoadBenchmarks() already added for {0}", CDev.UUID);
                } else {
                    var benchConfig = DeviceBenchmarkConfigManager.Instance.GetConfig(CDev.DeviceGroupType, CDev.UUID, CDev.Name);
                    benchConfig.InitializeConfig();
                    BenchmarkConfigs.Add(CDev.UUID, benchConfig);
                }
            }
        }

        public void AfterDeviceQueryInitialization() {
            GeneralConfig.AfterDeviceQueryInitialization();
            LoadBenchmarks();            

            SetDeviceBenchmarkReferences();

            

            // check ethminers and remove from settings if no device supports it in config
            foreach (var config in BenchmarkConfigs) {
                bool removeDagger = true;
                var cDev = ComputeDevice.GetDeviceWithUUID(config.Value.DeviceUUID);
                if (cDev != null) {
                    // if only one dev ing roup supports terminate
                    if (cDev.IsEtherumCapale) {
                        removeDagger = false;
                        break;
                    }
                }
                if (removeDagger) {
                    config.Value.AlgorithmSettings.Remove(AlgorithmType.DaggerHashimoto);
                }
            }
            
            CommitBenchmarks();
        }

        public void SetDeviceBenchmarkReferences() {
            // new stuff
            // set references
            // C# can handle cyclic refs
            DeviceBenchmarkConfigManager.Instance.BenchmarkConfigs = BenchmarkConfigs;
            BenchmarkConfigs = DeviceBenchmarkConfigManager.Instance.BenchmarkConfigs;
            // set Benchmarks for devices
            foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                cdev.SetDeviceBenchmarkConfig(DeviceBenchmarkConfigManager.Instance.GetConfig(cdev.DeviceGroupType, cdev.UUID, cdev.Name));
            }
        }

    }
}
