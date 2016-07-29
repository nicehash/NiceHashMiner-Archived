using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceHashMiner.Configs {
    public class ConfigManager : BaseLazySingleton<ConfigManager> {
        public GeneralConfig GeneralConfig { get; set; }
        public Dictionary<string, DeviceBenchmarkConfig> BenchmarkConfigs { get; set; }
        

        protected ConfigManager() {
            GeneralConfig = new GeneralConfig();
            BenchmarkConfigs = new Dictionary<string, DeviceBenchmarkConfig>();

            // load configs that are device independant
            GeneralConfig.InitializeConfig();
            if (GeneralConfig.FileLoaded != null) {
                GeneralConfig = GeneralConfig.FileLoaded;
            }
        }

        public void CommitBenchmarks() {
            foreach (var benchConfig in BenchmarkConfigs) {
                benchConfig.Value.Commit();
            }
        }

        private void LoadBenchmarks() {
            foreach (var CDev in ComputeDevice.UniqueAvaliableDevices) {
                var benchConfig = BenchmarkConfigManager.Instance.GetConfig(CDev.DeviceGroupType, CDev.Name, new int[] { CDev.ID });
                benchConfig.InitializeConfig();
                if (benchConfig.FileLoaded != null) {
                    benchConfig = benchConfig.FileLoaded;
                }
                BenchmarkConfigs.Add(CDev.Name, benchConfig);
            }
        }

        public void AfterDeviceQueryInitialization() {
            // initialize group settings
            //SetComputeDeviceConfig();
            ComputeDeviceGroupManager.Instance.InitializeGroupSettings();
            // new stuff
            // set references
            // C# can handle cyclic refs
            LoadBenchmarks();
            BenchmarkConfigManager.Instance.BenchmarkConfigs = BenchmarkConfigs;
            BenchmarkConfigs = BenchmarkConfigManager.Instance.BenchmarkConfigs;
            GeneralConfig.AfterDeviceQueryInitialization();
        }

    }
}
