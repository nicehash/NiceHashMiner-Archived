using NiceHashMiner.Devices;
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

        /// <summary>
        /// IsEnabledBenchmarksInitialized is to check if currently enabled devices have all enabled algorithms benchmarked.
        /// </summary>
        /// <returns>Returns tuple of boolean and dictionary of unbenchmarked algorithms per device</returns>
        public Tuple<bool, Dictionary<string, List<AlgorithmType>> > IsEnabledBenchmarksInitialized() {
            bool isEnabledBenchmarksInitialized = true;
            // first get all enabled devices names
            HashSet<string> enabledDevicesNames = new HashSet<string>();
            foreach (var device in ComputeDevice.AllAvaliableDevices) {
                if (device.Enabled) {
                    enabledDevicesNames.Add(device.Name);
                }
            }
            // get enabled unbenchmarked algorithms
            Dictionary<string, List<AlgorithmType>> unbenchmarkedAlgorithmsPerDevice = new Dictionary<string, List<AlgorithmType>>();
            // init unbenchmarkedAlgorithmsPerDevice
            foreach (var deviceName in enabledDevicesNames) {
                unbenchmarkedAlgorithmsPerDevice.Add(deviceName, new List<AlgorithmType>());
            }
            // check benchmarks
            foreach (var deviceName in enabledDevicesNames) {
                foreach (var benchmarkConfigPair in _benchmarkConfigs) {
                    foreach (var kvpAlgorithm in benchmarkConfigPair.Value.AlgorithmSettings) {
                        var algorithm = kvpAlgorithm.Value;
                        if (!algorithm.Skip && algorithm.BenchmarkSpeed <= 0.0d) {
                            isEnabledBenchmarksInitialized = false;
                            // add for reference to bench
                            unbenchmarkedAlgorithmsPerDevice[deviceName].Add(algorithm.NiceHashID);
                        }
                    }
                }
            }

            return
                new Tuple<bool,Dictionary<string,List<AlgorithmType>>>(
                    isEnabledBenchmarksInitialized,
                    unbenchmarkedAlgorithmsPerDevice
                );
        }

    }
}
