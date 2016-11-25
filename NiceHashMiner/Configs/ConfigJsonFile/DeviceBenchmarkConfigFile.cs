using NiceHashMiner.Configs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //FilePath = fileName + extension;
            //FilePathOld = fileName + "_OLD" + extension;
        }

        public DeviceBenchmarkConfigFile(string DeviceUUID)
            : base(GetName(DeviceUUID), GetName(DeviceUUID, "_OLD")) {
        }

    }
}
