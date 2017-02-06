using NiceHashMiner.Configs.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs.ConfigJsonFile {
    public class DeviceBenchmarkConfigFile : ConfigFile<DeviceBenchmarkConfig> {
        const string BENCHMARK_PREFIX = "benchmark_";

        private static string GetName(string DeviceUUID, string old = "") {
            // make device name
            char[] invalid = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            string fileName = BENCHMARK_PREFIX + DeviceUUID.Replace(' ', '_');
            foreach (var c in invalid) {
                fileName = fileName.Replace(c.ToString(), String.Empty);
            }
            const string extension = ".json";
            return fileName + old + extension;
        }

        public DeviceBenchmarkConfigFile(string DeviceUUID)
            : base(FOLDERS.CONFIG, GetName(DeviceUUID), GetName(DeviceUUID, "_OLD")) {
        }

    }
}
