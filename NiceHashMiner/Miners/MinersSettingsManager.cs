using NiceHashMiner.Configs.ConfigJsonFile;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners {
    public static class MinersSettingsManager {

        class MinerReservedPortsFile : ConfigFile<Dictionary<MinerBaseType, Dictionary<string, Dictionary<AlgorithmType, List<int>>>>> {
            public MinerReservedPortsFile()
                : base(FOLDERS.CONFIG, "MinerReservedPorts.json", "MinerReservedPorts_old.json") {
            }
        }

        // {miner path : {envName : envValue} }
        class MinerSystemVariablesFile : ConfigFile<Dictionary<string, Dictionary<string, string>>> {
            public MinerSystemVariablesFile() : base(FOLDERS.CONFIG, "MinerSystemVariables.json", "MinerSystemVariables_old.json") {}
        }

        private static Dictionary<MinerBaseType,
            Dictionary<string,
                Dictionary<AlgorithmType,
                    List<int>>>> MinerReservedPorts = new Dictionary<MinerBaseType, Dictionary<string, Dictionary<AlgorithmType, List<int>>>>();
        public static List<int> AllReservedPorts = new List<int>();

        public static Dictionary<string, Dictionary<string, string>> MinerSystemVariables = new Dictionary<string, Dictionary<string, string>>();

        public static void Init() {
            ExtraLaunchParameters.InitializePackages();
            InitMinerReservedPortsFile();
            InitMinerSystemVariablesFile();
        }

        public static List<int> GetPortsListFor(MinerBaseType minerBaseType, string path, AlgorithmType algorithmType) {
            if (MinerReservedPorts != null && MinerReservedPorts.ContainsKey(minerBaseType)) {
                if (MinerReservedPorts[minerBaseType] != null && MinerReservedPorts[minerBaseType].ContainsKey(path)) {
                    if (MinerReservedPorts[minerBaseType][path] != null && MinerReservedPorts[minerBaseType][path].ContainsKey(algorithmType)) {
                        if (MinerReservedPorts[minerBaseType][path][algorithmType] != null) {
                            return MinerReservedPorts[minerBaseType][path][algorithmType];
                        }
                    }
                }
            }

            return new List<int>();
        }

        public static void InitMinerReservedPortsFile() {
            MinerReservedPortsFile file = new MinerReservedPortsFile();
            MinerReservedPorts = new Dictionary<MinerBaseType, Dictionary<string, Dictionary<AlgorithmType, List<int>>>>();
            if (file.IsFileExists()) {
                var read = file.ReadFile();
                if (read != null) {
                    MinerReservedPorts = read;
                }
            }
            try {
                for (MinerBaseType type = (MinerBaseType.NONE + 1); type < MinerBaseType.END; ++type) {
                    if (MinerReservedPorts.ContainsKey(type) == false) {
                        MinerReservedPorts[type] = new Dictionary<string, Dictionary<AlgorithmType, List<int>>>();
                    }
                }
                for (DeviceGroupType devGroupType = (DeviceGroupType.NONE + 1); devGroupType < DeviceGroupType.LAST; ++devGroupType) {
                    var minerAlgosForGroup = GroupAlgorithms.CreateDefaultsForGroup(devGroupType);
                    if (minerAlgosForGroup != null) {
                        foreach (var mbaseKvp in minerAlgosForGroup) {
                            MinerBaseType minerBaseType = mbaseKvp.Key;
                            if (MinerReservedPorts.ContainsKey(minerBaseType)) {
                                var algos = mbaseKvp.Value;
                                foreach (var algo in algos) {
                                    var algoType = algo.NiceHashID;
                                    var path = MinerPaths.GetPathFor(minerBaseType, algoType, devGroupType);
                                    var isPathValid = path != MinerPaths.Data.NONE;
                                    if (isPathValid && MinerReservedPorts[minerBaseType].ContainsKey(path) == false) {
                                        MinerReservedPorts[minerBaseType][path] = new Dictionary<AlgorithmType, List<int>>();
                                    }
                                    if (isPathValid && MinerReservedPorts[minerBaseType][path] != null && MinerReservedPorts[minerBaseType][path].ContainsKey(algoType) == false) {
                                        MinerReservedPorts[minerBaseType][path][algoType] = new List<int>();
                                    }
                                }
                            }
                        }
                    }
                }
                file.Commit(MinerReservedPorts);
                // set all reserved
                foreach (var paths in MinerReservedPorts.Values) {
                    foreach (var algos in paths.Values) {
                        foreach (var ports in algos.Values) {
                            foreach (int port in ports) {
                                AllReservedPorts.Add(port);
                            }
                        }
                    }
                }
            } catch {
            }
        }

        public static void InitMinerSystemVariablesFile() {
            MinerSystemVariablesFile file = new MinerSystemVariablesFile();
            MinerSystemVariables = new Dictionary<string, Dictionary<string, string>>();
            bool isFileInit = false;
            if (file.IsFileExists()) {
                var read = file.ReadFile();
                if (read != null) {
                    isFileInit = true;
                    MinerSystemVariables = read;
                }
            }
            if (!isFileInit) {
                // general AMD defaults scope
                {
                    List<string> minerPaths = new List<string>() {
                        MinerPaths.Data.sgminer_5_6_0_general,
                        MinerPaths.Data.sgminer_gm,
                        MinerPaths.Data.ClaymoreCryptoNightMiner,
                        MinerPaths.Data.ClaymoreZcashMiner,
                        MinerPaths.Data.OptiminerZcashMiner
                    };
                    foreach (var minerPath in minerPaths) {
                        MinerSystemVariables[minerPath] = new Dictionary<string, string>() {
                            { "GPU_MAX_ALLOC_PERCENT",      "100" },
                            { "GPU_USE_SYNC_OBJECTS",       "1" },
                            { "GPU_SINGLE_ALLOC_PERCENT",   "100" },
                            { "GPU_MAX_HEAP_SIZE",          "100" },
                            { "GPU_FORCE_64BIT_PTR",        "1" }
                        };
                    }
                }
                // ClaymoreDual scope
                {
                    MinerSystemVariables[MinerPaths.Data.ClaymoreDual] = new Dictionary<string, string>() {
                        { "GPU_MAX_ALLOC_PERCENT",      "100" },
                        { "GPU_USE_SYNC_OBJECTS",       "1" },
                        { "GPU_SINGLE_ALLOC_PERCENT",   "100" },
                        { "GPU_MAX_HEAP_SIZE",          "100" },
                        { "GPU_FORCE_64BIT_PTR",        "0" }
                    };
                }
                // save defaults
                file.Commit(MinerSystemVariables);
            }
        }

    }
}
