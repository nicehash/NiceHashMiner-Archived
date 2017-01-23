using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices {

    /// <summary>
    /// GroupAlgorithms creates defaults supported algorithms. Currently based in Miner implementation
    /// </summary>
    public static class GroupAlgorithms {


        public static Dictionary<AlgorithmType, Algorithm> CreateForDevice(ComputeDevice device) {
            if (device != null) {
                var algoSettings = CreateDefaultsForGroup(device.DeviceGroupType);
                if (algoSettings != null) {
                    if (device.DeviceType == DeviceType.AMD) {
                        // Check for optimized version
                        if (device.IsOptimizedVersion) {
                            if (algoSettings.ContainsKey(AlgorithmType.Qubit)) {
                                algoSettings[AlgorithmType.Qubit].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                            }
                            if (algoSettings.ContainsKey(AlgorithmType.Quark)) {
                                algoSettings[AlgorithmType.Quark].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                            }
                            if (algoSettings.ContainsKey(AlgorithmType.Lyra2REv2)) {
                                algoSettings[AlgorithmType.Lyra2REv2].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 512  --thread-concurrency 0 --worksize 64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                            }
                        } else {
                            // this is not the same as the constructor values?? check!
                            if (algoSettings.ContainsKey(AlgorithmType.Qubit)) {
                                algoSettings[AlgorithmType.Qubit].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 128 --gpu-threads 4" + AmdGpuDevice.TemperatureParam;
                            }
                            if (algoSettings.ContainsKey(AlgorithmType.Quark)) {
                                algoSettings[AlgorithmType.Quark].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 256 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                            }
                            if (algoSettings.ContainsKey(AlgorithmType.Lyra2REv2)) {
                                algoSettings[AlgorithmType.Lyra2REv2].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam;
                            }
                        }
                        if (!device.Codename.Contains("Tahiti")) {
                            if (algoSettings.ContainsKey(AlgorithmType.NeoScrypt)) {
                                algoSettings[AlgorithmType.NeoScrypt].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam;
                                Helpers.ConsolePrint("ComputeDevice", "The GPU detected (" + device.Codename + ") is not Tahiti. Changing default gpu-threads to 2.");
                            }
                        }

                        // Ellesmere, Polaris
                        // Ellesmere sgminer workaround, keep this until sgminer is fixed to work with Ellesmere
                        if ((device.Codename.Contains("Ellesmere") || device.InfSection.ToLower().Contains("polaris")) && Globals.IsEllesmereSgminerIgnore) {
                            // remove all algos except equi and dagger
                            List<AlgorithmType> toRemove = new List<AlgorithmType>();
                            foreach (var key in algoSettings.Keys) {
                                if (AlgorithmType.DaggerHashimoto != key && AlgorithmType.Equihash != key && AlgorithmType.CryptoNight != key) {
                                    toRemove.Add(key);
                                }
                            }
                            // remove keys
                            foreach (var key in toRemove) {
                                algoSettings.Remove(key);
                            }
                        } else if ((device.Codename.Contains("Ellesmere") || device.InfSection.ToLower().Contains("polaris"))) {
                            if (algoSettings.ContainsKey(AlgorithmType.NeoScrypt)) {
                                algoSettings.Remove(AlgorithmType.NeoScrypt);
                            }
                        }

                        // check if 3rd party is enabled and allow 3rd party only algos
                        if (ConfigManager.GeneralConfig.Use3rdPartyMiners == Use3rdPartyMiners.YES && ConfigManager.GeneralConfig.AMD_CryptoNight_ForceSgminer == false) {
                            if (algoSettings.ContainsKey(AlgorithmType.CryptoNight)) {
                                //string regex_a_3 = "[5|6][0-9][0-9][0-9]";
                                List<string> a_4 = new List<string>() {
                                    "270",
                                    "270x",
                                    "280",
                                    "280x",
                                    "290",
                                    "290x",
                                    "370",
                                    "380",
                                    "390",
                                    "470",
                                    "480"};
                                foreach (var namePart in a_4) {
                                    if (device.Name.Contains(namePart)) {
                                        algoSettings[AlgorithmType.CryptoNight].ExtraLaunchParameters = "-a 4";
                                        break;
                                    }
                                }
                            }
                        } else { // we are using sgminer for CryptoNight
                            // set different defaults for Hawaii
                            if (algoSettings.ContainsKey(AlgorithmType.CryptoNight) && device.Name.Contains("Hawaii")) {
                                algoSettings[AlgorithmType.CryptoNight].ExtraLaunchParameters = "--rawintensity 640 -w 8 -g 2";
                            }
                        }

                        // drivers algos issue
                        if (device.DriverDisableAlgos) {
                            List<AlgorithmType> _3rdPartyOnlyAlgos = new List<AlgorithmType>() { AlgorithmType.NeoScrypt, AlgorithmType.Lyra2REv2 };
                            foreach (var algoType in _3rdPartyOnlyAlgos) {
                                if (algoSettings.ContainsKey(algoType)) {
                                    algoSettings.Remove(algoType);
                                }
                            }
                        }
                    } // END AMD case

                    // check if it is Etherum capable
                    if (algoSettings.ContainsKey(AlgorithmType.DaggerHashimoto) && device.IsEtherumCapale == false) {
                        algoSettings.Remove(AlgorithmType.DaggerHashimoto);
                    }
                    
                }
                return algoSettings;
            }
            return null;
        }

        private static Dictionary<AlgorithmType, Algorithm> CreateDefaultsForGroup(DeviceGroupType deviceGroupType) {
            if (DeviceGroupType.CPU == deviceGroupType) {
                return new Dictionary<AlgorithmType, Algorithm>() {
                { AlgorithmType.Lyra2RE, new Algorithm(AlgorithmType.Lyra2RE, "lyra2") },
                { AlgorithmType.Hodl, new Algorithm(AlgorithmType.Hodl, "hodl") },
                { AlgorithmType.CryptoNight, new Algorithm(AlgorithmType.CryptoNight, "cryptonight") },
                { AlgorithmType.Equihash, new Algorithm(AlgorithmType.Equihash, "equihash") }
                };
            }
            if (DeviceGroupType.AMD_OpenCL == deviceGroupType) {
                // DisableAMDTempControl = false; TemperatureParam must be appended lastly
                string DefaultParam = AmdGpuDevice.DefaultParam;
                return new Dictionary<AlgorithmType, Algorithm>() { 
                { AlgorithmType.NeoScrypt , new Algorithm(AlgorithmType.NeoScrypt, "neoscrypt")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 4" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Lyra2REv2 , new Algorithm(AlgorithmType.Lyra2REv2,  "Lyra2REv2")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity  160 --thread-concurrency    0 --worksize  64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.DaggerHashimoto , new Algorithm(AlgorithmType.DaggerHashimoto, "ethash")
                    { ExtraLaunchParameters = "--xintensity 512 -w 192 -g 1" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Decred , new Algorithm(AlgorithmType.Decred, "decred")
                    { ExtraLaunchParameters = "--gpu-threads 1 --remove-disabled --xintensity 256 --lookup-gap 2 --worksize 64" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Lbry, new Algorithm(AlgorithmType.Lbry, "lbry") 
                    { ExtraLaunchParameters = DefaultParam + "--xintensity 512 --worksize 128 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.CryptoNight, new Algorithm(AlgorithmType.CryptoNight, "cryptonight") 
                    { ExtraLaunchParameters = DefaultParam + "--rawintensity 512 -w 4 -g 2" + AmdGpuDevice.TemperatureParam } }, // TODO find best performance
                { AlgorithmType.Equihash, new Algorithm(AlgorithmType.Equihash, "equihash") }
                };
            }
            // NVIDIA
            if (DeviceGroupType.NVIDIA_2_1 == deviceGroupType || DeviceGroupType.NVIDIA_3_x == deviceGroupType || DeviceGroupType.NVIDIA_5_0 == deviceGroupType || DeviceGroupType.NVIDIA_5_2 == deviceGroupType || DeviceGroupType.NVIDIA_6_x == deviceGroupType) {
                var ret = new Dictionary<AlgorithmType, Algorithm> {
                { AlgorithmType.NeoScrypt , new Algorithm(AlgorithmType.NeoScrypt, "neoscrypt") },
                { AlgorithmType.Lyra2REv2 , new Algorithm(AlgorithmType.Lyra2REv2, "lyra2v2") },
                { AlgorithmType.DaggerHashimoto , new Algorithm(AlgorithmType.DaggerHashimoto, "daggerhashimoto") },
                { AlgorithmType.Decred , new Algorithm(AlgorithmType.Decred, "decred") },
                { AlgorithmType.CryptoNight, new Algorithm(AlgorithmType.CryptoNight, "cryptonight") },
                { AlgorithmType.Lbry, new Algorithm(AlgorithmType.Lbry, "lbry") },
                { AlgorithmType.Equihash, new Algorithm(AlgorithmType.Equihash, "equihash") }
                };
                if(DeviceGroupType.NVIDIA_2_1 == deviceGroupType || DeviceGroupType.NVIDIA_3_x == deviceGroupType) {
                    // minerName change => "whirlpoolx" => "whirlpool"
                    if (ret.ContainsKey(AlgorithmType.WhirlpoolX)) {
                        ret[AlgorithmType.WhirlpoolX] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpool");     // Needed for new tpruvot's ccminer
                    }
                    // disable/remove neoscrypt
                    if (ret.ContainsKey(AlgorithmType.NeoScrypt)) {
                        ret.Remove(AlgorithmType.NeoScrypt);
                    }
                    if (ret.ContainsKey(AlgorithmType.Lyra2RE)) {
                        ret.Remove(AlgorithmType.Lyra2RE);
                    }
                    if (ret.ContainsKey(AlgorithmType.Lyra2REv2)) {
                        ret.Remove(AlgorithmType.Lyra2REv2);
                    }
                }
                if (DeviceGroupType.NVIDIA_2_1 == deviceGroupType) {
                    // disable/remove daggerhashimoto
                    if (ret.ContainsKey(AlgorithmType.DaggerHashimoto)) {
                        ret.Remove(AlgorithmType.DaggerHashimoto);
                    }
                    if (ret.ContainsKey(AlgorithmType.CryptoNight)) {
                        ret.Remove(AlgorithmType.CryptoNight);
                    }
                }
                return ret;
            }

            return null;
        }

        private static bool _IsAlgosInGroupInit = false;
        private static Dictionary<DeviceGroupType, List<AlgorithmType>> _algosInGroup = new Dictionary<DeviceGroupType, List<AlgorithmType>>();
        private static void InitAlgosInGroup() {
            for (DeviceGroupType type = DeviceGroupType.CPU; type < DeviceGroupType.LAST; ++type) {
                var supportedList = GetAlgorithmKeysForGroup(type);
                if(supportedList != null) {
                    _algosInGroup[type] = supportedList;
                }
            }
            _IsAlgosInGroupInit = true;
        }

        public static bool IsAlgorithmSupportedForGroup(AlgorithmType algorithmType, DeviceGroupType deviceGroupType) {
            // lazy init
            if (_IsAlgosInGroupInit == false) {
                InitAlgosInGroup();
            }
            if (_algosInGroup.ContainsKey(deviceGroupType)) {
                return _algosInGroup[deviceGroupType].Contains(algorithmType);
            }
            return false;
        }

        private static List<AlgorithmType> GetAlgorithmKeysForGroup(DeviceGroupType deviceGroupType) {
            var ret = CreateDefaultsForGroup(deviceGroupType);
            if (ret != null) {
                return new List <AlgorithmType>(ret.Keys);
            }
            return null;
        }

    }
}
