using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Configs
{
    [Serializable]
    public class DeviceBenchmarkConfig : BaseConfigFile<DeviceBenchmarkConfig> {
        public string ID { get; private set; }
        public DeviceGroupType DeviceGroupType { get; private set; }
        public string DeviceName { get; private set; }
        public int[] DevicesIDs { get; private set; }
        // TODO handle defaults for this
        public string ExtraLaunchParameters { get; private set; }
        public int TimeLimit { get; private set; }
        public Dictionary<AlgorithmType, Algorithm> BenchmarkSpeeds { get; set; }
        

        [field: NonSerialized]
        readonly public static string BENCHMARK_PREFIX = "benchmark_";

        public DeviceBenchmarkConfig(DeviceGroupType deviceGroupType,
            string deviceGroupName, int[] devicesIDs,
            Dictionary<AlgorithmType, Algorithm> benchmarkSpeeds = null) {

            DeviceGroupType = deviceGroupType;
            DeviceName = deviceGroupName;
            DevicesIDs = devicesIDs;
            if (benchmarkSpeeds != null) {
                BenchmarkSpeeds = benchmarkSpeeds;
            } else {
                BenchmarkSpeeds = GroupAlgorithms.CreateDefaultsForGroup(deviceGroupType);
            }

            // calculate ID
            ID = GetId();
        }

        public static string GetId(DeviceGroupType deviceGroupType,
            string deviceGroupName, int[] devicesIDs) {
            var SHA256 = new SHA256Managed();
            var hash = new StringBuilder();
            string mixedAttr = ((int)deviceGroupType).ToString() + deviceGroupName;
            foreach (var devId in devicesIDs) {
                mixedAttr += devId.ToString();
            }
            byte[] hashedBytes = SHA256.ComputeHash(Encoding.UTF8.GetBytes(mixedAttr), 0, Encoding.UTF8.GetByteCount(mixedAttr));
            foreach (var b in hashedBytes) {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }

        private string GetId() {
            return GetId(DeviceGroupType, DeviceName, DevicesIDs);
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
        protected override void InitializeObject() { }
    }
}
