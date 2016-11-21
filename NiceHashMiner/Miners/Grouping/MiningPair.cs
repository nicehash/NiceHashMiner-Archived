using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners.Grouping {
    public class MiningPair {
        public readonly ComputeDevice Device;
        public readonly Algorithm Algorithm;
        public string CurrentExtraLaunchParameters;
        public MiningPair(ComputeDevice d, Algorithm a) {
            this.Device = d;
            this.Algorithm = a;
            this.CurrentExtraLaunchParameters = Algorithm.ExtraLaunchParameters;
        }
    }
}
