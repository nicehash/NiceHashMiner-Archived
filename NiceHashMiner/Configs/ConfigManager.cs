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
        public static Dictionary<string, DeviceBenchmarkConfig> BenchmarkConfigs = new Dictionary<string, DeviceBenchmarkConfig>();

        public static GeneralConfigFile GeneralConfigFile = new GeneralConfigFile();

        public static void Initialize() {
            // init defaults
            ConfigManager.GeneralConfig.SetDefaults();
            ConfigManager.GeneralConfig.hwid = Helpers.GetCpuID();
            // load file
            
        }


        public static void CommitBenchmarks() {
            foreach (var benchConfig in BenchmarkConfigs) {
                benchConfig.Value.Commit();
            }
        }

        private static void LoadBenchmarks() {
            foreach (var CDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
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

        public static void AfterDeviceQueryInitialization() {
            //GeneralConfig.AfterDeviceQueryInitialization();
            LoadBenchmarks();

            //SetDeviceBenchmarkReferences();

            // check ethminers and remove from settings if no device supports it in config
            foreach (var config in BenchmarkConfigs) {
                bool removeDagger = true;
                var cDev = ComputeDeviceManager.Avaliable.GetDeviceWithUUID(config.Value.DeviceUUID);
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
    }
}
