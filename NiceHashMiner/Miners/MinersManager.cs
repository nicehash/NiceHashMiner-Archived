using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NiceHashMiner.Miners {
    // typedefs
    using DeviceSubsetList = List<SortedSet<string>>;
    using PerDeviceProifitDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    using PerDeviceSpeedDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;

    using GroupedDevices = SortedSet<string>;
    using AllGroupedDevices = List<SortedSet<string>>;
    
    

    public partial class MinersManager : BaseLazySingleton<MinersManager> {

        // temporary varibales for current session
        PerDeviceSpeedDictionary _perDeviceSpeedDictionary;
        Dictionary<string, int> _enabledDeviceCount;
        string _miningLocation;
        string _worker;

        readonly DeviceGroupType[] _nvidiaTypes = new DeviceGroupType[] {
            DeviceGroupType.NVIDIA_2_1,
            DeviceGroupType.NVIDIA_3_x,
            DeviceGroupType.NVIDIA_5_x
        };

        

        // GroupDevices hash code doesn't work correctly use string instead
        //Dictionary<GroupedDevices, GroupMiners> _groupedDevicesMiners;
        Dictionary<string, GroupMiners> _groupedDevicesMiners;
        List<ComputeDevice> _enabledUniqueDevices;
        AllGroupedDevices _previousAllGroupedDevices;
        AllGroupedDevices _currentAllGroupedDevices;


        // we save cpu miners string group name
        Dictionary<string, cpuminer> _cpuMiners = new Dictionary<string, cpuminer>();

        protected MinersManager() {
        }

        public void AddCpuMiner(cpuminer miner, int deviceID, string deviceName) {
            _cpuMiners.Add(miner.MinerDeviceName, miner);
        }

        public void StopAllMiners() {
            if (_groupedDevicesMiners != null) {
                foreach (var kv in _groupedDevicesMiners) {
                    kv.Value.End();
                }
            }
        }


        public string GetActiveMinersGroup() {
            string ActiveMinersGroup = "";

            // TODO enable, change a set of devices CPU, NVIDIA, AMD
            //foreach (var kvp in _allMiners) {
            //    Miner m = kvp.Value;
            //    if (m.IsRunning) {
            //        ActiveMinersGroup += m.MinerDeviceName + "/";
            //    }
            //}

            return ActiveMinersGroup;
        }

        public static Miner GetCpuMiner(string groupName) {
            if (Instance._cpuMiners.Count > 0) {
                return Instance._cpuMiners[groupName];
            }
            return null;
        }
        // create miner creates new miners, except cpuminer, those are saves and called from GetCpuMiner()
        public static Miner CreateMiner(DeviceGroupType deviceGroupType, AlgorithmType algorithmType) {
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
                        return new ccminer_sm21(false);
                    case DeviceGroupType.NVIDIA_3_x:
                        return new ccminer_sm3x(false);
                    case DeviceGroupType.NVIDIA_5_x:
                        return new ccminer_sm5x(false);
                }
            }
            
            return null;
        }

        private string CalcGroupedDevicesKey(GroupedDevices group) {
            return string.Join(", ", group);
        }

        public double GetTotalRate() {
            double TotalRate = 0;

            foreach (var group in _currentAllGroupedDevices) {
                var groupMiners = _groupedDevicesMiners[CalcGroupedDevicesKey(group)];
                TotalRate += groupMiners.CurrentWorkingMiner.CurrentRate;
            }

            return TotalRate;
        }

        public void StartInitialize() {
            _perDeviceSpeedDictionary = GetEnabledDeviceTypeSpeeds();
            //_groupedDevicesMiners = new Dictionary<GroupedDevices, GroupMiners>();
            _groupedDevicesMiners = new Dictionary<string, GroupMiners>();
            _enabledUniqueDevices = new List<ComputeDevice>();
            _currentAllGroupedDevices = new AllGroupedDevices();

            foreach (var cdev in ComputeDevice.UniqueAvaliableDevices) {
                if (cdev.Enabled) {
                    _enabledUniqueDevices.Add(cdev);
                    // check if in CPU group and add the saved CPU miners
                    if (cdev.DeviceGroupType == DeviceGroupType.CPU) {
                        GroupedDevices gdevs = new GroupedDevices();
                        gdevs.Add(cdev.UUID);
                        cpuminer miner = _cpuMiners[cdev.Group];
                        CpuGroupMiner cpuGroupMiner = new CpuGroupMiner(gdevs, miner);
                        _groupedDevicesMiners.Add(CalcGroupedDevicesKey(gdevs), cpuGroupMiner);
                    }
                }
            }
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

        #region Groupping logic
        private bool IsAlgorithmSettingsSame(Algorithm a, Algorithm b) {
            return a.NiceHashID == b.NiceHashID
                && string.IsNullOrEmpty(a.UsePassword) == string.IsNullOrEmpty(b.UsePassword)
                && string.IsNullOrEmpty(a.ExtraLaunchParameters) == string.IsNullOrEmpty(b.ExtraLaunchParameters);
        }

        private bool IsNvidiaDevice(ComputeDevice a) {
            foreach (var type in _nvidiaTypes) {
                if (a.DeviceGroupType == type) return true;
            }
            return false;
        }

        private bool IsNvidiaAndDagger(ComputeDevice a, ComputeDevice b) {
            return a.MostProfitableAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto
                && IsAlgorithmSettingsSame(a.MostProfitableAlgorithm, b.MostProfitableAlgorithm)
                && IsNvidiaDevice(a) && IsNvidiaDevice(b);
        }

        private bool IsNotCpuGroups(ComputeDevice a, ComputeDevice b) {
            return a.DeviceGroupType != DeviceGroupType.CPU && b.DeviceGroupType != DeviceGroupType.CPU;
        }

        // we don't want to group CPU devices
        private bool IsGroupAndAlgorithmSame(ComputeDevice a, ComputeDevice b) {
            return IsNotCpuGroups(a,b) && a.DeviceGroupType == b.DeviceGroupType
                && IsAlgorithmSettingsSame(a.MostProfitableAlgorithm, b.MostProfitableAlgorithm);
        }
        #endregion //Groupping logic

        static int stepCheck = 5;
        /// <summary>
        /// SwichMostProfitable should check the best combination for most profit.
        /// Calculate profit for each supported algorithm per device group.
        /// Build from ground up compatible devices and algorithms.
        /// See #region Groupping logic
        /// Device groups are CPU, AMD_OpenCL and NVIDIA CUDA SM.x.x.
        /// NVIDIA SMx.x should be paired separately except for daggerhashimoto.
        /// </summary>
        /// <param name="NiceHashData"></param>
        public void SwichMostProfitableGroupUpMethod(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData, string Worker) {
            var devProfits = GetEnabledDeviceProifitDictionary(_perDeviceSpeedDictionary, NiceHashData);

            // calculate most profitable algorithm per unique device
            foreach (var cdev in _enabledUniqueDevices) {
                var curDevProfits = devProfits[cdev.Name];
                double maxProfit = double.MinValue;
                AlgorithmType maxAlgorithmTypeKey = AlgorithmType.NONE;
                var algorithmSettings = DeviceBenchmarkConfigManager.Instance.GetConfig(cdev.Name).AlgorithmSettings;

                foreach (var kvpTypeProfit in curDevProfits) {
                    if (!algorithmSettings[kvpTypeProfit.Key].Skip && kvpTypeProfit.Value > 0.0d && maxProfit < kvpTypeProfit.Value) {
                        maxProfit = kvpTypeProfit.Value;
                        maxAlgorithmTypeKey = kvpTypeProfit.Key;
                    }
                }
                if(maxAlgorithmTypeKey == AlgorithmType.NONE) {
                    cdev.MostProfitableAlgorithm = null;
                } else {
                    cdev.MostProfitableAlgorithm
                        = algorithmSettings[maxAlgorithmTypeKey];
                }
            }
            // group devices with same supported algorithms
            _previousAllGroupedDevices = _currentAllGroupedDevices;
            _currentAllGroupedDevices = new AllGroupedDevices();
            Dictionary<GroupedDevices, Algorithm> newGroupAndAlgorithm = new Dictionary<GroupedDevices,Algorithm>();
            for (int first = 0; first < _enabledUniqueDevices.Count; ++first) {
                var firstDev = _enabledUniqueDevices[first];
                // skip if no algorithm is profitable
                if (firstDev.MostProfitableAlgorithm == null) {
                    // TODO maybe print that algorithm is missing should not come to this
                    continue;
                }
                // check if is in group
                bool isInGroup = false;
                foreach (var groupedDevices in _currentAllGroupedDevices) {
                    if (groupedDevices.Contains(firstDev.UUID)) {
                        isInGroup = true;
                        break;
                    }
                }
                if (isInGroup) continue;

                var newGroup = new GroupedDevices();
                newGroup.Add(firstDev.UUID);
                for (int second = first + 1; second < _enabledUniqueDevices.Count; ++second) {
                    var secondDev = _enabledUniqueDevices[second];
                    // check if we should group
                    if(IsNvidiaAndDagger(firstDev, secondDev)
                        || IsGroupAndAlgorithmSame(firstDev, secondDev)) {
                        newGroup.Add(secondDev.UUID);
                    }
                }

                _currentAllGroupedDevices.Add(newGroup);
                newGroupAndAlgorithm.Add(newGroup, firstDev.MostProfitableAlgorithm);
            }

            // stop groupes that aren't in current group devices
            foreach (var curPrevGroup in _previousAllGroupedDevices) {
                var curPrevGroupKey = CalcGroupedDevicesKey(curPrevGroup);
                bool contains = false;
                foreach (var curCheckGroup in _currentAllGroupedDevices) {
                    var curCheckGroupKey = CalcGroupedDevicesKey(curCheckGroup);
                    if (curPrevGroupKey == curCheckGroupKey) {
                        contains = true;
                        break;
                    }
                }
                if (!contains) {
                    _groupedDevicesMiners[curPrevGroupKey].Stop();
                }
            }
            // switch to newGroupAndAlgorithm most profitable algorithm
            foreach (var kvpGroupAlgorithm in  newGroupAndAlgorithm) {
                var group = kvpGroupAlgorithm.Key;
                var algorithm = kvpGroupAlgorithm.Value;

                GroupMiners currentGroupMiners;
                // try find if it doesn't exist create new
                string groupStringKey = CalcGroupedDevicesKey(group);
                if (!_groupedDevicesMiners.TryGetValue(groupStringKey, out currentGroupMiners)) {
                    currentGroupMiners = new GroupMiners(group);
                    _groupedDevicesMiners.Add(groupStringKey, currentGroupMiners);
                }
                currentGroupMiners.StartAlgorihtm(algorithm, Worker);
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

            foreach (var group in _currentAllGroupedDevices) {
                var groupMiners = _groupedDevicesMiners[CalcGroupedDevicesKey(group)];
                Miner m = groupMiners.CurrentWorkingMiner;

                // skip if not running
                if (!m.IsRunning) continue;

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
                } else {
                    Helpers.ConsolePrint("GetSummary", String.Format("Devices {0}\tAlgorithm : {1}\tSpeed : {2}", groupMiners.DevicesInfoString, AD.AlgorithmName, AD.Speed));
                    m.StartingUpDelay = false;
                }

                if (NiceHashData != null)
                    m.CurrentRate = NiceHashData[AD.AlgorithmID].paying * AD.Speed * 0.000000001;
                else
                    m.CurrentRate = 0;

                if (m is cpuminer) {
                    CPUAlgoName = AD.AlgorithmName;
                    CPUTotalSpeed += AD.Speed;
                    CPUTotalRate += m.CurrentRate;
                } else if (m is ccminer_sm21) {
                    //SetNVIDIAtp21Stats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                } else if (m is ccminer_sm3x) {
                    //SetNVIDIAtpStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                } else if (m is ccminer_sm5x) {
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
