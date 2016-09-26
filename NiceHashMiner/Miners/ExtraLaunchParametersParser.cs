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
            new MinerOption(MinerOptionType.Ccminer_CryptoNightLaunch, "-l", "--launch=", "8x40"), // default is 8x40
            new MinerOption(MinerOptionType.Ccminer_CryptoNightBfactor, "", "--bfactor=", "0"),
            new MinerOption(MinerOptionType.Ccminer_CryptoNightBsleep, "", "--bsleep=", "0") // TODO check default
        };
        // OCL ethminer
        private static List<MinerOption> _oclEthminerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Ethminer_OCL_LocalWork, "", "--cl-local-work", "0"),
            new MinerOption(MinerOptionType.Ethminer_OCL_GlobalWork, "", "--cl-global-work", "0"),
        };

        // CUDA ethminer
        private static List<MinerOption> _cudaEthminerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.CudaBlockSize, "", "--cuda-block-size", "0"),
            new MinerOption(MinerOptionType.CudaGridSize, "", "--cuda-grid-size", "0"),
        };

        // sgminer
        private static List<MinerOption> _sgminerOptions = new List<MinerOption>() {
            // SingleParam
            new MinerOption(MinerOptionType.KeccakUnroll, "", "--keccak-unroll", "0", MinerOptionFlagType.SingleParam),
            new MinerOption(MinerOptionType.HamsiExpandBig, "", "--hamsi-expand-big", "4", MinerOptionFlagType.SingleParam),
            new MinerOption(MinerOptionType.Nfactor, "", "--nfactor", "10", MinerOptionFlagType.SingleParam),
            // MultiParam TODO IMPORTANT check defaults
            new MinerOption(MinerOptionType.Intensity, "-I", "--intensity", "d"), // default is "d" check if -1 works
            new MinerOption(MinerOptionType.Xintensity, "-X", "--xintensity", "-1"), // default none
            new MinerOption(MinerOptionType.Rawintensity, "", "--rawintensity", "-1"), // default none
            new MinerOption(MinerOptionType.ThreadConcurrency, "", "--thread-concurrency", "-1"), // default none
            new MinerOption(MinerOptionType.Worksize, "-w", "--worksize", "-1"), // default none
            new MinerOption(MinerOptionType.GpuThreads, "-g", "--gpu-threads", "1"),
            new MinerOption(MinerOptionType.LookupGap, "", "--lookup-gap", "-1"), // default none
            // Uni
        };
        private static List<MinerOption> _sgminerTemperatureOptions = new List<MinerOption>() {
            // temperature stuff
            new MinerOption(MinerOptionType.GpuFan, "", "--gpu-fan", "30-60"), // default none
            new MinerOption(MinerOptionType.TempCutoff, "", "--temp-cutoff", "95"),
            new MinerOption(MinerOptionType.TempOverheat, "", "--temp-overheat", "85"),
            new MinerOption(MinerOptionType.TempTarget, "", "--temp-target", "75"),
            new MinerOption(MinerOptionType.AutoFan, "", "--auto-fan", null, MinerOptionFlagType.Uni),
            new MinerOption(MinerOptionType.AutoGpu, "", "--auto-gpu", null, MinerOptionFlagType.Uni)
        };

        private static List<MinerOption> _cpuminerOptions = new List<MinerOption>() {
            // temperature stuff
            new MinerOption(MinerOptionType.Threads, "-t", "--threads=", "-1"), // default none
            new MinerOption(MinerOptionType.CpuAffinity, "", "--cpu-affinity", "-1"), // default none
            new MinerOption(MinerOptionType.CpuPriority, "", "--cpu-priority", "-1"), // default none
        };

        private static bool _showLog = true;

        private static void LogParser(string msg) {
            if (_showLog) {
                Helpers.ConsolePrint(TAG, msg);
            }
        }

        private static string Parse(List<ComputeDevice> CDevs, List<MinerOption> options, bool useIfDefaults = false, List<MinerOption> ignoreLogOpions = null) {
            const string IGNORE_PARAM = "Cannot parse \"{0}\", not supported, set to ignore, or wrong extra launch parameter settings";
            List<MinerOptionType> optionsOrder = new List<MinerOptionType>();
            //Dictionary<MinerOptionType, string> paramsFlags = new Dictionary<MinerOptionType, string>();
            Dictionary<string, Dictionary<MinerOptionType, string>> cdevOptions = new Dictionary<string, Dictionary<MinerOptionType, string>>();
            Dictionary<MinerOptionType, bool> isOptionDefaults = new Dictionary<MinerOptionType, bool>();
            Dictionary<MinerOptionType, bool> isOptionExist = new Dictionary<MinerOptionType, bool>();
            // init devs options, and defaults
            foreach (var cDev in CDevs) {
                var defaults = new Dictionary<MinerOptionType, string>();
                foreach (var option in options) {
                    //defaults.Add(option.Type, option.Default);
                    defaults[option.Type] = option.Default;
                }
                //cdevOptions.Add(cDev.UUID, defaults);
                cdevOptions[cDev.UUID] = defaults;
            }
            // init order and params flags, and params list
            foreach (var option in options) {
                MinerOptionType optionType = option.Type;
                optionsOrder.Add(optionType);

                //isOptionDefaults.Add(optionType, true);
                //isOptionExist.Add(optionType, false);
                isOptionDefaults[optionType] = true;
                isOptionExist[optionType] = false;
            }
            // parse
            foreach (var cDev in CDevs) {
                LogParser(String.Format("ExtraLaunch params \"{0}\" for device UUID {1}", cDev.CurrentExtraLaunchParameters, cDev.UUID));
                var parameters = cDev.CurrentExtraLaunchParameters.Replace("=", "= ").Split(' ');

                bool prevHasIgnoreParam = false;
                int logCount = 0;
                Func<string, string> ignorePrintLog = (string param) => {
                    // AMD temp controll is separated and logs stuff that is ignored
                    bool printIgnore = true;
                    if (ignoreLogOpions != null) {
                        foreach (var ignoreOption in ignoreLogOpions) {
                            if (param.Equals(ignoreOption.ShortName) || param.Equals(ignoreOption.LongName)) {
                                printIgnore = false;
                                prevHasIgnoreParam = true;
                                logCount = 0;
                                break;
                            }
                        }
                    }
                    if (printIgnore && !prevHasIgnoreParam) {
                        LogParser(String.Format(IGNORE_PARAM, param));
                    }
                    if (logCount == 1) {
                        prevHasIgnoreParam = false;
                        logCount = 0;
                    }
                    ++logCount;
                    return ""; // fake crappy C# crap
                };

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
                                    isOptionExist[option.Type] = true;
                                    cdevOptions[cDev.UUID][option.Type] = "notNull"; // if Uni param is null it is not present
                                } else { // Sinlge and Multi param
                                    currentFlag = option.Type;
                                }
                            }
                        }
                        if (isIngored) { // ignored
                            ignorePrintLog(param);
                        }
                    } else if (currentFlag != MinerOptionType.NONE) {
                        isOptionExist[currentFlag] = true;
                        cdevOptions[cDev.UUID][currentFlag] = param;
                        currentFlag = MinerOptionType.NONE;
                    } else { // problem
                        ignorePrintLog(param);
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
                        isOptionDefaults[option.Type] = false;
                    }
                }
            }

            if (!isAllDefault || useIfDefaults) {
                foreach (var option in options) {
                    if (!isOptionDefaults[option.Type] || isOptionExist[option.Type] || useIfDefaults) { // if options all default ignore
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
            }

            LogParser(String.Format("Final extra launch params parse \"{0}\"", retVal));

            return retVal;
        }


        public static string ParseForCDevs(List<ComputeDevice> CDevs, AlgorithmType algorithmType, DeviceType deviceType, bool showLog = true) {
            _showLog = showLog;
            // init cdevs extra launch parameters
            foreach (var cDev in CDevs) {
                cDev.CurrentExtraLaunchParameters = cDev.MostProfitableAlgorithm.ExtraLaunchParameters;
            }
            // parse for device
            if (deviceType == DeviceType.NVIDIA) {
                if (algorithmType != AlgorithmType.DaggerHashimoto && algorithmType != AlgorithmType.CryptoNight) {
                    return Parse(CDevs, _ccimerOptions);
                } else if (algorithmType == AlgorithmType.CryptoNight) {
                    // check if any device is SM21 or SM3.x if yes return empty for stability reasons
                    foreach (var cDev in CDevs) {
                        if (cDev.DeviceGroupType == DeviceGroupType.NVIDIA_2_1
                            || cDev.DeviceGroupType == DeviceGroupType.NVIDIA_3_x) {
                            return "";
                        }
                    }
                    return Parse(CDevs, _ccimerCryptoNightOptions, true);
                } else { // ethminer dagger
                    // use if missing compute device for correct mapping
                    int id = -1;
                    var cdevs_mappings = new List<ComputeDevice>();
                    foreach (var cDev in CDevs) {
                        while(++id != cDev.ID) {
                            var fakeCdev = new ComputeDevice(id, "", "", "");
                            fakeCdev.CurrentExtraLaunchParameters = ""; // empty
                            cdevs_mappings.Add(fakeCdev);
                        }
                        cdevs_mappings.Add(cDev);
                    }
                    return Parse(cdevs_mappings, _cudaEthminerOptions);
                }
            } else if (deviceType == DeviceType.AMD) {
                if (algorithmType != AlgorithmType.DaggerHashimoto) {
                    // rawIntensity overrides xintensity, xintensity overrides intensity
                    var sgminer_intensities = new List<MinerOption>() {
                        new MinerOption(MinerOptionType.Intensity, "-I", "--intensity", "d"), // default is "d" check if -1 works
                        new MinerOption(MinerOptionType.Xintensity, "-X", "--xintensity", "-1"), // default none
                        new MinerOption(MinerOptionType.Rawintensity, "", "--rawintensity", "-1"), // default none
                    };
                    var contains_intensity = new Dictionary<MinerOptionType, bool>() {
                        { MinerOptionType.Intensity, false },
                        { MinerOptionType.Xintensity, false },
                        { MinerOptionType.Rawintensity, false },
                    };
                    // check intensity and xintensity, the latter overrides so change accordingly
                    foreach (var cDev in CDevs) {
                        foreach (var intensityOption in sgminer_intensities) {
                            if (!string.IsNullOrEmpty(intensityOption.ShortName) && cDev.CurrentExtraLaunchParameters.Contains(intensityOption.ShortName)) {
                                cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace(intensityOption.ShortName, intensityOption.LongName);
                                contains_intensity[intensityOption.Type] = true; 
                            }
                            if (cDev.CurrentExtraLaunchParameters.Contains(intensityOption.LongName)) {
                                contains_intensity[intensityOption.Type] = true;
                            }
                        }
                    }
                    
                    // replace
                    if(contains_intensity[MinerOptionType.Intensity] && contains_intensity[MinerOptionType.Xintensity]) {
                        LogParser("Sgminer replacing --intensity with --xintensity");
                        foreach (var cDev in CDevs) {
                            cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace("--intensity", "--xintensity");
                        }
                    }
                    if (contains_intensity[MinerOptionType.Xintensity] && contains_intensity[MinerOptionType.Rawintensity]) {
                        LogParser("Sgminer replacing --xintensity with --rawintensity");
                        foreach (var cDev in CDevs) {
                            cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace("--xintensity", "--rawintensity");
                        }
                    }

                    List<MinerOption> sgminerOptionsNew = new List<MinerOption>();
                    string temperatureControl = "";
                    // temp control and parse
                    if (ConfigManager.Instance.GeneralConfig.DisableAMDTempControl) {
                        LogParser("DisableAMDTempControl is TRUE, temp control parameters will be ignored");
                    } else {
                        LogParser("Sgminer parsing temperature control parameters");
                        temperatureControl = Parse(CDevs, _sgminerTemperatureOptions, true, _sgminerOptions);
                    }
                    LogParser("Sgminer parsing default parameters");
                    string returnStr = String.Format("{0} {1}", Parse(CDevs, _sgminerOptions, false, _sgminerTemperatureOptions), temperatureControl);
                    LogParser("Sgminer extra launch parameters merged: " + returnStr);
                    return returnStr;
                } else { // ethminer dagger
                    // use if missing compute device for correct mapping
                    int id = -1;
                    var cdevs_mappings = new List<ComputeDevice>();
                    foreach (var cDev in CDevs) {
                        while (++id != cDev.ID) {
                            var fakeCdev = new ComputeDevice(id, "", "", "");
                            fakeCdev.CurrentExtraLaunchParameters = ""; // empty
                            cdevs_mappings.Add(fakeCdev);
                        }
                        cdevs_mappings.Add(cDev);
                    }
                    return Parse(cdevs_mappings, _oclEthminerOptions);
                }
            } else if (deviceType == DeviceType.CPU) {
                foreach (var cDev in CDevs) {
                    // extra thread check
                    if (cDev.CurrentExtraLaunchParameters.Contains("--threads=") || cDev.CurrentExtraLaunchParameters.Contains("-t")) {
                        // nothing
                    } else { // add threads params mandatory
                        cDev.CurrentExtraLaunchParameters += " --threads=" + GetThreads(cDev.Threads, cDev.MostProfitableAlgorithm.LessThreads).ToString();
                    }
                }
                return Parse(CDevs, _cpuminerOptions);
            }

            return "";
        }

        private static int GetThreads(int Threads, int LessThreads) {
            if (Threads > LessThreads) {
                return Threads - LessThreads;
            }
            return Threads;
        }


        /////////////////////////////////////////////////////////////////// testing 
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
