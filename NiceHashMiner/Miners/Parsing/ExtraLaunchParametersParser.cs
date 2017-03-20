using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Parsing {
    using MinerOptionType = String;
    static class ExtraLaunchParametersParser {
        private static readonly string TAG = "ExtraLaunchParametersParser";
        private static readonly string MinerOptionType_NONE = "MinerOptionType_NONE";
        

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
                    new MinerOption("CUDA_Solver_Thread", "-ct", "-ct", "1", MinerOptionFlagType.MultiParam, " "),
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

        private static void IgnorePrintLogInit() {
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

        private static string Parse(List<MiningPair> MiningPairs, List<MinerOption> options, bool useIfDefaults = true, List<MinerOption> ignoreLogOpions = null) {
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

                IgnorePrintLogInit();


                MinerOptionType currentFlag = MinerOptionType_NONE;
                foreach (var param in parameters) {
                    if (param.Equals("")) { // skip
                        continue;
                    } else if (currentFlag == MinerOptionType_NONE) {
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
                    } else if (currentFlag != MinerOptionType_NONE) {
                        isOptionExist[currentFlag] = true;
                        cdevOptions[pair.Device.UUID][currentFlag] = param;
                        currentFlag = MinerOptionType_NONE;
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
                        } else if (option.FlagType == MinerOptionFlagType.DuplicateMultiParam) {
                            List<string> values = new List<string>();
                            string MASK = " {0} {1}";
                            foreach (var pair in MiningPairs) {
                                values.Add(String.Format(MASK, option.LongName, cdevOptions[pair.Device.UUID][option.Type]));
                            }
                            retVal += " " + StringHelper.Join(" ", values);
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
                deviceType, showLog);
        }

        public static string ParseForMiningPair(MiningPair miningPair, AlgorithmType algorithmType, DeviceType deviceType, bool showLog = true) {
            return ParseForMiningPairs(
                new List<MiningPair>() { miningPair },
                deviceType, showLog);
        }

        private static MinerType GetMinerType(DeviceType deviceType, MinerBaseType minerBaseType, AlgorithmType algorithmType) {    
            //if (MinerBaseType.cpuminer == minerBaseType) {
            //    return MinerType.cpuminer_opt;
            //}
            if (MinerBaseType.OptiminerAMD == minerBaseType) {
                return MinerType.OptiminerZcash;
            }
            if (MinerBaseType.sgminer == minerBaseType) {
                return MinerType.sgminer;
            }
            if (MinerBaseType.ccminer == minerBaseType || MinerBaseType.ccminer_alexis == minerBaseType || MinerBaseType.experimental == minerBaseType) {
                if (AlgorithmType.CryptoNight == algorithmType) {
                    return MinerType.ccminer_CryptoNight;
                }
                return MinerType.ccminer;
            }
            if (MinerBaseType.ClaymoreAMD == minerBaseType) {
                if (AlgorithmType.CryptoNight == algorithmType) {
                    return MinerType.ClaymoreCryptoNight;
                }
                if (AlgorithmType.Equihash == algorithmType) {
                    return MinerType.ClaymoreZcash;
                }
                if (AlgorithmType.DaggerHashimoto == algorithmType) {
                    return MinerType.ClaymoreDual;
                }
            }
            if (MinerBaseType.ethminer == minerBaseType) {
                if(DeviceType.AMD == deviceType) {
                    return MinerType.ethminer_OCL;
                }
                if(DeviceType.NVIDIA == deviceType) {
                    return MinerType.ethminer_CUDA;
                }
            }
            if (MinerBaseType.nheqminer == minerBaseType) {
                if(DeviceType.CPU == deviceType) {
                    return MinerType.nheqminer_CPU;
                }
                if(DeviceType.AMD == deviceType) {
                    return MinerType.nheqminer_AMD;
                }
                if(DeviceType.NVIDIA == deviceType) {
                    return MinerType.nheqminer_CUDA;
                }
            }
            if (MinerBaseType.eqm == minerBaseType) {
                if(DeviceType.CPU == deviceType) {
                    return MinerType.eqm_CPU;
                }
                if(DeviceType.NVIDIA == deviceType) {
                    return MinerType.eqm_CUDA;
                }
            }
            if (MinerBaseType.excavator == minerBaseType) {
                return MinerType.excavator;
            }

            return MinerType.NONE;
        }

        private static string ParseForMiningPairs(List<MiningPair> MiningPairs, DeviceType deviceType, bool showLog = true) {
            _showLog = showLog;

            MinerBaseType minerBaseType = MinerBaseType.NONE;
            AlgorithmType algorithmType = AlgorithmType.NONE;
            if (MiningPairs.Count > 0) {
                var algo = MiningPairs[0].Algorithm;
                if (algo != null) {
                    algorithmType = algo.NiceHashID;
                    minerBaseType = algo.MinerBaseType;
                }
            }

            MinerType minerType = GetMinerType(deviceType, minerBaseType, algorithmType);

            MinerOptionPackage minerOptionPackage = ExtraLaunchParameters.GetMinerOptionPackageForMinerType(minerType);

            List<MiningPair> setMiningPairs = MiningPairs.ConvertAll((mp) => mp);
            // handle exceptions and package parsing
            // CPU exception
            if (deviceType == DeviceType.CPU) {
                CheckAndSetCPUPairs(setMiningPairs);
            }
            // ethminer exception
            if (MinerType.ethminer_OCL == minerType || MinerType.ethminer_CUDA == minerType) {
                // use if missing compute device for correct mapping
                // init fakes workaround
                var cdevs_mappings = new List<MiningPair>();
                {
                    int id = -1;
                    var fakeAlgo = new Algorithm(MinerBaseType.ethminer, AlgorithmType.DaggerHashimoto, "daggerhashimoto");
                    foreach (var pair in setMiningPairs) {
                        while (++id != pair.Device.ID) {
                            var fakeCdev = new ComputeDevice(id);
                            cdevs_mappings.Add(new MiningPair(fakeCdev, fakeAlgo));
                        }
                        cdevs_mappings.Add(pair);
                    }
                }
                // reset setMiningPairs
                setMiningPairs = cdevs_mappings;
            }
            // sgminer exception handle intensity types
            if (MinerType.sgminer == minerType) {
                // rawIntensity overrides xintensity, xintensity overrides intensity
                var sgminer_intensities = new List<MinerOption>() {
                    new MinerOption("Intensity", "-I", "--intensity", "d", MinerOptionFlagType.MultiParam, ","), // default is "d" check if -1 works
                    new MinerOption("Xintensity", "-X", "--xintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                    new MinerOption("Rawintensity", "", "--rawintensity", "-1", MinerOptionFlagType.MultiParam, ","), // default none
                };
                var contains_intensity = new Dictionary<MinerOptionType, bool>() {
                    { "Intensity", false },
                    { "Xintensity", false },
                    { "Rawintensity", false },
                };
                // check intensity and xintensity, the latter overrides so change accordingly
                foreach (var cDev in setMiningPairs) {
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
                if (contains_intensity["Intensity"] && contains_intensity["Xintensity"]) {
                    LogParser("Sgminer replacing --intensity with --xintensity");
                    foreach (var cDev in setMiningPairs) {
                        cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace("--intensity", "--xintensity");
                    }
                }
                if (contains_intensity["Xintensity"] && contains_intensity["Rawintensity"]) {
                    LogParser("Sgminer replacing --xintensity with --rawintensity");
                    foreach (var cDev in setMiningPairs) {
                        cDev.CurrentExtraLaunchParameters = cDev.CurrentExtraLaunchParameters.Replace("--xintensity", "--rawintensity");
                    }
                }
            }
        
            string ret = "";
            string general = Parse(setMiningPairs, minerOptionPackage.GeneralOptions, false, minerOptionPackage.TemperatureOptions);
            // temp control and parse
            if (ConfigManager.GeneralConfig.DisableAMDTempControl) {
                LogParser("DisableAMDTempControl is TRUE, temp control parameters will be ignored");
                ret = general;
            } else {
                LogParser("AMD parsing temperature control parameters");
                // temp = Parse(setMiningPairs, minerOptionPackage.TemperatureOptions, true, minerOptionPackage.GeneralOptions);            
                string temp = Parse(setMiningPairs, minerOptionPackage.TemperatureOptions, false, minerOptionPackage.GeneralOptions);

                ret = general + "  " + temp;
            }
        
            return ret;
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

        public static int GetThreadsNumber(MiningPair cpuPair) {
            var cDev = cpuPair.Device;
            var algo = cpuPair.Algorithm;
            // extra thread check
            if (algo.ExtraLaunchParameters.Contains("--threads=") || algo.ExtraLaunchParameters.Contains("-t")) {
                var strings = algo.ExtraLaunchParameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int i = -1;
                for (int cur_i = 0; cur_i < strings.Length; ++cur_i) {
                    if (strings[cur_i] == "--threads=" || strings[cur_i] == "-t") {
                        i = cur_i + 1;
                        break;
                    }
                }
                if (i > -1 && strings.Length < i) {
                    int numTr = cDev.Threads;
                    if (Int32.TryParse(strings[i], out numTr)) {
                        if (numTr <= cDev.Threads) return numTr; 
                    }
                }
            }
            return GetThreads(cDev.Threads, cpuPair.Algorithm.LessThreads);
        }

        private static int GetThreads(int Threads, int LessThreads) {
            if (Threads > LessThreads) {
                return Threads - LessThreads;
            }
            return Threads;
        }
    }
}
