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

        // constructor has references don't delete it
        protected DeviceBenchmarkConfigManager() {
            _benchmarkConfigs = new Dictionary<string, DeviceBenchmarkConfig>();
        }

        private DeviceBenchmarkConfig GetConfig(string deviceUUID) {
            DeviceBenchmarkConfig retConfig = null;
            if (_benchmarkConfigs.TryGetValue(deviceUUID, out retConfig) == false) {
                retConfig = null;
            }
            return retConfig;
        }

        public DeviceBenchmarkConfig GetConfig(DeviceGroupType deviceGroupType,
            string deviceUUID, string deviceName) {
                DeviceBenchmarkConfig retConfig = GetConfig(deviceUUID);
            if (retConfig == null) {
                retConfig = new DeviceBenchmarkConfig(deviceGroupType, deviceUUID, deviceName);
                _benchmarkConfigs.Add(deviceUUID, retConfig);
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
                    enabledDevicesNames.Add(device.UUID);
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
                if (_benchmarkConfigs.ContainsKey(deviceName)) {
                    foreach (var kvpAlgorithm in _benchmarkConfigs[deviceName].AlgorithmSettings) {
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
