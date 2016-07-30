using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs {
    
    public class DeviceBenchmarkConfigManager : BaseLazySingleton<DeviceBenchmarkConfigManager> {

        private Dictionary<string, DeviceBenchmarkConfig> _benchmarkConfigs;
        public Dictionary<string, DeviceBenchmarkConfig> BenchmarkConfigs {
            get { return _benchmarkConfigs; }
            set {
                if (value != null) {
                    _benchmarkConfigs = value; 
                }
            }
        }

        protected DeviceBenchmarkConfigManager() {
            _benchmarkConfigs = new Dictionary<string, DeviceBenchmarkConfig>();
        }

        public DeviceBenchmarkConfig GetConfig(string deviceName) {
            DeviceBenchmarkConfig retConfig = null;

            if (_benchmarkConfigs.TryGetValue(deviceName, out retConfig) == false) {
                // TODO for now do nothing
                retConfig = null;
            }

            return retConfig;
        }

        public DeviceBenchmarkConfig GetConfig(DeviceGroupType deviceGroupType,
            string deviceName, int[] devicesIDs) {
            DeviceBenchmarkConfig retConfig = GetConfig(deviceName);
            if (retConfig == null) {
                retConfig = new DeviceBenchmarkConfig(deviceGroupType, deviceName, devicesIDs, null);
                _benchmarkConfigs.Add(deviceName, retConfig);
            }

            return retConfig;
        }

    }
}
