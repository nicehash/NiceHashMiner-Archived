using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Enums {
    public enum MinerType : int {
        cpuminer = 0,
        ccminer_sm21,
        ccminer_sm3x,
        ccminer_sm5x,
        MinerEtherumCUDA,
        MinerEtherumOCL,
        sgminer,
    }
}
