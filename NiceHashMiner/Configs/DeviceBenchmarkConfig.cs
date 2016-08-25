using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;
using Newtonsoft.Json;

namespace NiceHashMiner.Configs
{
    [Serializable]
    public class DeviceBenchmarkConfig : BaseConfigFile<DeviceBenchmarkConfig> {
        // TODO remove id if only unique benchmarks enabled
        //public string ID { get; private set; }
        public DeviceGroupType DeviceGroupType { get; private set; }
        public string DeviceName { get; private set; }
        // TODO handle defaults for this
        public string ExtraLaunchParameters { get; set; }
        public int TimeLimit { get; set; }
        public Dictionary<AlgorithmType, Algorithm> AlgorithmSettings { get; set; }

        // TODO add cdev UUIDs???
        List<string> deviceUUIDs;

        [field: NonSerialized]
        readonly public static string BENCHMARK_PREFIX = "benchmark_";

        [JsonIgnore]
        public bool IsAlgorithmSettingsInit { get; set; }

        public DeviceBenchmarkConfig(DeviceGroupType deviceGroupType,
            string deviceGroupName,
            Dictionary<AlgorithmType, Algorithm> benchmarkSpeeds = null) {

            DeviceGroupType = deviceGroupType;
            DeviceName = deviceGroupName;
            if (benchmarkSpeeds != null) {
                AlgorithmSettings = benchmarkSpeeds;
            } else {
                AlgorithmSettings = GroupAlgorithms.CreateDefaultsForGroup(deviceGroupType);
            }

            deviceUUIDs = new List<string>();
            IsAlgorithmSettingsInit = false;

            // calculate ID
            //ID = GetId();
        }

        public static string GetId(DeviceGroupType deviceGroupType,
            string deviceGroupName) {
            var SHA256 = new SHA256Managed();
            var hash = new StringBuilder();
            string mixedAttr = ((int)deviceGroupType).ToString() + deviceGroupName;
            byte[] hashedBytes = SHA256.ComputeHash(Encoding.UTF8.GetBytes(mixedAttr), 0, Encoding.UTF8.GetByteCount(mixedAttr));
            foreach (var b in hashedBytes) {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }

        private string GetId() {
            return GetId(DeviceGroupType, DeviceName);
        }

        protected override void InitializePaths() {
            // make device name
            char[] invalid = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            string fileName = BENCHMARK_PREFIX + DeviceName.Replace(' ', '_');
            foreach (var c in invalid) {
                fileName = fileName.Replace(c.ToString(), String.Empty);
            }
            const string extension = ".json";
            FilePath = fileName + extension;
            FilePathOld = fileName + "_old" + extension;
        }
        protected override void InitializeObject() {
            // check if data tampered
            bool IsDataTampered = !(
                /*this.ID == _file.ID
                &&*/ this.DeviceGroupType == _file.DeviceGroupType
                && this.DeviceName == _file.DeviceName
                );

            // set editable data
            if (_file.ExtraLaunchParameters != null) {
                this.ExtraLaunchParameters = _file.ExtraLaunchParameters;
            }
            this.TimeLimit = _file.TimeLimit;

            if (_file.AlgorithmSettings != null) {
                // settings from files are initialized
                foreach (var key in _file.AlgorithmSettings.Keys) {
                    if(this.AlgorithmSettings.ContainsKey(key)) {
                        this.AlgorithmSettings[key] = _file.AlgorithmSettings[key];
                    } else {
                        // TODO think if we let tamnpered data
                    }
                }
            }
        }
    }
}
