using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceHashMiner.Configs {
    public partial class ConfigManager : BaseLazySingleton<ConfigManager> {
        public GeneralConfig GeneralConfig { get; set; }
        public Dictionary<string, DeviceBenchmarkConfig> BenchmarkConfigs { get; set; }

        protected ConfigManager() {
            GeneralConfig = new GeneralConfig(true);
            BenchmarkConfigs = new Dictionary<string, DeviceBenchmarkConfig>();
        }

        public void CommitBenchmarks() {
            foreach (var benchConfig in BenchmarkConfigs) {
                benchConfig.Value.Commit();
            }
        }

        private void LoadBenchmarks() {
            foreach (var CDev in ComputeDevice.UniqueAvaliableDevices) {
                var benchConfig = DeviceBenchmarkConfigManager.Instance.GetConfig(CDev.DeviceGroupType, CDev.Name);
                benchConfig.InitializeConfig();
                BenchmarkConfigs.Add(CDev.Name, benchConfig);
            }
        }

        public void AfterDeviceQueryInitialization() {
            // initialize group settings
            ComputeDeviceGroupManager.Instance.InitializeGroupSettings();
            // new stuff
            // set references
            // C# can handle cyclic refs
            LoadBenchmarks();
            DeviceBenchmarkConfigManager.Instance.BenchmarkConfigs = BenchmarkConfigs;
            BenchmarkConfigs = DeviceBenchmarkConfigManager.Instance.BenchmarkConfigs;
            GeneralConfig.AfterDeviceQueryInitialization();

            // set Benchmarks for devices
            foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                cdev.SetDeviceBenchmarkConfig(DeviceBenchmarkConfigManager.Instance.GetConfig(cdev.DeviceGroupType, cdev.Name));
            }
            CommitBenchmarks();
        }

    }
}
