using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Parsing {
    static class ExtraLaunchParametersParser {
        private static readonly string TAG = "ExtraLaunchParametersParser";

        // Order of miner options tpyes is important make sure to implement it corectly
        // ccminer
        private static List<MinerOption> _ccimerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Intensity, "-i", "--intensity=", "0", MinerOptionFlagType.MultiParam, ",")
        };
        // ccminer CryptoNight
        private static List<MinerOption> _ccimerCryptoNightOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Ccminer_CryptoNightLaunch, "-l", "--launch=", "8x40", MinerOptionFlagType.MultiParam, ","), // default is 8x40
            new MinerOption(MinerOptionType.Ccminer_CryptoNightBfactor, "", "--bfactor=", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.Ccminer_CryptoNightBsleep, "", "--bsleep=", "0", MinerOptionFlagType.MultiParam, ",") // TODO check default
        };
        // OCL ethminer
        private static List<MinerOption> _oclEthminerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Ethminer_OCL_LocalWork, "", "--cl-local-work", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.Ethminer_OCL_GlobalWork, "", "--cl-global-work", "0", MinerOptionFlagType.MultiParam, ","),
        };

        // CUDA ethminer
        private static List<MinerOption> _cudaEthminerOptions = new List<MinerOption>() {
            new MinerOption(MinerOptionType.CudaBlockSize, "", "--cuda-block-size", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.CudaGridSize, "", "--cuda-grid-size", "0", MinerOptionFlagType.MultiParam, ","),
        };

        // sgminer
        private static List<MinerOption> _sgminerOptions = new List<MinerOption>() {
            // SingleParam
            new MinerOption(MinerOptionType.KeccakUnroll, "", "--keccak-unroll", "0", MinerOptionFlagType.SingleParam, ""),
            new MinerOption(MinerOptionType.HamsiExpandBig, "", "--hamsi-expand-big", "4", MinerOptionFlagType.SingleParam, ""),
            new MinerOption(MinerOptionType.Nfactor, "", "--nfactor", "10", MinerOptionFlagType.SingleParam, ""),
            // MultiParam TODO IMPORTANT check defaults
            new MinerOption(MinerOptionType.Intensity, "-I", "--intensity", "d", MinerOptionFlagType.MultiParam, ","), // default is "d" check if -1 works
            new MinerOption(MinerOptionType.Xintensity, "-X", "--xintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.Rawintensity, "", "--rawintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.ThreadConcurrency, "", "--thread-concurrency", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.Worksize, "-w", "--worksize", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.GpuThreads, "-g", "--gpu-threads", "1", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.LookupGap, "", "--lookup-gap", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            // Uni
        };
        private static List<MinerOption> _sgminerTemperatureOptions = new List<MinerOption>() {
            // temperature stuff
            new MinerOption(MinerOptionType.GpuFan, "", "--gpu-fan", "30-60", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.TempCutoff, "", "--temp-cutoff", "95", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.TempOverheat, "", "--temp-overheat", "85", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.TempTarget, "", "--temp-target", "75", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.AutoFan, "", "--auto-fan", null, MinerOptionFlagType.Uni, ""),
            new MinerOption(MinerOptionType.AutoGpu, "", "--auto-gpu", null, MinerOptionFlagType.Uni, "")
        };

        private static List<MinerOption> _cpuminerOptions = new List<MinerOption>() {
            // temperature stuff
            new MinerOption(MinerOptionType.Threads, "-t", "--threads=", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.CpuAffinity, "", "--cpu-affinity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
            new MinerOption(MinerOptionType.CpuPriority, "", "--cpu-priority", "-1", MinerOptionFlagType.MultiParam, ","), // default none
        };
        // nheqminer 
        private static List<MinerOption> _nheqminer_CPU_Options = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Threads, "-t", "-t", "-1", MinerOptionFlagType.SingleParam, " "), // default none
        };
        private static List<MinerOption> _nheqminer_CUDA_Options = new List<MinerOption>() {
            new MinerOption(MinerOptionType.CUDA_Solver_Version, "-cv", "-cv", "0", MinerOptionFlagType.SingleParam, " "), // default 0
            new MinerOption(MinerOptionType.CUDA_Solver_Block, "-cb", "-cb", "0", MinerOptionFlagType.MultiParam, " "), // default 0
            new MinerOption(MinerOptionType.CUDA_Solver_Thread, "-ct", "-ct", "0", MinerOptionFlagType.MultiParam, " "), // default 0
        };
        private static List<MinerOption> _nheqminer_AMD_Options = new List<MinerOption>() {
            new MinerOption(MinerOptionType.OpenCL_Solver_Version, "-ov", "-ov", "0", MinerOptionFlagType.SingleParam, " "), // default none
            new MinerOption(MinerOptionType.OpenCL_Solver_Dev_Thread, "-ot", "-ot", "1", MinerOptionFlagType.MultiParam, " "), // default none
        };
        // eqm
        private static List<MinerOption> _eqm_CPU_Options = new List<MinerOption>() {
            new MinerOption(MinerOptionType.Threads, "-t", "-t", "-1", MinerOptionFlagType.SingleParam, " "), // default none
        };
        // eqm CUDA
        private static List<MinerOption> _eqm_CUDA_Options = new List<MinerOption>() {
            new MinerOption(MinerOptionType.CUDA_Solver_Mode, "-cm", "-cm", "0", MinerOptionFlagType.MultiParam, " "), // default 0
        };
        // Zcash claymore
        private static List<MinerOption> _ClaymoreZcash_Options = new List<MinerOption>() {
            new MinerOption(MinerOptionType.ClaymoreZcash_a      , "-a", "-a", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_i      , "-i", "-i", "6", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_wd     , "-wd", "-wd", "1", MinerOptionFlagType.SingleParam, ","),
            //new MinerOption(MinerOptionType.ClaymoreZcash_r      , , , , MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_nofee  , "-nofee", "-nofee", "0", MinerOptionFlagType.SingleParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_li     , "-li", "-li", "0", MinerOptionFlagType.MultiParam, ","),
            // temperature stuff
            //MinerOptionFlagType.MultiParam might not work corectly due to ADL indexing so use single param to apply to all
            new MinerOption(MinerOptionType.ClaymoreZcash_tt     , "-tt", "-tt", "1", MinerOptionFlagType.SingleParam, ","), 
            new MinerOption(MinerOptionType.ClaymoreZcash_ttli   , "-ttli", "-ttli", "70", MinerOptionFlagType.SingleParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_tstop  , "-tstop", "-tstop", "0", MinerOptionFlagType.SingleParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_fanmax , "-fanmax", "-fanmax", "100", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_fanmin , "-fanmin", "-fanmin", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_cclock , "-cclock", "-cclock", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_mclock , "-mclock", "-mclock", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_powlim , "-powlim", "-powlim", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_cvddc  , "-cvddc", "-cvddc", "0", MinerOptionFlagType.MultiParam, ","),
            new MinerOption(MinerOptionType.ClaymoreZcash_mvddc  , "-mvddc", "-mvddc", "0", MinerOptionFlagType.MultiParam, ","),
        };

        private static bool _showLog = true;

        private static void LogParser(string msg) {
            if (_showLog) {
                Helpers.ConsolePrint(TAG, msg);
            }
        }

        // exception...
        public static int GetEqmCudaThreadCount(MiningPair pair) {
            if (pair.CurrentExtraLaunchParameters.Contains("-ct")) {
                List<MinerOption> eqm_CUDA_Options = new List<MinerOption>() {
                    new MinerOption(MinerOptionType.CUDA_Solver_Thread, "-ct", "-ct", "1", MinerOptionFlagType.MultiParam, " "),
                };
                string parsedStr = Parse(new List<MiningPair>() { pair }, eqm_CUDA_Options, true);
                try {
                    int threads = Int32.Parse(parsedStr.Trim().Replace("-ct", "").Trim());
                    return threads;
                } catch { }
            }
            return 1; // default 
        }

        private static bool prevHasIgnoreParam = false;
        private static int logCount = 0;

        private static void IgnorePrintLogIbnit() {
            prevHasIgnoreParam = false;
            logCount = 0;
        }

        private static void IgnorePrintLog(string param, string IGNORE_PARAM, List<MinerOption> ignoreLogOpions = null) {
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
        }

        private static string Parse(List<MiningPair> MiningPairs, List<MinerOption> options, bool useIfDefaults = false, List<MinerOption> ignoreLogOpions = null) {
            const string IGNORE_PARAM = "Cannot parse \"{0}\", not supported, set to ignore, or wrong extra launch parameter settings";
            List<MinerOptionType> optionsOrder = new List<MinerOptionType>();
            Dictionary<string, Dictionary<MinerOptionType, string>> cdevOptions = new Dictionary<string, Dictionary<MinerOptionType, string>>();
            Dictionary<MinerOptionType, bool> isOptionDefaults = new Dictionary<MinerOptionType, bool>();
            Dictionary<MinerOptionType, bool> isOptionExist = new Dictionary<MinerOptionType, bool>();
            // init devs options, and defaults
            foreach (var pair in MiningPairs) {
                var defaults = new Dictionary<MinerOptionType, string>();
                foreach (var option in options) {
                    defaults[option.Type] = option.Default;
                }
                cdevOptions[pair.Device.UUID] = defaults;
            }
            // init order and params flags, and params list
            foreach (var option in options) {
                MinerOptionType optionType = option.Type;
                optionsOrder.Add(optionType);

                isOptionDefaults[optionType] = true;
                isOptionExist[optionType] = false;
            }
            // parse
            foreach (var pair in MiningPairs) {
                LogParser(String.Format("ExtraLaunch params \"{0}\" for device UUID {1}", pair.CurrentExtraLaunchParameters, pair.Device.UUID));
                var parameters = pair.CurrentExtraLaunchParameters.Replace("=", "= ").Split(' ');

                IgnorePrintLogIbnit();


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
                                    cdevOptions[pair.Device.UUID][option.Type] = "notNull"; // if Uni param is null it is not present
                                } else { // Sinlge and Multi param
                                    currentFlag = option.Type;
                                }
                            }
                        }
                        if (isIngored) { // ignored
                            IgnorePrintLog(param, IGNORE_PARAM, ignoreLogOpions);
                        }
                    } else if (currentFlag != MinerOptionType.NONE) {
                        isOptionExist[currentFlag] = true;
                        cdevOptions[pair.Device.UUID][currentFlag] = param;
                        currentFlag = MinerOptionType.NONE;
                    } else { // problem
                        IgnorePrintLog(param, IGNORE_PARAM, ignoreLogOpions);
                    }
                }
            }

            string retVal = "";

            // check if is all defaults
            bool isAllDefault = true;
            foreach (var pair in MiningPairs) {
                foreach (var option in options) {
                    if (option.Default != cdevOptions[pair.Device.UUID][option.Type]) {
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
                        foreach (var pair in MiningPairs) {
                            if (cdevOptions[pair.Device.UUID][option.Type] != null) {
                                isOptionInUse = true;
                                break;
                            }
                        }
                        if (isOptionInUse) {
                            retVal += String.Format(" {0}", option.LongName);
                        }
                        } else if(option.FlagType == MinerOptionFlagType.MultiParam) {
                            List<string> values = new List<string>();
                            foreach (var pair in MiningPairs) {
                                values.Add(cdevOptions[pair.Device.UUID][option.Type]);
                            }
                            string MASK = " {0} {1}";
                            if(option.LongName.Contains("=")) {
                                MASK = " {0}{1}";
                            }
                            retVal += String.Format(MASK, option.LongName, StringHelper.Join(option.Separator, values));
                        } else if (option.FlagType == MinerOptionFlagType.SingleParam) {
                            HashSet<string> values = new HashSet<string>();
                            foreach (var pair in MiningPairs) {
                                values.Add(cdevOptions[pair.Device.UUID][option.Type]);
                            }
                            string setValue = option.Default;
                            if (values.Count >= 1) {
                                // Always take first
                                setValue = values.First();
                            }
                            string MASK = " {0} {1}";
                            if (option.LongName.Contains("=")) {
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

        public static string ParseForMiningSetup(MiningSetup miningSetup, DeviceType deviceType, bool showLog = true) {
            return ParseForMiningPairs(
                miningSetup.MiningPairs,
                miningSetup.CurrentAlgorithmType,
                deviceType, miningSetup.MinerPath, showLog);
        }

        public static string ParseForMiningPair(MiningPair miningPair, AlgorithmType algorithmType, DeviceType deviceType, bool showLog = true) {
            return ParseForMiningPairs(
                new List<MiningPair>() { miningPair },
                algorithmType, deviceType,
                MinerPaths.GetOptimizedMinerPath(miningPair), showLog);
        }

        private static string ParseForMiningPairs(List<MiningPair> MiningPairs, AlgorithmType algorithmType, DeviceType deviceType, string minerPath, bool showLog = true) {
            _showLog = showLog;

            // parse for nheqminer
            bool deviceCheckSkip = algorithmType == AlgorithmType.Equihash || algorithmType == AlgorithmType.DaggerHashimoto;
            if (algorithmType == AlgorithmType.Equihash) {
                // nheqminer
                if (minerPath == MinerPaths.nheqminer) {
                    if (deviceType == DeviceType.CPU) {
                        CheckAndSetCPUPairs(MiningPairs);
                        return Parse(MiningPairs, _nheqminer_CPU_Options);
                    }
                    if (deviceType == DeviceType.NVIDIA) {
                        return Parse(MiningPairs, _nheqminer_CUDA_Options);
                    }
                    if (deviceType == DeviceType.AMD) {
                        return Parse(MiningPairs, _nheqminer_AMD_Options);
                    }
                } else if (minerPath == MinerPaths.eqm) {
                    if (deviceType == DeviceType.CPU) {
                        CheckAndSetCPUPairs(MiningPairs);
                        return Parse(MiningPairs, _eqm_CPU_Options);
                    }
                    if (deviceType == DeviceType.NVIDIA) {
                        return Parse(MiningPairs, _eqm_CUDA_Options);
                    }
                } else if (minerPath == MinerPaths.ClaymoreZcashMiner) {
                    return Parse(MiningPairs, _ClaymoreZcash_Options);
                }
            } else if (algorithmType == AlgorithmType.DaggerHashimoto) { // ethminer dagger
                // use if missing compute device for correct mapping
                // init fakes workaround
                var cdevs_mappings = new List<MiningPair>();
                {
                    int id = -1;
                    var fakeAlgo = new Algorithm(AlgorithmType.DaggerHashimoto, "daggerhashimoto");
                    foreach (var pair in MiningPairs) {
                        while (++id != pair.Device.ID) {
                            var fakeCdev = new ComputeDevice(id);
                            cdevs_mappings.Add(new MiningPair(fakeCdev, fakeAlgo));
                        }
                        cdevs_mappings.Add(pair);
                    }
                }
                if (deviceType == DeviceType.NVIDIA) {
                    return Parse(cdevs_mappings, _cudaEthminerOptions);
                } else if (deviceType == DeviceType.AMD) {
                    return Parse(cdevs_mappings, _oclEthminerOptions);
                }
            } else if (deviceCheckSkip == false) {
                // parse for device
                if (deviceType == DeviceType.CPU) {
                    CheckAndSetCPUPairs(MiningPairs);
                    return Parse(MiningPairs, _cpuminerOptions);
                } else if (deviceType == DeviceType.NVIDIA) {
                    if (algorithmType != AlgorithmType.CryptoNight) {
                        return Parse(MiningPairs, _ccimerOptions);
                    } else if (algorithmType == AlgorithmType.CryptoNight) {
                        // check if any device is SM21 or SM3.x if yes return empty for stability reasons
                        foreach (var pair in MiningPairs) {
                            var groupType = pair.Device.DeviceGroupType;
                            if (groupType == DeviceGroupType.NVIDIA_2_1 || groupType == DeviceGroupType.NVIDIA_3_x) {
                                return "";
                            }
                        }
                        return Parse(MiningPairs, _ccimerCryptoNightOptions, true);
                    }
                } else if (deviceType == DeviceType.AMD) {
                    // rawIntensity overrides xintensity, xintensity overrides intensity
                    var sgminer_intensities = new List<MinerOption>() {
                        new MinerOption(MinerOptionType.Intensity, "-I", "--intensity", "d", MinerOptionFlagType.MultiParam, ","), // default is "d" check if -1 works
                        new MinerOption(MinerOptionType.Xintensity, "-X", "--xintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                        new MinerOption(MinerOptionType.Rawintensity, "", "--rawintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    };
                    var contains_intensity = new Dictionary<MinerOptionType, bool>() {
                        { MinerOptionType.Intensity, false },
                        { MinerOptionType.Xintensity, false },
                        { MinerOptionType.Rawintensity, false },
                    };
                    // check intensity and xintensity, the latter overrides so change accordingly
                    foreach (var cDev in MiningPairs) {
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
                    if (contains_intensity[MinerOptionType.Intensity] && contains_intensity[MinerOptionType.Xintensity]) {
                        LogParser("Sgminer replacing --intensity with --xintensity");
                        foreach (var cDev in MiningPairs) {
                            cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace("--intensity", "--xintensity");
                        }
                    }
                    if (contains_intensity[MinerOptionType.Xintensity] && contains_intensity[MinerOptionType.Rawintensity]) {
                        LogParser("Sgminer replacing --xintensity with --rawintensity");
                        foreach (var cDev in MiningPairs) {
                            cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace("--xintensity", "--rawintensity");
                        }
                    }

                    List<MinerOption> sgminerOptionsNew = new List<MinerOption>();
                    string temperatureControl = "";
                    // temp control and parse
                    if (ConfigManager.GeneralConfig.DisableAMDTempControl) {
                        LogParser("DisableAMDTempControl is TRUE, temp control parameters will be ignored");
                    } else {
                        LogParser("Sgminer parsing temperature control parameters");
                        temperatureControl = Parse(MiningPairs, _sgminerTemperatureOptions, true, _sgminerOptions);
                    }
                    LogParser("Sgminer parsing default parameters");
                    string returnStr = String.Format("{0} {1}", Parse(MiningPairs, _sgminerOptions, false, _sgminerTemperatureOptions), temperatureControl);
                    LogParser("Sgminer extra launch parameters merged: " + returnStr);
                    return returnStr;
                }
            }

            return "";
        }

        private static void CheckAndSetCPUPairs(List<MiningPair> MiningPairs) {
            foreach (var pair in MiningPairs) {
                var cDev = pair.Device;
                // extra thread check
                if (pair.CurrentExtraLaunchParameters.Contains("--threads=") || pair.CurrentExtraLaunchParameters.Contains("-t")) {
                    // nothing
                } else { // add threads params mandatory
                    pair.CurrentExtraLaunchParameters += " -t " + GetThreads(cDev.Threads, pair.Algorithm.LessThreads).ToString();
                }
            }
        }

        private static int GetThreads(int Threads, int LessThreads) {
            if (Threads > LessThreads) {
                return Threads - LessThreads;
            }
            return Threads;
        }
    }
}
