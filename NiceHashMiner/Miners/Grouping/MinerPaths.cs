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
        public const string sgminer_5_1_1_optimized =   _bin + @"\sgminer-5-1-1-optimized\sgminer.exe";
        public const string sgminer_5_5_0_general =     _bin + @"\sgminer-5-5-0-general\sgminer.exe";
        public const string sgminer_5_4_0_tweaked =     _bin + @"\sgminer-5-4-0-tweaked\sgminer.exe";
        //public const string sgminer_gm =                _bin + @"\sgminer-gm\sgminer.exe"; // open source miner

        /// <summary>
        /// nheqminer
        /// </summary>
        public const string nheqminer = _bin + @"\nheqminer_v0.4b\nheqminer.exe";

        /// <summary>
        /// eqm
        /// </summary>
        //public const string eqm_sm50 = _bin + @"\eqm\eqm.exe";
        //public const string eqm_sm52plus = _bin + @"\eqm_sm52plus\eqm.exe";
        public const string eqm = _bin + @"\eqm\eqm.exe";

        public const string NONE = "";

        // root binary folder
        private const string _bin_3rdparty = @"bin_3rdparty";
        public const string ClaymoreZcashMiner = _bin_3rdparty + @"\claymore_zcash\ZecMiner64.exe";
        public const string ClaymoreCryptoNightMiner = _bin_3rdparty + @"\claymore_cryptonight\NsGpuCNMiner.exe";
        public const string OptiminerZcashMiner = _bin_3rdparty + @"\optiminer_zcash_win\Optiminer.exe";

        public static string GetOptimizedMinerPath(MiningPair pair) {
            return GetOptimizedMinerPath(pair.Device, pair.Algorithm);
        }

        public static string GetOptimizedMinerPath(ComputeDevice computeDevice, Algorithm algorithm) {
            if (computeDevice == null || algorithm == null) {
                return NONE;
            }
            AlgorithmType algoType = algorithm.NiceHashID;
            DeviceType devType = computeDevice.DeviceType;
            DeviceGroupType devGroupType = computeDevice.DeviceGroupType;
            string devCodename = computeDevice.Codename;
            bool isOptimized = computeDevice.IsOptimizedVersion;
            return GetOptimizedMinerPath(algoType, devType, devGroupType, devCodename, isOptimized);
        }

        public static string GetOptimizedMinerPath(AlgorithmType algorithmType, DeviceType deviceType, DeviceGroupType deviceGroupType, string devCodename, bool isOptimized) {
            // special cases
            // AlgorithmType.DaggerHashimoto special shared case
            if (algorithmType == AlgorithmType.DaggerHashimoto
                && (deviceType == DeviceType.AMD || deviceType == DeviceType.NVIDIA)) {
                return MinerPaths.ethminer;
            }
            // AlgorithmType.Equihash special shared case
            if (algorithmType == AlgorithmType.Equihash) {
                if (deviceGroupType == DeviceGroupType.NVIDIA_5_0 || deviceGroupType == DeviceGroupType.NVIDIA_5_2 || deviceGroupType == DeviceGroupType.NVIDIA_6_x
                    || (MinersManager.EquihashCPU_USE_eqm() && DeviceGroupType.CPU == deviceGroupType)) {
                    return MinerPaths.eqm;
                } else if(deviceType == DeviceType.AMD && ConfigManager.GeneralConfig.Use3rdPartyMiners == Use3rdPartyMiners.YES) { // TODO remove state
                    // TODO add settings to choose what miner to use, or to automatically determine what to use
                    return MinerPaths.ClaymoreZcashMiner;
                    //return MinerPaths.OptiminerZcashMiner;
                }
                else { // supports all DeviceTypes
                    return MinerPaths.nheqminer;
                }
            }
            // 3rd party miner
            if (algorithmType == AlgorithmType.CryptoNight && deviceType == DeviceType.AMD && ConfigManager.GeneralConfig.Use3rdPartyMiners == Use3rdPartyMiners.YES) {
                return MinerPaths.ClaymoreCryptoNightMiner;
            }
            // normal stuff
            // CPU
            if (deviceType == DeviceType.CPU) {
                return CPU_GROUP.cpu_miner_opt(CPUUtils.GetMostOptimized());
            }
            // NVIDIA
            if (deviceType == DeviceType.NVIDIA) {
                var nvidiaGroup = deviceGroupType;
                // sm21 and sm3x have same settings
                if (nvidiaGroup == DeviceGroupType.NVIDIA_2_1 || nvidiaGroup == DeviceGroupType.NVIDIA_3_x) {
                    return NVIDIA_GROUPS.ccminer_sm21_or_sm3x(algorithmType);
                }
                // sm5x and sm6x have same settings
                if (nvidiaGroup == DeviceGroupType.NVIDIA_5_0 || nvidiaGroup == DeviceGroupType.NVIDIA_5_2 || nvidiaGroup == DeviceGroupType.NVIDIA_6_x) {
                    return NVIDIA_GROUPS.ccminer_sm5x_or_sm6x(algorithmType);
                }
            }
            // AMD
            if (deviceType == DeviceType.AMD) {
                return AMD_GROUP.sgminer_path(algorithmType, devCodename, isOptimized);
            }

            return NONE;
        }

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
        }

        static class AMD_GROUP {
            public static string sgminer_path(AlgorithmType type, string gpuCodename, bool isOptimized) {
                if (isOptimized) {
                    if (AlgorithmType.Quark == type || AlgorithmType.Lyra2REv2 == type || AlgorithmType.Qubit == type) {
                        if (!(gpuCodename.Contains("Hawaii") || gpuCodename.Contains("Pitcairn") || gpuCodename.Contains("Tahiti"))) {
                            if (!Helpers.InternalCheckIsWow64())
                                return MinerPaths.sgminer_5_5_0_general;

                            return MinerPaths.sgminer_5_4_0_tweaked;
                        }
                        if (AlgorithmType.Quark == type || AlgorithmType.Lyra2REv2 == type)
                            return MinerPaths.sgminer_5_1_0_optimized;
                        else
                            return MinerPaths.sgminer_5_1_1_optimized;
                    }
                }
                //if (AlgorithmType.CryptoNight == type /*|| AlgorithmType.DaggerHashimoto == type*/) {
                //    return MinerPaths.sgminer_gm;
                //}

                return MinerPaths.sgminer_5_5_0_general;
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
