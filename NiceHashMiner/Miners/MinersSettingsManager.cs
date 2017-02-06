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

        private static Dictionary<MinerBaseType,
            Dictionary<string,
                Dictionary<AlgorithmType,
                    List<int>>>> MinerReservedPorts = new Dictionary<MinerBaseType, Dictionary<string, Dictionary<AlgorithmType, List<int>>>>();
        public static List<int> AllReservedPorts = new List<int>();

        public static void Init() {
            ExtraLaunchParameters.InitializePackages();
            InitMinerReservedPortsFile();
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
            var AMDCodenames = new List<string>() { "Hawaii", "Pitcairn", "Tahiti" };
            var AMDOptimizations = new List<bool>() { true, false };
            var CPUExtensions = new List<CPUExtensionType>() {
                CPUExtensionType.AVX2_AES,
                CPUExtensionType.AVX2,
                CPUExtensionType.AVX_AES,
                CPUExtensionType.AVX,
                CPUExtensionType.AES,
                CPUExtensionType.SSE2
            };
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
                                // CPU case
                                if (MinerBaseType.cpuminer == minerBaseType) {
                                    foreach (var algo in algos) {
                                        foreach (var cpuExt in CPUExtensions) {
                                            var algoType = algo.NiceHashID;
                                            var path = MinerPaths.GetPathFor(minerBaseType, algoType, devGroupType, "", false, cpuExt);
                                            if (path != MinerPaths.NONE && MinerReservedPorts[minerBaseType].ContainsKey(path) == false) {
                                                MinerReservedPorts[minerBaseType][path] = new Dictionary<AlgorithmType, List<int>>();
                                            }
                                            if (MinerReservedPorts[minerBaseType][path] != null && MinerReservedPorts[minerBaseType][path].ContainsKey(algoType) == false) {
                                                MinerReservedPorts[minerBaseType][path][algoType] = new List<int>();
                                            }
                                        }
                                    }
                                } else if (MinerBaseType.sgminer == minerBaseType) {
                                    foreach (var algo in algos) {
                                        foreach (var isOptimized in AMDOptimizations) {
                                            foreach (var codename in AMDCodenames) {
                                                var algoType = algo.NiceHashID;
                                                var path = MinerPaths.GetPathFor(minerBaseType, algoType, devGroupType, codename, isOptimized, CPUExtensionType.Automatic);
                                                if (path != MinerPaths.NONE && MinerReservedPorts[minerBaseType].ContainsKey(path) == false) {
                                                    MinerReservedPorts[minerBaseType][path] = new Dictionary<AlgorithmType, List<int>>();
                                                }
                                                if (MinerReservedPorts[minerBaseType][path] != null && MinerReservedPorts[minerBaseType][path].ContainsKey(algoType) == false) {
                                                    MinerReservedPorts[minerBaseType][path][algoType] = new List<int>();
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    foreach (var algo in algos) {
                                        var algoType = algo.NiceHashID;
                                        var path = MinerPaths.GetPathFor(minerBaseType, algoType, devGroupType, "", false, CPUExtensionType.Automatic);
                                        if (path != MinerPaths.NONE && MinerReservedPorts[minerBaseType].ContainsKey(path) == false) {
                                            MinerReservedPorts[minerBaseType][path] = new Dictionary<AlgorithmType, List<int>>();
                                        }
                                        if (MinerReservedPorts[minerBaseType][path] != null && MinerReservedPorts[minerBaseType][path].ContainsKey(algoType) == false) {
                                            MinerReservedPorts[minerBaseType][path][algoType] = new List<int>();
                                        }
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

    }
}
