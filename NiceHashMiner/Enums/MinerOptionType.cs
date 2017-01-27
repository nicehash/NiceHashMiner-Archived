using System;
using System.Collections.Generic;
using System.Text;

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
        CpuPriority,
        // nheqminer/eqm
        // nheqminer CUDA
        CUDA_Solver_Version, // -cb
        CUDA_Solver_Block, // -cb
        CUDA_Solver_Thread, // -ct
        // nheqminer OpenCL
        OpenCL_Solver_Version, //-ov
        OpenCL_Solver_Dev_Thread, // -ot
        // eqm
        CUDA_Solver_Mode, // -cm
        // ClaymoreZcash
        ClaymoreZcash_a,
        ClaymoreZcash_asm,
        ClaymoreZcash_i,  // -i
        ClaymoreZcash_wd, // -wd
        ClaymoreZcash_r,
        ClaymoreZcash_nofee,
        ClaymoreZcash_li,
        ClaymoreZcash_tt, // -tt
        ClaymoreZcash_ttli,
        ClaymoreZcash_tstop,
        ClaymoreZcash_fanmax,
        ClaymoreZcash_fanmin,
        ClaymoreZcash_cclock,
        ClaymoreZcash_mclock,
        ClaymoreZcash_powlim,
        ClaymoreZcash_cvddc,
        ClaymoreZcash_mvddc,
        // ClaymoreCryptoNight
        ClaymoreCryptoNight_a,
        ClaymoreCryptoNight_wd,
        ClaymoreCryptoNight_r,
        ClaymoreCryptoNight_nofee,
        ClaymoreCryptoNight_li,
        ClaymoreCryptoNight_h,
        ClaymoreCryptoNight_tt, // -tt
        ClaymoreCryptoNight_ttli, // not present?
        ClaymoreCryptoNight_tstop,
        ClaymoreCryptoNight_fanmax,
        ClaymoreCryptoNight_fanmin,
        ClaymoreCryptoNight_cclock,
        ClaymoreCryptoNight_mclock,
        ClaymoreCryptoNight_powlim,
        ClaymoreCryptoNight_cvddc,
        ClaymoreCryptoNight_mvddc,
        // Optiminer
        Optiminer_ForceGenericKernel,
        Optiminer_ExperimentalKernel,
        Optiminer_nodevfee,
        Optiminer_i,

    }
}
