using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums {
    public enum MinerType {
        NONE,
        ccminer,
        ccminer_CryptoNight,
        ethminer_OCL,
        ethminer_CUDA,
        sgminer,
        cpuminer_opt,
        nheqminer_CPU,
        nheqminer_CUDA,
        nheqminer_AMD,
        eqm_CPU,
        eqm_CUDA,
        ClaymoreZcash,
        ClaymoreCryptoNight,
        OptiminerZcash,
        excavator,
        ClaymoreDual,
        END
    }
}
