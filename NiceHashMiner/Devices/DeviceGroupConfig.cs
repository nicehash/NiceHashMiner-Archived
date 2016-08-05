using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices {
    [Serializable]
    public class DeviceGroupConfig {
        readonly public string Name;
        // TODO create port ranges and use those
        public int APIBindPort { get; set; }
        public string ExtraLaunchParameters { get; set; }
        // TODO will bew removed
        public string UsePassword { get; set; }
        // Divide this only by 3 groups CPU, AMD, NVIDIA
        public double MinimumProfit { get; set; }
        // TODO add intensity per device/algorithm

        public DeviceGroupConfig(string name) {
            Name = name;
        }
    }
}
