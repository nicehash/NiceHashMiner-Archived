using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners
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

        /// <summary>
        /// nheqminer
        /// </summary>
        public const string nheqminer = _bin + @"\nheqminer_v0.4b\nheqminer.exe";

        /// <summary>
        /// eqm
        /// </summary>
        public const string eqm = _bin + @"\eqm\eqm.exe";

        public const string NONE = "";


        public static string GetOptimizedMinerPath(AlgorithmType algorithmType, DeviceType deviceType, DeviceGroupType deviceGroupType, string devCodename, bool isOptimized) {
            // special cases
            // AlgorithmType.DaggerHashimoto special shared case
            if (algorithmType == AlgorithmType.DaggerHashimoto
                && (deviceType == DeviceType.AMD || deviceType == DeviceType.NVIDIA)) {
                return MinerPaths.ethminer;
            }
            // AlgorithmType.Equihash special shared case
            if (algorithmType == AlgorithmType.Equihash) {
                if (deviceType == DeviceType.NVIDIA_CPU || deviceGroupType == DeviceGroupType.NVIDIA_5_x || deviceGroupType == DeviceGroupType.NVIDIA_6_x) {
                    return MinerPaths.eqm;
                } else {
                    return MinerPaths.nheqminer;
                }
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
                if (nvidiaGroup == DeviceGroupType.NVIDIA_5_x || nvidiaGroup == DeviceGroupType.NVIDIA_6_x) {
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
