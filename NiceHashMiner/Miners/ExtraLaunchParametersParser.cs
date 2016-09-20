using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    static class ExtraLaunchParametersParser {
        private static readonly string TAG = "ExtraLaunchParametersParser";

        // Order of miner options tpyes is important make sure to implement it corectly
        // ccminer
        private static List<MinerOption> _ccimerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Intensity, "-i", "--intensity=", "0")
        };
        // ccminer CryptoNight
        private static List<MinerOption> _ccimerCryptoNightOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Ccminer_CryptoNightLaunch, "-l", "--launch=", "8x40"),
            new MinerOption(MinerOptionType.Ccminer_CryptoNightBfactor, "", "--bfactor=", "0"),
            new MinerOption(MinerOptionType.Ccminer_CryptoNightBsleep, "", "--bsleep=", "0") // TODO check default
        };
        // OCL ethminer
        private static List<MinerOption> _oclEthminerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Ethminer_OCL_LocalWork, "", "--cl-local-work", "64"),
            new MinerOption(MinerOptionType.Ethminer_OCL_GlobalWork, "", "--cl-global-work", "262144"),
        };

        // sgminer
        private static List<MinerOption> _sgminerOptions = new List<MinerOption>() {
            // SingleParam
            new MinerOption(MinerOptionType.KeccakUnroll, "", "--keccak-unroll", "0", MinerOptionFlagType.SingleParam),
            new MinerOption(MinerOptionType.HamsiExpandBig, "", "--hamsi-expand-big", "4", MinerOptionFlagType.SingleParam),
            new MinerOption(MinerOptionType.Nfactor, "", "--nfactor", "10", MinerOptionFlagType.SingleParam),
            // MultiParam TODO IMPORTANT check defaults
            new MinerOption(MinerOptionType.Intensity, "-I", "--intensity", "-1"), // default is "d" check if -1 works
            new MinerOption(MinerOptionType.Xintensity, "-X", "--xintensity", "-1"),
            new MinerOption(MinerOptionType.Rawintensity, "", "--rawintensity", "-1"),
            new MinerOption(MinerOptionType.ThreadConcurrency, "", "--thread-concurrency", "-1"),
            new MinerOption(MinerOptionType.Worksize, "-w", "--worksize", "-1"),
            new MinerOption(MinerOptionType.GpuThreads, "-g", "--gpu-threads", "1"),
            new MinerOption(MinerOptionType.LookupGap, "", "--lookup-gap", "-1"),
            // Uni
            new MinerOption(MinerOptionType.RemoveDisabled, "", "--remove-disabled", null, MinerOptionFlagType.Uni),
        };
        private static List<MinerOption> _sgminerTemperatureOptions = new List<MinerOption>() {
            // temperature stuff
            new MinerOption(MinerOptionType.GpuFan, "", "--gpu-fan", "-1"), // default none
            new MinerOption(MinerOptionType.TempCutoff, "", "--temp-cutoff", "95"),
            new MinerOption(MinerOptionType.TempOverheat, "", "--temp-overheat", "85"),
            new MinerOption(MinerOptionType.TempTarget, "", "--temp-target", "75"),
            new MinerOption(MinerOptionType.AutoFan, "", "--auto-fan", null, MinerOptionFlagType.Uni),
            new MinerOption(MinerOptionType.AutoGpu, "", "--auto-gpu", null, MinerOptionFlagType.Uni)
        };

        private static string Parse(List<ComputeDevice> CDevs, List<MinerOption> options, bool useIfDefaults = false) {
            List<MinerOptionType> optionsOrder = new List<MinerOptionType>();
            Dictionary<MinerOptionType, string> paramsFlags = new Dictionary<MinerOptionType, string>();
            Dictionary<string, Dictionary<MinerOptionType, string>> cdevOptions = new Dictionary<string, Dictionary<MinerOptionType, string>>();

            // init devs options, and defaults
            foreach (var cDev in CDevs) {
                var defaults = new Dictionary<MinerOptionType, string>();
                foreach (var option in options) {
                    defaults.Add(option.Type, option.Default);
                }
                cdevOptions.Add(cDev.UUID, defaults);
            }
            // init order and params flags, and params list
            foreach (var option in options) {
                MinerOptionType optionType = option.Type;
                optionsOrder.Add(optionType);
                paramsFlags.Add(optionType, option.LongName);
            }
            // parse
            foreach (var cDev in CDevs) {
                Helpers.ConsolePrint(TAG, String.Format("ExtraLaunch params \"{0}\" for device UUID {1}", cDev.MostProfitableAlgorithm.ExtraLaunchParameters, cDev.UUID));
                var parameters = cDev.MostProfitableAlgorithm.ExtraLaunchParameters.Replace("=", "= ").Split(' ');
                
                MinerOptionType currentFlag = MinerOptionType.NONE;
                foreach (var param in parameters) {
                    if (param.Equals("")) { // skip
                        continue;
                    } else if (currentFlag == MinerOptionType.NONE) {
                        bool isIngored = true;
                        foreach (var option in options) {
                            if (param.Equals(option.ShortName) || param.Equals(option.LongName)) {
                                isIngored = false;
                                if (option.FlagType == MinerOptionFlagType.Uni) {
                                    cdevOptions[cDev.UUID][option.Type] = "notNull"; // if Uni param is null it is not present
                                } else { // Sinlge and Multi param
                                    currentFlag = option.Type;
                                }
                            }
                        }
                        if (isIngored) { // ignored
                            Helpers.ConsolePrint(TAG, String.Format("Cannot parse \"{0}\", not supported or wrong extra launch parameter settings", param));
                        }
                    } else if (currentFlag != MinerOptionType.NONE) {
                        cdevOptions[cDev.UUID][currentFlag] = param;
                        currentFlag = MinerOptionType.NONE;
                    } else { // problem
                        Helpers.ConsolePrint(TAG, String.Format("Cannot parse \"{0}\", not supported or wrong extra launch parameter settings", param));
                    }
                }
            }

            string retVal = "";

            // check if is all defaults
            bool isAllDefault = true;
            foreach (var cDev in CDevs) {
                foreach (var option in options) {
                    if (option.Default != cdevOptions[cDev.UUID][option.Type]) {
                        isAllDefault = false;
                        break;
                    }
                }
                if (!isAllDefault) break;
            }

            if (!isAllDefault || useIfDefaults) {
                foreach (var option in options) {
                    if(option.FlagType == MinerOptionFlagType.Uni) {
                        // uni params if one exist use or all must exist?
                        bool isOptionInUse = false;
                        foreach (var cDev in CDevs) {
                            if (cdevOptions[cDev.UUID][option.Type] != null) {
                                isOptionInUse = true;
                                break;
                            }
                        }
                        if (isOptionInUse) {
                            retVal += String.Format(" {0}", option.LongName);
                        }
                    } else if(option.FlagType == MinerOptionFlagType.MultiParam) {
                        List<string> values = new List<string>();
                        foreach (var cDev in CDevs) {
                            values.Add(cdevOptions[cDev.UUID][option.Type]);
                        }
                        string MASK = " {0} {1}";
                        if(option.LongName.Contains('=')) {
                            MASK = " {0}{1}";
                        }
                        retVal += String.Format(MASK, option.LongName, string.Join(option.Separator, values));
                    } else if (option.FlagType == MinerOptionFlagType.SingleParam) {
                        HashSet<string> values = new HashSet<string>();
                        foreach (var cDev in CDevs) {
                            values.Add(cdevOptions[cDev.UUID][option.Type]);
                        }
                        string setValue = option.Default;
                        if (values.Count == 1) {
                            setValue = values.First();
                        }
                        string MASK = " {0} {1}";
                        if (option.LongName.Contains('=')) {
                            MASK = " {0}{1}";
                        }
                        retVal += String.Format(MASK, option.LongName, setValue);
                    }
                }
            }

            Helpers.ConsolePrint(TAG, String.Format("Final extra launch params parse \"{0}\"", retVal));

            return retVal;
        }

        public static string ParseForCDevs(List<ComputeDevice> CDevs, AlgorithmType algorithmType, DeviceType deviceType) {
            if (deviceType == DeviceType.NVIDIA) {
                if (algorithmType != AlgorithmType.DaggerHashimoto && algorithmType != AlgorithmType.CryptoNight) {
                    return Parse(CDevs, _ccimerOptions);
                } else if (algorithmType == AlgorithmType.CryptoNight) {
                    return Parse(CDevs, _ccimerCryptoNightOptions);
                } else { // ethminer dagger
                    return ""; // TODO
                }
            } else if (deviceType == DeviceType.AMD) {
                if (algorithmType != AlgorithmType.DaggerHashimoto) {
                    string temperatureControl = ConfigManager.Instance.GeneralConfig.DisableAMDTempControl ? "" : Parse(CDevs, _sgminerTemperatureOptions, true);
                    return String.Format("{0} {1}", Parse(CDevs, _sgminerOptions), temperatureControl);
                } else { // ethminer dagger
                    return ""; // TODO
                }
            }

            return "";
        }


        // testing 
        private static List<ComputeDevice> CreateFakeCDevs(List<Algorithm> algos) {
            List<ComputeDevice> CDevs = new List<ComputeDevice>();
            int uuid = 0;
            foreach (var algo in algos) {
                ++uuid;
                ComputeDevice cDev = new ComputeDevice(0, "", String.Format("device_{0}", uuid.ToString()), uuid.ToString());
                cDev.MostProfitableAlgorithm = algo;
                CDevs.Add(cDev);
            }
            return CDevs;
        }

        public static string GetCcminer_CryptonightTest() {
            List<Algorithm> algos = new List<Algorithm>() {
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight") { ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x13" },
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight") {ExtraLaunchParameters = "should not be here --bfactor= ssw"}, // no extra launch params
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight") { ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x8" }
            };
            return ParseForCDevs(CreateFakeCDevs(algos), AlgorithmType.CryptoNight, DeviceType.NVIDIA);
        }

        public static string GetCcminer_CryptonightTest2() {
            List<Algorithm> algos = new List<Algorithm>() {
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight") { ExtraLaunchParameters = "--bsleep=  0    --bfactor=  0     --launch=32x13" },
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight"), // no extra launch params
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight") { ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x8" }
            };
            return ParseForCDevs(CreateFakeCDevs(algos), AlgorithmType.CryptoNight, DeviceType.NVIDIA);
        }

        public static string Get_NoParamsTest() {
            List<Algorithm> algos = new List<Algorithm>() {
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight"),
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight"), // no extra launch params
                new Algorithm(AlgorithmType.CryptoNight, "cryptonight")
            };
            return ParseForCDevs(CreateFakeCDevs(algos), AlgorithmType.CryptoNight, DeviceType.NVIDIA);
        }

        public static string Get_SgminerTest() {
            List<Algorithm> algos = new List<Algorithm>() {
                new Algorithm(AlgorithmType.X13,  "x13")
                    { ExtraLaunchParameters = AmdGpuDevice.TemperatureParam + "--nfactor 10 --xintensity   1024  --thread-concurrency    64  --worksize  64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam },
                    new Algorithm(AlgorithmType.X13,  "x13")
                    { ExtraLaunchParameters = AmdGpuDevice.TemperatureParam + "--nfactor 10 --xintensity   512  --thread-concurrency    128  --worksize  64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam },
                    new Algorithm(AlgorithmType.X13,  "x13")
                    { ExtraLaunchParameters = AmdGpuDevice.TemperatureParam + "--nfactor 10 --xintensity   64 --thread-concurrency    64  --worksize  64 --gpu-threads 4" + AmdGpuDevice.TemperatureParam },
            };

            return ParseForCDevs(CreateFakeCDevs(algos), AlgorithmType.X13, DeviceType.AMD);
        }
    }
}
