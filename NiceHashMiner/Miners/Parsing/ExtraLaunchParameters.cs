using NiceHashMiner.Configs.ConfigJsonFile;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Parsing {
    class MinerOptionPackageFile : ConfigFile<MinerOptionPackage> {
        public MinerOptionPackageFile(string name) : base(FOLDERS.INTERNALS, String.Format("{0}.json", name), String.Format("{0}.json", name)) {
        }
    }
    public static class ExtraLaunchParameters {

        private static readonly List<MinerOptionPackage> DEFAULTS = new List<MinerOptionPackage>() {
            new MinerOptionPackage(
                MinerType.ccminer,
                new List<MinerOption>() {
                    new MinerOption("Intensity", "-i", "--intensity=", "0", MinerOptionFlagType.MultiParam, ",")
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.ccminer_CryptoNight,
                new List<MinerOption>() {
                    new MinerOption("Launch", "-l", "--launch=", "8x40", MinerOptionFlagType.MultiParam, ","), // default is 8x40
                    new MinerOption("Bfactor", "", "--bfactor=", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("Bsleep", "", "--bsleep=", "0", MinerOptionFlagType.MultiParam, ",") // TODO check default
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.ethminer_OCL,
                new List<MinerOption>() {
                    new MinerOption("LocalWork", "", "--cl-local-work", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("GlobalWork", "", "--cl-global-work", "0", MinerOptionFlagType.MultiParam, ","),
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.ethminer_CUDA,
                new List<MinerOption>() {
                    new MinerOption("CudaBlockSize", "", "--cuda-block-size", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("CudaGridSize", "", "--cuda-grid-size", "0", MinerOptionFlagType.MultiParam, ","),
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.sgminer,
                new List<MinerOption>() {
                    // SingleParam
                    new MinerOption("KeccakUnroll", "", "--keccak-unroll", "0", MinerOptionFlagType.SingleParam, ""),
                    new MinerOption("HamsiExpandBig", "", "--hamsi-expand-big", "4", MinerOptionFlagType.SingleParam, ""),
                    new MinerOption("Nfactor", "", "--nfactor", "10", MinerOptionFlagType.SingleParam, ""),
                    // MultiParam TODO IMPORTANT check defaults
                    new MinerOption("Intensity", "-I", "--intensity", "d", MinerOptionFlagType.MultiParam, ","), // default is "d" check if -1 works
                    new MinerOption("Xintensity", "-X", "--xintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("Rawintensity", "", "--rawintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("ThreadConcurrency", "", "--thread-concurrency", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("Worksize", "-w", "--worksize", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("GpuThreads", "-g", "--gpu-threads", "1", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("LookupGap", "", "--lookup-gap", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    // Uni
                },
                // TemperatureOptions
                new List<MinerOption>() {
                    new MinerOption("GpuFan", "", "--gpu-fan", "30-60", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("TempCutoff", "", "--temp-cutoff", "95", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("TempOverheat", "", "--temp-overheat", "85", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("TempTarget", "", "--temp-target", "75", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("AutoFan", "", "--auto-fan", null, MinerOptionFlagType.Uni, ""),
                    new MinerOption("AutoGpu", "", "--auto-gpu", null, MinerOptionFlagType.Uni, "")
                }
            ),
            new MinerOptionPackage(
                MinerType.cpuminer_opt,
                new List<MinerOption>() {
                    new MinerOption("Threads", "-t", "--threads=", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("CpuAffinity", "", "--cpu-affinity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("CpuPriority", "", "--cpu-priority", "-1", MinerOptionFlagType.MultiParam, ",") // default 
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.nheqminer_CPU,
                new List<MinerOption>() {
                    new MinerOption("Threads", "-t", "-t", "-1", MinerOptionFlagType.SingleParam, " "), // default none
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.nheqminer_CUDA,
                new List<MinerOption>() {
                    new MinerOption("Solver_Version", "-cv", "-cv", "0", MinerOptionFlagType.SingleParam, " "), // default 0
                    new MinerOption("Solver_Block", "-cb", "-cb", "0", MinerOptionFlagType.MultiParam, " "), // default 0
                    new MinerOption("Solver_Thread", "-ct", "-ct", "0", MinerOptionFlagType.MultiParam, " "), // default 0
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.nheqminer_AMD,
                new List<MinerOption>() {
                    new MinerOption("Solver_Version", "-ov", "-ov", "0", MinerOptionFlagType.SingleParam, " "), // default none
                    new MinerOption("Solver_Dev_Thread", "-ot", "-ot", "1", MinerOptionFlagType.MultiParam, " "), // default none
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.eqm_CPU,
                new List<MinerOption>() {
                    new MinerOption("Threads", "-t", "-t", "-1", MinerOptionFlagType.SingleParam, " ") // default none
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.eqm_CUDA,
                new List<MinerOption>() {
                    new MinerOption("Solver_Mode", "-cm", "-cm", "0", MinerOptionFlagType.MultiParam, " "), // default 0
                },
                new List<MinerOption>()
            ),
            new MinerOptionPackage(
                MinerType.ClaymoreZcash,
                new List<MinerOption>() {
                    new MinerOption("ClaymoreZcash_a"      , "-a", "-a", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_asm"      , "-asm", "-asm", "1", MinerOptionFlagType.MultiParam, ","), 

                    new MinerOption("ClaymoreZcash_i"      , "-i", "-i", "6", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_wd"     , "-wd", "-wd", "1", MinerOptionFlagType.SingleParam, ","),
                    //new MinerOption(ClaymoreZcash_r      , , , , MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_nofee"  , "-nofee", "-nofee", "0", MinerOptionFlagType.SingleParam, ","),
                    new MinerOption("ClaymoreZcash_li"     , "-li", "-li", "0", MinerOptionFlagType.MultiParam, ","),
                
                    //MinerOptionFlagType.MultiParam might not work corectly due to ADL indexing so use single param to apply to all
                    new MinerOption("ClaymoreZcash_cclock" , "-cclock", "-cclock", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_mclock" , "-mclock", "-mclock", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_powlim" , "-powlim", "-powlim", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_cvddc"  , "-cvddc", "-cvddc", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_mvddc"  , "-mvddc", "-mvddc", "0", MinerOptionFlagType.MultiParam, ","),
                },
                new List<MinerOption>() {
                    // temperature stuff
                    //MinerOptionFlagType.MultiParam might not work corectly due to ADL indexing so use single param to apply to all
                    new MinerOption("ClaymoreZcash_tt"     , "-tt", "-tt", "1", MinerOptionFlagType.SingleParam, ","), 
                    new MinerOption("ClaymoreZcash_ttli"   , "-ttli", "-ttli", "70", MinerOptionFlagType.SingleParam, ","),
                    new MinerOption("ClaymoreZcash_tstop"  , "-tstop", "-tstop", "0", MinerOptionFlagType.SingleParam, ","),
                    new MinerOption("ClaymoreZcash_fanmax" , "-fanmax", "-fanmax", "100", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreZcash_fanmin" , "-fanmin", "-fanmin", "0", MinerOptionFlagType.MultiParam, ","),
                }
            ),
            new MinerOptionPackage(
                MinerType.ccminer_CryptoNight,
                new List<MinerOption>() {
                    new MinerOption("ClaymoreCryptoNight_a"      , "-a", "-a", "0", MinerOptionFlagType.MultiParam, ""),
                    new MinerOption("ClaymoreCryptoNight_wd"     , "-wd", "-wd", "1", MinerOptionFlagType.SingleParam, ","),
                    //new MinerOption(ClaymoreCryptoNight_r      , , , , MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_nofee"  , "-nofee", "-nofee", "0", MinerOptionFlagType.SingleParam, ","),
                    new MinerOption("ClaymoreCryptoNight_li"     , "-li", "-li", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_h"     , "-h", "-h", "0", MinerOptionFlagType.MultiParam, ","),
                
                    new MinerOption("ClaymoreCryptoNight_cclock" , "-cclock", "-cclock", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_mclock" , "-mclock", "-mclock", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_powlim" , "-powlim", "-powlim", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_cvddc"  , "-cvddc", "-cvddc", "0", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_mvddc"  , "-mvddc", "-mvddc", "0", MinerOptionFlagType.MultiParam, ","),
                },
                new List<MinerOption>() {
                    // temperature stuff
                    //MinerOptionFlagType.MultiParam might not work corectly due to ADL indexing so use single param to apply to all
                    new MinerOption("ClaymoreCryptoNight_tt"     , "-tt", "-tt", "1", MinerOptionFlagType.SingleParam, ","), 
                    new MinerOption("ClaymoreCryptoNight_tstop"  , "-tstop", "-tstop", "0", MinerOptionFlagType.SingleParam, ","),
                    new MinerOption("ClaymoreCryptoNight_fanmax" , "-fanmax", "-fanmax", "100", MinerOptionFlagType.MultiParam, ","),
                    new MinerOption("ClaymoreCryptoNight_fanmin" , "-fanmin", "-fanmin", "0", MinerOptionFlagType.MultiParam, ","),
                }
            ),
            new MinerOptionPackage(
                MinerType.OptiminerZcash,
                new List<MinerOption>() {
                    new MinerOption("ForceGenericKernel"      , "", "--force-generic-kernel", null, MinerOptionFlagType.Uni, ""),
                    new MinerOption("ExperimentalKernel"      , "", "--experimental-kernel", null, MinerOptionFlagType.Uni, ""),
                    new MinerOption("nodevfee"                , "", "--nodevfee", null, MinerOptionFlagType.Uni, ""),
                    new MinerOption("Intensity"               , "-i", "--intensity", "0", MinerOptionFlagType.DuplicateMultiParam, ""),
                },
                new List<MinerOption>()
            ),
        };

        private static List<MinerOptionPackage> MinerOptionPackages = new List<MinerOptionPackage>();

        public static void InitializePackages() {
            foreach (var pack in DEFAULTS) {
                var packageName = String.Format("MinerOptionPackage_{0}", pack.Name);
                var packageFile = new MinerOptionPackageFile(packageName);
                var readPack = packageFile.ReadFile();
                if (readPack == null) { // read has failed
                    Helpers.ConsolePrint("ExtraLaunchParameters", "Creating internal params config " + packageName);
                    MinerOptionPackages.Add(pack);
                    // create defaults
                    packageFile.Commit(pack);
                } else {
                    Helpers.ConsolePrint("ExtraLaunchParameters", "Loading internal params config " + packageName);
                    MinerOptionPackages.Add(readPack);
                }
            }
        }

        public static MinerOptionPackage GetMinerOptionPackageForMinerType(MinerType type) {
            int index = MinerOptionPackages.FindIndex((p) => p.Type == type);
            if(index > -1) {
                return MinerOptionPackages[index];
            }
            // if none found
            return new MinerOptionPackage(MinerType.NONE, new List<MinerOption>(), new List<MinerOption>());
        }

    }
}
