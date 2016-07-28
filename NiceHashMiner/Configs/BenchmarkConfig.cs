using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Configs
{
    public class BenchmarkConfig {
        public string ID { get; private set; }
        public DeviceGroupType DeviceGroupType { get; private set; }
        public string DeviceGroupName { get; private set; }
        public int[] DevicesIDs { get; private set; }
        // TODO handle defaults for this
        public string ExtraLaunchParameters { get; private set; }
        public Dictionary<AlgorithmType, Algorithm> BenchmarkSpeeds { get; set; }
        
        // quality check variables
        public BenchmarkPerformanceType PerformanceType { get; private set; }
        public int TimeLimit { get; private set; }

        public BenchmarkConfig(DeviceGroupType deviceGroupType,
            string deviceGroupName, int[] devicesIDs,
            Dictionary<AlgorithmType, Algorithm> benchmarkSpeeds = null) {

            DeviceGroupType = deviceGroupType;
            DeviceGroupName = deviceGroupName;
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
            return GetId(DeviceGroupType, DeviceGroupName, DevicesIDs);
        }
    }
}
