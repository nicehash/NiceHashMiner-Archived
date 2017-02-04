using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Grouping
{
    /// <summary>
    /// MinerPaths, used just to store miners paths strings. Only one instance needed
    /// </summary>
    public static class MinerPaths
    {
        // root binary folder
        private const string _bin = @"bin";
        /// <summary>
        /// ccminers
        /// </summary>
        public const string ccminer_decred =            _bin + @"\ccminer_decred.exe";
        public const string ccminer_nanashi =           _bin + @"\ccminer_nanashi.exe";
        public const string ccminer_neoscrypt =         _bin + @"\ccminer_neoscrypt.exe";
        public const string ccminer_sp =                _bin + @"\ccminer_sp.exe";
        public const string ccminer_tpruvot =           _bin + @"\ccminer_tpruvot.exe";
        public const string ccminer_cryptonight =       _bin + @"\ccminer_cryptonight.exe";
        /// <summary>
        /// cpuminers opt new
        /// </summary>
        public const string cpuminer_opt_AVX2_AES =     _bin + @"\cpuminer_opt_AVX2_AES.exe";    
        public const string cpuminer_opt_AVX2 =         _bin + @"\cpuminer_opt_AVX2.exe";
        public const string cpuminer_opt_AVX_AES =      _bin + @"\cpuminer_opt_AVX_AES.exe";    
        public const string cpuminer_opt_AVX =          _bin + @"\cpuminer_opt_AVX.exe";
        public const string cpuminer_opt_AES =          _bin + @"\cpuminer_opt_AES.exe";
        public const string cpuminer_opt_SSE2 =         _bin + @"\cpuminer_opt_SSE2.exe";
        /// <summary>
        /// ethminers
        /// </summary>
        public const string ethminer = _bin + @"\ethminer.exe";
                
        /// <summary>
        /// sgminers
        /// </summary>
        public const string sgminer_5_1_0_optimized =   _bin + @"\sgminer-5-1-0-optimized\sgminer.exe";
        //public const string sgminer_5_1_1_optimized =   _bin + @"\sgminer-5-1-1-optimized\sgminer.exe";
        public const string sgminer_5_6_0_general =     _bin + @"\sgminer-5-6-0-general\sgminer.exe";
        public const string sgminer_5_4_0_tweaked =     _bin + @"\sgminer-5-4-0-tweaked\sgminer.exe";
        public const string sgminer_gm =                _bin + @"\sgminer-gm\sgminer.exe";

        /// <summary>
        /// nheqminer
        /// </summary>
        public const string nheqminer = _bin + @"\nheqminer_v0.4b\nheqminer.exe";

        /// <summary>
        /// eqm
        /// </summary>
        public const string eqm = _bin + @"\eqm\eqm.exe";
        public const string excavator = _bin + @"\excavator\excavator.exe";

        public const string NONE = "";

        // root binary folder
        private const string _bin_3rdparty = @"bin_3rdparty";
        public const string ClaymoreZcashMiner = _bin_3rdparty + @"\claymore_zcash\ZecMiner64.exe";
        public const string ClaymoreCryptoNightMiner = _bin_3rdparty + @"\claymore_cryptonight\NsGpuCNMiner.exe";
        public const string OptiminerZcashMiner = _bin_3rdparty + @"\optiminer_zcash_win\Optiminer.exe";

        // NEW START
        ////////////////////////////////////////////
        // Pure functions
        public static bool IsMinerAlgorithmAvaliable(List<Algorithm> algos, MinerBaseType minerBaseType, AlgorithmType algorithmType) {
            return algos.FindIndex((a) => a.MinerBaseType == minerBaseType && a.NiceHashID == algorithmType) > -1;
        }

        public static string GetPathFor(MinerBaseType minerBaseType, AlgorithmType algoType, DeviceGroupType devGroupType, string devCodenameAMD, bool isOptimizedAMD, CPUExtensionType MostOptimizedCPUExtensionType) {
            switch (minerBaseType) {
                case MinerBaseType.cpuminer:
                    return CPU_GROUP.cpu_miner_opt(MostOptimizedCPUExtensionType);
                case MinerBaseType.ccminer:
                    return NVIDIA_GROUPS.ccminer_path(algoType, devGroupType);
                case MinerBaseType.sgminer:
                    return AMD_GROUP.sgminer_path(algoType, devCodenameAMD, isOptimizedAMD);
                case MinerBaseType.nheqminer:
                    return MinerPaths.nheqminer;
                case MinerBaseType.eqm:
                    return MinerPaths.eqm;
                case MinerBaseType.ethminer:
                    return MinerPaths.ethminer;
                case MinerBaseType.ClaymoreAMD:
                    return AMD_GROUP.ClaymorePath(algoType);
                case MinerBaseType.OptiminerAMD:
                    return MinerPaths.OptiminerZcashMiner;
                case MinerBaseType.excavator:
                    return MinerPaths.excavator;
            }
            return MinerPaths.NONE;
        }

        public static string GetPathFor(ComputeDevice computeDevice, Algorithm algorithm /*, Options: MinerPathsConfig*/) {
            if (computeDevice == null || algorithm == null) {
                return MinerPaths.NONE;
            }

            return GetPathFor(
                algorithm.MinerBaseType,
                algorithm.NiceHashID,
                computeDevice.DeviceGroupType,
                computeDevice.Codename,
                computeDevice.IsOptimizedVersion,
                CPUUtils.GetMostOptimized()
                );
        }

        public static bool IsValidMinerPath(string minerPath) {
            // TODO make a list of valid miner paths and check that instead
            return minerPath != null && MinerPaths.NONE != minerPath && minerPath != ""; 
        }

        /**
         * InitAlgorithmsMinerPaths gets and sets miner paths
         */
        public static List<Algorithm> GetAndInitAlgorithmsMinerPaths(List<Algorithm> algos, ComputeDevice computeDevice/*, Options: MinerPathsConfig*/) {
            var retAlgos = algos.FindAll((a) => a != null).ConvertAll((a) => {
                a.MinerBinaryPath = GetPathFor(computeDevice, a/*, Options*/);
                return a;
            });

            return retAlgos;
        }
        // NEW END

        ////// private stuff from here on
        static class NVIDIA_GROUPS {
            public static string ccminer_sm21_or_sm3x(AlgorithmType algorithmType) {
                if (AlgorithmType.Decred == algorithmType) {
                    return MinerPaths.ccminer_decred;
                }
                if (AlgorithmType.CryptoNight == algorithmType) {
                    return MinerPaths.ccminer_cryptonight;
                }
                return MinerPaths.ccminer_tpruvot;
            }

            public static string ccminer_sm5x_or_sm6x(AlgorithmType algorithmType) {
                if (AlgorithmType.Decred == algorithmType) {
                    return MinerPaths.ccminer_decred;
                }
                if (AlgorithmType.NeoScrypt == algorithmType) {
                    return MinerPaths.ccminer_neoscrypt;
                }
                if (AlgorithmType.Lyra2RE == algorithmType || AlgorithmType.Lyra2REv2 == algorithmType) {
                    return MinerPaths.ccminer_nanashi;
                }
                if (AlgorithmType.CryptoNight == algorithmType) {
                    return MinerPaths.ccminer_cryptonight;
                }
                if (AlgorithmType.Lbry == algorithmType) {
                    return MinerPaths.ccminer_tpruvot;
                }

                return MinerPaths.ccminer_sp;
            }
            public static string ccminer_path(AlgorithmType algorithmType, DeviceGroupType nvidiaGroup) {
                // sm21 and sm3x have same settings
                if (nvidiaGroup == DeviceGroupType.NVIDIA_2_1 || nvidiaGroup == DeviceGroupType.NVIDIA_3_x) {
                    return NVIDIA_GROUPS.ccminer_sm21_or_sm3x(algorithmType);
                }
                // sm5x and sm6x have same settings
                if (nvidiaGroup == DeviceGroupType.NVIDIA_5_x || nvidiaGroup == DeviceGroupType.NVIDIA_6_x) {
                    return NVIDIA_GROUPS.ccminer_sm5x_or_sm6x(algorithmType);
                }
                // TODO wrong case?
                return MinerPaths.NONE; // should not happen
            }
        }

        static class AMD_GROUP {
            public static string sgminer_path(AlgorithmType type, string gpuCodename, bool isOptimized) {
                if (AlgorithmType.CryptoNight == type || AlgorithmType.DaggerHashimoto == type) {
                    return MinerPaths.sgminer_gm;
                }
                if (isOptimized) {
                    if (AlgorithmType.Lyra2REv2 == type) {
                        if (!(gpuCodename.Contains("Hawaii") || gpuCodename.Contains("Pitcairn") || gpuCodename.Contains("Tahiti"))) {
                            if (!Helpers.InternalCheckIsWow64())
                                return MinerPaths.sgminer_5_6_0_general;

                            return MinerPaths.sgminer_5_4_0_tweaked;
                        }
                        return MinerPaths.sgminer_5_1_0_optimized;
                    }
                }

                return MinerPaths.sgminer_5_6_0_general;
            }

            public static string ClaymorePath(AlgorithmType type) {
                if(AlgorithmType.Equihash == type) {
                    return MinerPaths.ClaymoreZcashMiner;
                } else if(AlgorithmType.CryptoNight == type) {
                    return MinerPaths.ClaymoreCryptoNightMiner;
                }
                return MinerPaths.NONE; // should not happen
            }
        }

        static class CPU_GROUP {
            public static string cpu_miner_opt(CPUExtensionType type) {
                // algorithmType is not needed ATM
                // algorithmType: AlgorithmType
                if (CPUExtensionType.AVX2_AES == type) { return MinerPaths.cpuminer_opt_AVX2_AES; }
                if (CPUExtensionType.AVX2 == type) { return MinerPaths.cpuminer_opt_AVX2; }
                if (CPUExtensionType.AVX_AES == type) { return MinerPaths.cpuminer_opt_AVX_AES; }
                if (CPUExtensionType.AVX == type) { return MinerPaths.cpuminer_opt_AVX; }
                if (CPUExtensionType.AES == type) { return MinerPaths.cpuminer_opt_AES; }
                if (CPUExtensionType.SSE2 == type) { return MinerPaths.cpuminer_opt_SSE2; }

                return NONE; // should not happen
            }
        }

    }
}
