using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs {
    
    public class BenchmarkConfigManager : SingletonTemplate<BenchmarkConfigManager> {

        private Dictionary<string, BenchmarkConfig> _benchmarkConfigs;
        public Dictionary<string, BenchmarkConfig> BenchmarkConfigs {
            get { return _benchmarkConfigs; }
            set {
                if (value != null) {
                    _benchmarkConfigs = value; 
                }
            }
        }

        public BenchmarkConfigManager() {
            _benchmarkConfigs = new Dictionary<string, BenchmarkConfig>();
        }

        public BenchmarkConfig GetConfig(string hashKey) {
            BenchmarkConfig retConfig = null;

            if (_benchmarkConfigs.TryGetValue(hashKey, out retConfig) == false) {
                // TODO for now do nothing
                retConfig = null;
            }

            return retConfig;
        }

        public BenchmarkConfig GetConfig(DeviceGroupType deviceGroupType,
            string deviceGroupName, int[] devicesIDs) {
            string hashKey = BenchmarkConfig.GetId(deviceGroupType, deviceGroupName, devicesIDs);
            BenchmarkConfig retConfig = GetConfig(hashKey);
            if (retConfig == null) {
                retConfig = new BenchmarkConfig(deviceGroupType, deviceGroupName, devicesIDs, null);
                _benchmarkConfigs.Add(hashKey, retConfig);
            }

            return retConfig;
        }

    }
}
