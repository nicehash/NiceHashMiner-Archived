using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    using NiceHashMiner.Enums;
    using NiceHashMiner.Devices;
    using NiceHashMiner.Configs;
    // typedefs
    using GroupedDevices = SortedSet<string>;
    using GroupedDevicesKey = SortedSet<SortedSet<string>>;

    public partial class MinersManager {
        private class CpuGroupMiner : GroupMiners {

            public CpuGroupMiner(GroupedDevices deviceUUIDSet, cpuminer miner)
                : base(deviceUUIDSet) {
                _miners.Add(miner);
            }
        }
    }
}
