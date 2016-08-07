using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    using System.Diagnostics;
    // typedefs
    using DeviceSubsetList = List<SortedSet<string>>;
    using PerDeviceProifitDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    using PerDeviceSpeedDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;

    public partial class MinersManager : BaseLazySingleton<MinersManager> {

        // TODO make members private
        public class MinerKey {
            // TODO change id to uuid
            public string[] DeviceUUIDs;
            public string Group;
            //public string DeviceName;
            public DeviceGroupType DeviceGroupType;
        }

        // temporary varibales for current session
        List<DeviceGroupSettupManager> _deviceGroupSettupManager;
        PerDeviceSpeedDictionary _perDeviceSpeedDictionary;
        Dictionary<string, int> _enabledDeviceCount;
        string _miningLocation;


        // we save cpu miners
        Dictionary<MinerKey, cpuminer> _cpuMiners = new Dictionary<MinerKey,cpuminer>();

        // gpu miner are made as we go
        Dictionary<MinerKey, ccminer> _ccminerMiners;


        Dictionary<MinerKey, Miner> _allMiners = new Dictionary<MinerKey,Miner>();

        protected MinersManager() {
        }


        // TODO CPU is broken
        public void AddCpuMiner(cpuminer miner, int deviceID, string deviceName) {
            MinerKey key = new MinerKey() {
                // TODO fix this
                //DeviceID = new int[] { deviceID },
                Group = miner.MinerDeviceName,
                //DeviceName = deviceName,
                DeviceGroupType = DeviceGroupType.CPU
            };
            _cpuMiners.Add(key, miner);
            // add to all miners
            _allMiners.Add(key, miner);
        }

        public void StopAllMiners() {
            foreach (var kv in _allMiners) {
                Miner m = kv.Value;
                m.Stop(false);
                m.IsRunning = false;
                m.CurrentAlgo = AlgorithmType.NONE;
                m.CurrentRate = 0;
            }
            // clear this sessin
            _allMiners.Clear();
        }


        public string GetActiveMinersGroup() {
            string ActiveMinersGroup = "";

            foreach (var kvp in _allMiners) {
                Miner m = kvp.Value;
                if (m.IsRunning) {
                    ActiveMinersGroup += m.MinerDeviceName + "/";
                }
            }

            return ActiveMinersGroup;
        }

        // TODO check how will it work for double CPU settup
        // TODO add ethimer for dagger separation
        public Miner CreateMiner(DeviceGroupType deviceGroupType, AlgorithmType algorithmType) {
            
            // return first cpuminer
            if (DeviceGroupType.CPU == deviceGroupType && _cpuMiners.Count > 0) {
                return _cpuMiners[_cpuMiners.Keys.First()];
            }

            if (AlgorithmType.DaggerHashimoto == algorithmType) {
                if (DeviceGroupType.AMD_OpenCL == deviceGroupType) {
                    return new MinerEtherumOCL();
                } else {
                    return new MinerEtherumCUDA();
                }
            } else {
                switch (deviceGroupType) {
                    case DeviceGroupType.AMD_OpenCL:
                        return new sgminer(false);
                    case DeviceGroupType.NVIDIA_2_1:
                        return new ccminer_tpruvot_sm21(false);
                    case DeviceGroupType.NVIDIA_3_x:
                        return new ccminer_tpruvot(false);
                    case DeviceGroupType.NVIDIA_5_x:
                        return new ccminer_sp(false);
                }
            }
            

            return null;
        }

        public double GetTotalRate() {
            double TotalRate = 0;
            // TODO 
            foreach (var kvp in _allMiners) {
                TotalRate += kvp.Value.CurrentRate;
            }

            return TotalRate;
        }

        public void StartInitialize() {
            DeviceGroupType[] groupTypes = new DeviceGroupType[] {
                DeviceGroupType.CPU,
                DeviceGroupType.AMD_OpenCL,
                DeviceGroupType.NVIDIA_2_1,
                DeviceGroupType.NVIDIA_3_x,
                DeviceGroupType.NVIDIA_5_x
            };
            _deviceGroupSettupManager = new List<DeviceGroupSettupManager>();
            foreach (var group in groupTypes) {
                var uniqueCDevUUIDs = ComputeDevice.GetUniqueEnabledDevicesUUIDsForGroup(group);
                if (uniqueCDevUUIDs.Count > 0) {
                    _deviceGroupSettupManager.Add(new DeviceGroupSettupManager(group, uniqueCDevUUIDs));
                }
            }
            _perDeviceSpeedDictionary = GetEnabledDeviceTypeSpeeds();
        }

        /// <summary>
        /// GetEnabledDeviceTypeBenchmarks calculates currently enabled ComputeDevice benchmark speeds.
        /// If there are more cards of the same model it multiplies the speeds by it's count
        /// </summary>
        /// <returns></returns>
        PerDeviceSpeedDictionary GetEnabledDeviceTypeSpeeds() {
            PerDeviceSpeedDictionary perDeviceTypeBenchmarks = new PerDeviceSpeedDictionary();

            // get enabled devices and their count
            _enabledDeviceCount = new Dictionary<string, int>();
            foreach (var enabledDevice in ComputeDevice.GetEnabledDevices()) {
                if (_enabledDeviceCount.ContainsKey(enabledDevice.Name)) {
                    _enabledDeviceCount[enabledDevice.Name]++;
                } else {
                    _enabledDeviceCount.Add(enabledDevice.Name, 1);
                }
            }

            // calculate benchmarks
            foreach (var cdevKvp in _enabledDeviceCount) {
                var cdevName = cdevKvp.Key;
                var cdevCount = cdevKvp.Value;
                Dictionary<AlgorithmType, double> cumulativeSpeeds = new Dictionary<AlgorithmType, double>();
                var deviceConfig = DeviceBenchmarkConfigManager.Instance.GetConfig(cdevName);

                foreach (var kvp in deviceConfig.AlgorithmSettings) {
                    var key = kvp.Key;
                    var algorithm = kvp.Value;
                    // TODO what is the constant at the end?
                    if (algorithm.Skip) {
                        // for now set to negative value as not profitable
                        cumulativeSpeeds.Add(key, -1);
                    } else {
                        cumulativeSpeeds.Add(key, algorithm.BenchmarkSpeed * cdevCount);
                    }
                }
                perDeviceTypeBenchmarks.Add(cdevName, cumulativeSpeeds);
            }

            return perDeviceTypeBenchmarks;
        }


        PerDeviceProifitDictionary GetEnabledDeviceProifitDictionary(PerDeviceSpeedDictionary speedDict, Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            PerDeviceProifitDictionary profitDict = new PerDeviceProifitDictionary();

            foreach (var nameBenchKvp in speedDict) {
                var deviceName = nameBenchKvp.Key;
                var curDevProfits = new Dictionary<AlgorithmType, double>();
                foreach (var algoSpeedKvp in nameBenchKvp.Value) {
                    if (algoSpeedKvp.Value < 0) {
                        // if disabled make unprofitable
                        curDevProfits.Add(algoSpeedKvp.Key, -1000000);
                    } else {
                        // TODO what is the constant at the end?
                        curDevProfits.Add(algoSpeedKvp.Key, algoSpeedKvp.Value * NiceHashData[algoSpeedKvp.Key].paying * 0.000000001);
                    }
                }
                // add profits
                profitDict.Add(deviceName, curDevProfits);
            }

            return profitDict;
        }

        /// <summary>
        /// SwichMostProfitable should check the best combination for most profit.
        /// Calculate profit for each supported algorithm per device group.
        /// Get max profit GroupSetups and run miners
        /// Device groups are CPU, AMD_OpenCL and NVIDIA CUDA SM.x.x.
        /// NVIDIA SMx.x should be paired separately except for daggerhashimoto.
        /// </summary>
        /// <param name="NiceHashData"></param>
        public void SwichMostProfitable(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData, string Worker) {
            var devProfits = GetEnabledDeviceProifitDictionary(_perDeviceSpeedDictionary, NiceHashData);
            
            // calculate most profitable
            foreach (var groupSettupProfit in _deviceGroupSettupManager) {
                groupSettupProfit.CalculateMostProfitable(devProfits);
            }

            // start/stop miners
            foreach (var groupSettupProfit in _deviceGroupSettupManager) {
                var groupId = groupSettupProfit.GroupID;
                // TODO ethminer refactor after ethminer implementation
                // TODO group profitability check
                if (groupSettupProfit.IsChange) {
                    // check if device settup change
                    if (groupSettupProfit.IsMostProfitableSettupChange) {
                        Helpers.ConsolePrint("IsMostProfitableSettupChange", "Profitable change group");
                        // stop group settups that are different
                        foreach (var groupSettup in groupSettupProfit.LastMostProfitable.PerGroupProfit) {
                            if (groupSettupProfit.MostProfitable.PerGroupProfit.Contains(groupSettup) == false) {
                                Helpers.ConsolePrint("IsMostProfitableSettupChange",
                                    "Stopping: " + groupSettup.DevicesInfoString
                                    + " Algorithm: " + AlgorithmNiceHashNames.GetName(groupSettup.MostProfitAlgorithmType));
                                // TODO Nvidia dagger special case

                                // stop this settup (no need to check)
                                _allMiners[groupSettup.MinerKey].Stop(false);
                                // wait 0.5 seconds before going on
                                System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                            }
                        }
                    }
                    // switching
                    foreach (var groupSettup in groupSettupProfit.MostProfitable.PerGroupProfit) {
                        Miner m = null;
                        // if miner doesn't exist create it
                        if (_allMiners.TryGetValue(groupSettup.MinerKey, out m) == false) {
                            m = CreateMiner(groupId, groupSettup.MostProfitAlgorithmType);
                            m.SetCDevs(groupSettup.MinerKey.DeviceUUIDs);
                            _allMiners.Add(groupSettup.MinerKey, m);
                        }
                        if (m.CurrentAlgo != groupSettup.MostProfitAlgorithmType) {
                            Helpers.ConsolePrint(m.MinerDeviceName, "Devices: "
                                + groupSettup.DevicesInfoString
                                + ". Switching to most profitable algorithm: " + AlgorithmNiceHashNames.GetName(groupSettup.MostProfitAlgorithmType));

                            if (m.CurrentAlgo != AlgorithmType.NONE && m.CurrentAlgo != AlgorithmType.INVALID) {
                                m.Stop(true);
                                // wait 0.5 seconds before going on
                                System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                            }

                            m.CurrentAlgo = groupSettup.MostProfitAlgorithmType;
                            var MaxProfitKey = groupSettup.MostProfitAlgorithmType;

                            m.Start(groupSettup.MostProfitAlgorithm,
                                "stratum+tcp://"
                                + Globals.NiceHashData[MaxProfitKey].name
                                // TODO fix this combo box things
                                + "." + Globals.MiningLocation[/*comboBoxLocation.SelectedIndex*/ 1]
                                + ".nicehash.com:"
                                + Globals.NiceHashData[MaxProfitKey].port, Worker);
                        }
                    }
                }
            }
            
        }

        public void MinerStatsCheck(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            // TODO replace with new switching logic and set gui comunication interfaces
            string CPUAlgoName = "";
            double CPUTotalSpeed = 0;
            double CPUTotalRate = 0;

            //// Reset all stats
            //SetCPUStats("", 0, 0);
            //SetNVIDIAtp21Stats("", 0, 0);
            //SetNVIDIAspStats("", 0, 0);
            //SetNVIDIAtpStats("", 0, 0);
            //SetAMDOpenCLStats("", 0, 0);

            foreach (var kvp in _allMiners) {
                Miner m = kvp.Value;
                // skip if not running
                if (!m.IsRunning) continue;

                if (m is cpuminer && m.IsCurrentAlgo(AlgorithmType.Hodl)) {
                    string pname = m.Path.Split('\\')[2];
                    pname = pname.Substring(0, pname.Length - 4);

                    Process[] processes = Process.GetProcessesByName(pname);

                    if (processes.Length < CPUID.GetPhysicalProcessorCount())
                        m.Restart();

                    AlgorithmType algoIndex = AlgorithmType.Hodl; // m.GetAlgoIndex("hodl");
                    CPUAlgoName = "hodl";
                    //CPUTotalSpeed = m.SupportedAlgorithms[algoIndex].BenchmarkSpeed;
                    //CPUTotalRate = Globals.NiceHashData[m.SupportedAlgorithms[algoIndex].NiceHashID].paying * CPUTotalSpeed * 0.000000001;

                    continue;
                }

                APIData AD = m.GetSummary();
                if (AD == null) {
                    Helpers.ConsolePrint(m.MinerDeviceName, "GetSummary returned null..");

                    // Make sure sgminer has time to start
                    // properly on slow CPU system
                    if (m.StartingUpDelay && m.NumRetries > 0) {
                        m.NumRetries--;
                        if (m.NumRetries == 0) m.StartingUpDelay = false;
                        Helpers.ConsolePrint(m.MinerDeviceName, "NumRetries: " + m.NumRetries);
                        continue;
                    }

                    // API is inaccessible, try to restart miner
                    m.Restart();

                    continue;
                } else
                    m.StartingUpDelay = false;

                if (NiceHashData != null)
                    m.CurrentRate = NiceHashData[AD.AlgorithmID].paying * AD.Speed * 0.000000001;
                else
                    m.CurrentRate = 0;

                if (m is cpuminer) {
                    CPUAlgoName = AD.AlgorithmName;
                    CPUTotalSpeed += AD.Speed;
                    CPUTotalRate += m.CurrentRate;
                } else if (m is ccminer_tpruvot_sm21) {
                    //SetNVIDIAtp21Stats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                } else if (m is ccminer_tpruvot) {
                    //SetNVIDIAtpStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                } else if (m is ccminer_sp) {
                    //SetNVIDIAspStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                } else if (m is sgminer) {
                    //SetAMDOpenCLStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                }
            }
            if (CPUAlgoName != null && CPUAlgoName.Length > 0) {
                //SetCPUStats(CPUAlgoName, CPUTotalSpeed, CPUTotalRate);
            }
        }


    }
}
