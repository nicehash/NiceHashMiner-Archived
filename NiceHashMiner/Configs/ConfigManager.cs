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

        public void CommitBenchmarks() {
            foreach (var benchConfig in BenchmarkConfigs) {
                benchConfig.Value.Commit();
            }
        }

        private void LoadBenchmarks() {
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

        public void AfterDeviceQueryInitialization() {
            GeneralConfig.AfterDeviceQueryInitialization();
            LoadBenchmarks();            

            SetDeviceBenchmarkReferences();

            

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

        public void SetDeviceBenchmarkReferences() {
            // new stuff
            // set references
            // C# can handle cyclic refs
            DeviceBenchmarkConfigManager.Instance.BenchmarkConfigs = BenchmarkConfigs;
            BenchmarkConfigs = DeviceBenchmarkConfigManager.Instance.BenchmarkConfigs;
            // set Benchmarks for devices
            foreach (var cdev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                cdev.SetDeviceBenchmarkConfig(DeviceBenchmarkConfigManager.Instance.GetConfig(cdev.DeviceGroupType, cdev.UUID, cdev.Name));
            }
        }

    }
}
