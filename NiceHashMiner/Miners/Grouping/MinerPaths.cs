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

        public static class Data {
            // root binary folder
            private const string _bin = @"bin";
            /// <summary>
            /// ccminers
            /// </summary>
            public const string ccminer_decred = _bin + @"\ccminer_decred\ccminer.exe";
            public const string ccminer_nanashi = _bin + @"\ccminer_nanashi\ccminer.exe";
            public const string ccminer_neoscrypt = _bin + @"\ccminer_neoscrypt\ccminer.exe";
            public const string ccminer_sp = _bin + @"\ccminer_sp\ccminer.exe";
            public const string ccminer_tpruvot = _bin + @"\ccminer_tpruvot\ccminer.exe";
            public const string ccminer_cryptonight = _bin + @"\ccminer_cryptonight\ccminer.exe";
            public const string ccminer_x11gost = _bin + @"\ccminer_x11gost\ccminer.exe";

            /// <summary>
            /// ethminers
            /// </summary>
            public const string ethminer = _bin + @"\ethminer\ethminer.exe";

            /// <summary>
            /// sgminers
            /// </summary>
            public const string sgminer_5_6_0_general = _bin + @"\sgminer-5-6-0-general\sgminer.exe";
            public const string sgminer_gm = _bin + @"\sgminer-gm\sgminer.exe";

            public const string nheqminer = _bin + @"\nheqminer_v0.4b\nheqminer.exe";
            public const string excavator = _bin + @"\excavator\excavator.exe";

            public const string XmrStackCPUMiner = _bin + @"\xmr-stak-cpu\xmr-stak-cpu.exe";

            public const string NONE = "";

            // root binary folder
            private const string _bin_3rdparty = @"bin_3rdparty";
            public const string ClaymoreZcashMiner = _bin_3rdparty + @"\claymore_zcash\ZecMiner64.exe";
            public const string ClaymoreCryptoNightMiner = _bin_3rdparty + @"\claymore_cryptonight\NsGpuCNMiner.exe";
            public const string OptiminerZcashMiner = _bin_3rdparty + @"\optiminer_zcash_win\Optiminer.exe";
            public const string ClaymoreDual = _bin_3rdparty + @"\claymore_dual\EthDcrMiner64.exe";
        }

        // NEW START
        ////////////////////////////////////////////
        // Pure functions
        //public static bool IsMinerAlgorithmAvaliable(List<Algorithm> algos, MinerBaseType minerBaseType, AlgorithmType algorithmType) {
        //    return algos.FindIndex((a) => a.MinerBaseType == minerBaseType && a.NiceHashID == algorithmType) > -1;
        //}

        public static string GetPathFor(MinerBaseType minerBaseType, AlgorithmType algoType, DeviceGroupType devGroupType) {
            switch (minerBaseType) {
                case MinerBaseType.ccminer:
                    return NVIDIA_GROUPS.ccminer_path(algoType, devGroupType);
                case MinerBaseType.sgminer:
                    return AMD_GROUP.sgminer_path(algoType);
                case MinerBaseType.nheqminer:
                    return Data.nheqminer;
                case MinerBaseType.ethminer:
                    return Data.ethminer;
                case MinerBaseType.ClaymoreAMD:
                    return AMD_GROUP.ClaymorePath(algoType);
                case MinerBaseType.OptiminerAMD:
                    return Data.OptiminerZcashMiner;
                case MinerBaseType.excavator:
                    return Data.excavator;
                case MinerBaseType.XmrStackCPU:
                    return Data.XmrStackCPUMiner;
                case MinerBaseType.ccminer_alexis:
                    return NVIDIA_GROUPS.ccminer_unstable_path(algoType, devGroupType);
                case MinerBaseType.experimental:
                    return EXPERIMENTAL.GetPath(algoType, devGroupType);
            }
            return Data.NONE;
        }

        public static string GetPathFor(ComputeDevice computeDevice, Algorithm algorithm /*, Options: MinerPathsConfig*/) {
            if (computeDevice == null || algorithm == null) {
                return Data.NONE;
            }

            return GetPathFor(
                algorithm.MinerBaseType,
                algorithm.NiceHashID,
                computeDevice.DeviceGroupType
                );
        }

        public static bool IsValidMinerPath(string minerPath) {
            // TODO make a list of valid miner paths and check that instead
            return minerPath != null && Data.NONE != minerPath && minerPath != ""; 
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
                    return Data.ccminer_decred;
                }
                if (AlgorithmType.CryptoNight == algorithmType) {
                    return Data.ccminer_cryptonight;
                }
                return Data.ccminer_tpruvot;
            }

            public static string ccminer_sm5x_or_sm6x(AlgorithmType algorithmType) {
                if (AlgorithmType.Decred == algorithmType) {
                    return Data.ccminer_decred;
                }
                if (AlgorithmType.NeoScrypt == algorithmType) {
                    return Data.ccminer_neoscrypt;
                }
                if (AlgorithmType.Lyra2RE == algorithmType || AlgorithmType.Lyra2REv2 == algorithmType) {
                    return Data.ccminer_nanashi;
                }
                if (AlgorithmType.CryptoNight == algorithmType) {
                    return Data.ccminer_cryptonight;
                }
                if (AlgorithmType.Lbry == algorithmType || AlgorithmType.X11Gost == algorithmType) {
                    return Data.ccminer_tpruvot;
                }

                return Data.ccminer_sp;
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
                return Data.NONE; // should not happen
            }

            public static string ccminer_unstable_path(AlgorithmType algorithmType, DeviceGroupType nvidiaGroup) {
                // sm5x and sm6x have same settings
                if (nvidiaGroup == DeviceGroupType.NVIDIA_5_x || nvidiaGroup == DeviceGroupType.NVIDIA_6_x) {
                    if (AlgorithmType.X11Gost == algorithmType) {
                        return Data.ccminer_x11gost;
                    }
                }
                // TODO wrong case?
                return Data.NONE; // should not happen
            }
        }

        static class AMD_GROUP {
            public static string sgminer_path(AlgorithmType type) {
                if (AlgorithmType.CryptoNight == type || AlgorithmType.DaggerHashimoto == type) {
                    return Data.sgminer_gm;
                }
                return Data.sgminer_5_6_0_general;
            }

            public static string ClaymorePath(AlgorithmType type) {
                if(AlgorithmType.Equihash == type) {
                    return Data.ClaymoreZcashMiner;
                } else if(AlgorithmType.CryptoNight == type) {
                    return Data.ClaymoreCryptoNightMiner;
                } else if (AlgorithmType.DaggerHashimoto == type) {
                    return Data.ClaymoreDual;
                }
                return Data.NONE; // should not happen
            }
        }

        // unstable miners, NVIDIA for now
        static class EXPERIMENTAL {
            public static string GetPath(AlgorithmType algoType, DeviceGroupType devGroupType) {
                if (devGroupType == DeviceGroupType.NVIDIA_6_x) {
                    return NVIDIA_GROUPS.ccminer_path(algoType, devGroupType);
                }
                return Data.NONE; // should not happen
            }
        }
    }
}
