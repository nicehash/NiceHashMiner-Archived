using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Enums {
    public enum MinerOptionType {
        NONE,
        // ccminer, sgminer
        Intensity,
        // sgminer
        KeccakUnroll,
        HamsiExpandBig,
        Nfactor,
        Xintensity,
        Rawintensity,
        ThreadConcurrency,
        Worksize,
        GpuThreads,
        LookupGap,
        RemoveDisabled,
        // sgminer temp
        GpuFan,
        TempCutoff,
        TempOverheat,
        TempTarget,
        AutoFan,
        AutoGpu,
        // ccminer cryptonight
        Ccminer_CryptoNightLaunch,
        Ccminer_CryptoNightBfactor,
        Ccminer_CryptoNightBsleep,
        // OCL ethminer
        Ethminer_OCL_LocalWork,
        Ethminer_OCL_GlobalWork,
        // CUDA ethminer
        CudaBlockSize,
        CudaGridSize,
        // TODO
        // cpuminer
        Threads,
        CpuAffinity,
        CpuPriority
    }
}
