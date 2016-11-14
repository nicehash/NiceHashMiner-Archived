using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Enums {
    public enum DeviceType {
        CPU = 0,
        NVIDIA,
        AMD,
        // combined types used for miner identification only
        ALL,
        NVIDIA_CPU
    }
}
