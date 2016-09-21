using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    class ccminer_sm6x : ccminer_sm5x {
        public ccminer_sm6x() :
            base("NVIDIA6.x")
        { }

    }
}
