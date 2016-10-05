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

using Timer = System.Timers.Timer;
using System.Timers;

namespace NiceHashMiner.Miners {
    // typedefs
    using DeviceSubsetList = List<SortedSet<string>>;
    using PerDeviceProifitDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    using PerDeviceSpeedDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;

    using GroupedDevices = SortedSet<string>;
    using AllGroupedDevices = List<SortedSet<string>>;
    using NiceHashMiner.Interfaces;
    
    

    public partial class MinersManager : BaseLazySingleton<MinersManager> {

        // temporary varibales for current session
        PerDeviceSpeedDictionary _perDeviceSpeedDictionary;
        string _miningLocation;
        string _worker;
        string _btcAdress;

        string _workerBtcStringWorker;

        readonly DeviceGroupType[] _nvidiaTypes = new DeviceGroupType[] {
            DeviceGroupType.NVIDIA_2_1,
            DeviceGroupType.NVIDIA_3_x,
            DeviceGroupType.NVIDIA_5_x,
            DeviceGroupType.NVIDIA_6_x
        };

        readonly DeviceGroupType[] _gpuTypes = new DeviceGroupType[] {
            DeviceGroupType.AMD_OpenCL,
            DeviceGroupType.NVIDIA_2_1,
            DeviceGroupType.NVIDIA_3_x,
            DeviceGroupType.NVIDIA_5_x,
            DeviceGroupType.NVIDIA_6_x
        };

        // these miners are just used for binary path checking
        public readonly Dictionary<DeviceGroupType, Miner> MinerPathChecker;

        // ccminers intensity checking
        public readonly Dictionary<string, Tuple<double, double>> CCMinersIntensitiesBoundries = new Dictionary<string, Tuple<double, double>>() {
            { MinerPaths.ccminer_decred,     Tuple.Create(8.0, 25.0) },
            { MinerPaths.ccminer_nanashi,    Tuple.Create(8.0, 25.0) },
            { MinerPaths.ccminer_tpruvot,    Tuple.Create(8.0, 25.0) },
            { MinerPaths.ccminer_neoscrypt,  Tuple.Create(8.0, 31.0) },
            { MinerPaths.ccminer_sp,         Tuple.Create(8.0, 31.0) },
        };

        // GroupDevices hash code doesn't work correctly use string instead
        //Dictionary<GroupedDevices, GroupMiners> _groupedDevicesMiners;
        Dictionary<string, GroupMiners> _groupedDevicesMiners;
        List<ComputeDevice> _enabledDevices;
        AllGroupedDevices _previousAllGroupedDevices;
        AllGroupedDevices _currentAllGroupedDevices;

        IMainFormRatesComunication _mainFormRatesComunication;

        private Timer _preventSleepTimer;
        // check internet connection 
        private Timer _internetCheckTimer;

        // we save cpu miners string group name
        Dictionary<string, cpuminer> _cpuMiners = new Dictionary<string, cpuminer>();

        private bool IsProfitable = true;
        private bool IsConnectedToInternet = true;

        readonly string TAG;

        readonly string DOUBLE_FORMAT = "F12";

        private bool IsCurrentlyIdle = true;

        protected MinersManager() {
            TAG = this.GetType().Name;
            _preventSleepTimer = new Timer();
            _preventSleepTimer.Elapsed += PreventSleepTimer_Tick;
             // sleep time is minimal 1 minute
            _preventSleepTimer.Interval = 20 * 1000; // leave this interval, it works

            // set internet checking
            _internetCheckTimer = new Timer();
            _internetCheckTimer.Elapsed += InternetCheckTimer_Tick;
            _internetCheckTimer.Interval = 1000 * 30 * 1; // every minute or 5?? // 1000 * 60 * 1

            // path checker
            Helpers.ConsolePrint(TAG, "Creating MinerPathChecker miners");
            MinerPathChecker = new Dictionary<DeviceGroupType, Miner>();
            foreach (var gpuGroup in _gpuTypes) {
                MinerPathChecker.Add(gpuGroup, CreateMiner(gpuGroup, AlgorithmType.NONE));
            }
        }

        private void InternetCheckTimer_Tick(object sender, EventArgs e) {
            if (ConfigManager.Instance.GeneralConfig.ContinueMiningIfNoInternetAccess == false) {
                IsConnectedToInternet = Helpers.IsConnectedToInternet();
            }
        }

        private void PreventSleepTimer_Tick(object sender, ElapsedEventArgs e) {
            // when mining keep system awake, prevent sleep
            Helpers.PreventSleep();
        }

        public void AddCpuMiner(cpuminer miner, int deviceID, string deviceName) {
            _cpuMiners.Add(miner.MinerDeviceName, miner);
        }

        public void StopAllMiners() {
            IsCurrentlyIdle = true;
            if (_groupedDevicesMiners != null) {
                foreach (var kv in _groupedDevicesMiners) {
                    kv.Value.End();
                }
            }
            if (_mainFormRatesComunication != null) {
                _mainFormRatesComunication.ClearRatesALL();
            }

            // restroe/enable sleep
            _preventSleepTimer.Stop();
            _internetCheckTimer.Stop();
            Helpers.AllowMonitorPowerdownAndSleep();
        }

        public void StopAllMinersNonProfitable() {
            IsCurrentlyIdle = true;
            if (_groupedDevicesMiners != null) {
                foreach (var kv in _groupedDevicesMiners) {
                    kv.Value.End();
                }
            }
            if (_mainFormRatesComunication != null) {
                _mainFormRatesComunication.ClearRates(-1);
            }
        }

        public string GetActiveMinersGroup() {
            if (IsCurrentlyIdle) {
                return "IDLE";
            }

            string ActiveMinersGroup = "";

            //get unique miner groups like CPU, NVIDIA, AMD,...
            HashSet<string> UniqueMinerGroups = new HashSet<string>();
            foreach (var curDevice in ComputeDevice.AllAvaliableDevices) {
                if (curDevice.Enabled) {
                    UniqueMinerGroups.Add(curDevice.DeviceGroupString);
                }
            }
            if (UniqueMinerGroups.Count > 0 && IsProfitable) {
                ActiveMinersGroup = string.Join("/", UniqueMinerGroups);
            }

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
                        return new sgminer();
                    case DeviceGroupType.NVIDIA_2_1:
                        return new ccminer_sm21();
                    case DeviceGroupType.NVIDIA_3_x:
                        return new ccminer_sm3x();
                    case DeviceGroupType.NVIDIA_5_x:
                        return new ccminer_sm5x();
                    case DeviceGroupType.NVIDIA_6_x:
                        return new ccminer_sm6x();
                }
            }
            
            return null;
        }

        private string CalcGroupedDevicesKey(GroupedDevices group) {
            return string.Join(", ", group);
        }

        public double GetTotalRate() {
            double TotalRate = 0;

            if (_currentAllGroupedDevices != null) {
                foreach (var group in _currentAllGroupedDevices) {
                    var groupMiners = _groupedDevicesMiners[CalcGroupedDevicesKey(group)];
                    TotalRate += groupMiners.CurrentWorkingMiner.CurrentRate;
                }
            }

            return TotalRate;
        }

        public bool StartInitialize(IMainFormRatesComunication mainFormRatesComunication,
            string miningLocation, string worker, string btcAdress) {
            _mainFormRatesComunication = mainFormRatesComunication;
            _miningLocation = miningLocation;
            _worker = worker;
            _btcAdress = btcAdress;

            if (_worker.Length > 0)
                _workerBtcStringWorker = _btcAdress + "." + _worker;
            else
                _workerBtcStringWorker = _btcAdress;

            _perDeviceSpeedDictionary = GetEnabledDeviceTypeSpeeds();
            //_groupedDevicesMiners = new Dictionary<GroupedDevices, GroupMiners>();
            _groupedDevicesMiners = new Dictionary<string, GroupMiners>();
            _enabledDevices = new List<ComputeDevice>();
            _currentAllGroupedDevices = new AllGroupedDevices();

            // assume profitable
            IsProfitable = true;
            // assume we have internet
            IsConnectedToInternet = true;


            // this checks if there are enabled devices and enabled algorithms
            bool isMiningEnabled = false;

            foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                if (cdev.Enabled) {
                    _enabledDevices.Add(cdev);
                    // check if in CPU group and add the saved CPU miners
                    if (cdev.DeviceGroupType == DeviceGroupType.CPU) {
                        GroupedDevices gdevs = new GroupedDevices();
                        gdevs.Add(cdev.UUID);
                        cpuminer miner = _cpuMiners[cdev.Group];
                        CpuGroupMiner cpuGroupMiner = new CpuGroupMiner(gdevs, miner);
                        _groupedDevicesMiners.Add(CalcGroupedDevicesKey(gdevs), cpuGroupMiner);
                    }
                    // check if any algorithm enabled
                    if(!isMiningEnabled) {
                        foreach (var algorithm in cdev.DeviceBenchmarkConfig.AlgorithmSettings) {
                            if (!algorithm.Value.Skip) {
                                isMiningEnabled = true;
                                break;
                            }
                        }
                    }

                }
            }

            if (isMiningEnabled) {
                _preventSleepTimer.Start();
                _internetCheckTimer.Start();
            }

            IsCurrentlyIdle = !isMiningEnabled;

            return isMiningEnabled;
        }

        /// <summary>
        /// GetEnabledDeviceTypeBenchmarks calculates currently enabled ComputeDevice benchmark speeds.
        /// If there are more cards of the same model it multiplies the speeds by it's count
        /// </summary>
        /// <returns></returns>
        PerDeviceSpeedDictionary GetEnabledDeviceTypeSpeeds() {
            PerDeviceSpeedDictionary perDeviceTypeBenchmarks = new PerDeviceSpeedDictionary();

            // init algorithms count 0
            foreach (var curCDev in ComputeDevice.AllAvaliableDevices) {
                if (curCDev.Enabled) {
                    Dictionary<AlgorithmType, double> cumulativeSpeeds = new Dictionary<AlgorithmType, double>();
                    var deviceConfig = curCDev.DeviceBenchmarkConfig;
                    foreach (var kvp in deviceConfig.AlgorithmSettings) {
                        var key = kvp.Key;
                        cumulativeSpeeds[key] = 0;
                    }
                    perDeviceTypeBenchmarks[curCDev.UUID] = cumulativeSpeeds;
                }
            }
            // set enabled algorithm count per device counts
            foreach (var curCDev in ComputeDevice.AllAvaliableDevices) {
                if (curCDev.Enabled) {
                    Dictionary<AlgorithmType, double> cumulativeSpeeds = perDeviceTypeBenchmarks[curCDev.UUID];
                    var deviceConfig = curCDev.DeviceBenchmarkConfig;
                    foreach (var kvp in deviceConfig.AlgorithmSettings) {
                        var key = kvp.Key;
                        var algorithm = kvp.Value;
                        // check dagger RAM SIZE
                        if (algorithm.Skip
                            || (algorithm.NiceHashID == AlgorithmType.DaggerHashimoto && !curCDev.IsEtherumCapale)) {
                            // for now set to negative value as not profitable
                            cumulativeSpeeds[key]--;
                        } else {
                            cumulativeSpeeds[key]++;
                        }
                    }
                }
            }
            // calculate benchmarks
            foreach (var curCDev in ComputeDevice.AllAvaliableDevices) {
                if (curCDev.Enabled) {
                    Dictionary<AlgorithmType, double> cumulativeSpeeds = perDeviceTypeBenchmarks[curCDev.UUID];
                    var deviceConfig = curCDev.DeviceBenchmarkConfig;
                    foreach (var kvp in deviceConfig.AlgorithmSettings) {
                        var key = kvp.Key;
                        var algorithm = kvp.Value;
                        // instead of cumulative speeds get just if enabled or not count x > 0 => enabled; x < 0 => disabled
                        if (cumulativeSpeeds[key] > 0) {
                            cumulativeSpeeds[key] = algorithm.BenchmarkSpeed;
                        } else {
                            cumulativeSpeeds[key] = -1 * algorithm.BenchmarkSpeed;
                        }
                    }
                }
            }

            return perDeviceTypeBenchmarks;
        }


        PerDeviceProifitDictionary GetEnabledDeviceProifitDictionary(PerDeviceSpeedDictionary speedDict, Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            PerDeviceProifitDictionary profitDict = new PerDeviceProifitDictionary();

            // log stuff
            int MAX_NAME_LEN = "daggerhashimoto".Length;
            int MAX_SPEED_LEN = 15;
            StringBuilder stringBuilderFull = new StringBuilder();
            stringBuilderFull.AppendLine("Current device profits:");

            foreach (var nameBenchKvp in speedDict) {
                var deviceUUID = nameBenchKvp.Key;
                var curDevProfits = new Dictionary<AlgorithmType, double>();
                StringBuilder stringBuilderDevice = new StringBuilder();
                stringBuilderDevice.AppendLine(String.Format("\tProfits for {0} ({1}):", deviceUUID, ComputeDevice.GetNameForUUID(deviceUUID)));
                AlgorithmType mostProfitKey = AlgorithmType.NONE;
                double mostProfitAlgoVal = -1;
                foreach (var algoSpeedKvp in nameBenchKvp.Value) {
                    // Log stuff and calculation
                    string name = AlgorithmNiceHashNames.GetName(algoSpeedKvp.Key);
                    int namePreatyCount = MAX_NAME_LEN - name.Length;
                    if (namePreatyCount <= 0) {
                        namePreatyCount = 1;
                    }
                    string namePreaty = name + new String(' ', namePreatyCount);
                    bool isEnabled = algoSpeedKvp.Value > 0;
                    double nhmSMADataVal = NiceHashData[algoSpeedKvp.Key].paying;
                    // TODO what is the constant at the end?
                    double algoProfit = algoSpeedKvp.Value * nhmSMADataVal * 0.000000001;

                    // calculate
                    if (isEnabled) {
                        curDevProfits.Add(algoSpeedKvp.Key, algoProfit);
                        if (mostProfitAlgoVal < algoProfit) {
                            mostProfitKey = algoSpeedKvp.Key;
                            mostProfitAlgoVal = algoProfit;
                        }
                    } else {
                        // if disabled make unprofitable
                        curDevProfits.Add(algoSpeedKvp.Key, -1000000);
                        algoProfit *= -1; // make bigger then 0 for logging reasons
                    }
                    // log stuff
                    string speedStr = algoSpeedKvp.Value.ToString("F3");
                    int speedStrCount = MAX_SPEED_LEN - speedStr.Length;
                    if (speedStrCount <= 0) {
                        speedStrCount = 1;
                    }
                    string speedPreaty = new String(' ', speedStrCount) + speedStr;
                    stringBuilderDevice.AppendLine(String.Format("\t\t{0}\t:\tPROFIT = {1}  ({2}, SPEED = {3}, NHSMA = {4})",
                    namePreaty, // Name
                    algoProfit.ToString(DOUBLE_FORMAT), // Profit
                    isEnabled ? "ENABLED " : "DISABLED", // ENABLED/DISABLED
                    speedPreaty, // Speed
                    nhmSMADataVal.ToString(DOUBLE_FORMAT) // NiceHashData
                    ));
                }
                // add profits
                profitDict.Add(deviceUUID, curDevProfits);
                // log stuff
                stringBuilderDevice.AppendLine(String.Format("\t\tMOST PROFITABLE (ENABLED) ALGO: {0}, PROFIT: {1}",
                    AlgorithmNiceHashNames.GetName(mostProfitKey),
                    mostProfitAlgoVal.ToString(DOUBLE_FORMAT)));
                stringBuilderFull.AppendLine(stringBuilderDevice.ToString());
            }

            Helpers.ConsolePrint(TAG, stringBuilderFull.ToString());
            return profitDict;
        }

        #region Groupping logic
        private bool IsAlgorithmSettingsSame(Algorithm a, Algorithm b) {
            return a.NiceHashID == b.NiceHashID
#if (SWITCH_TESTING)
 && (!ForcePerCardMiners) // this will force individual miners
#endif
;
        }

        //private bool SafeStrCompare(string a, string b) {
        //    if (string.IsNullOrEmpty(a) == true && string.IsNullOrEmpty(a) == string.IsNullOrEmpty(b)) return true;
        //    return a == b;
        //}

        private bool IsNvidiaDevice(ComputeDevice a) {
            foreach (var type in _nvidiaTypes) {
                if (a.DeviceGroupType == type) return true;
            }
            return false;
        }

        // checks if dagger algo, same settings and if compute platform is same
        private bool IsDaggerAndSameComputePlatform(ComputeDevice a, ComputeDevice b) {
            return a.MostProfitableAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto
                && a.MostProfitableAlgorithm.NiceHashID == b.MostProfitableAlgorithm.NiceHashID //IsAlgorithmSettingsSame(a.MostProfitableAlgorithm, b.MostProfitableAlgorithm)
                // check if both etherum capable
                && a.IsEtherumCapale && b.IsEtherumCapale
                // compute platforms must be same
                && (IsNvidiaDevice(a) == IsNvidiaDevice(b));
        }

        private bool IsNotCpuGroups(ComputeDevice a, ComputeDevice b) {
            return a.DeviceGroupType != DeviceGroupType.CPU && b.DeviceGroupType != DeviceGroupType.CPU;
        }

        // this will not check Ethminer path
        private bool IsSameBinPath(ComputeDevice a, ComputeDevice b) {
            // same group uses same Miner class and therefore same binary path for same algorithm
            bool sameGroup = a.DeviceGroupType == b.DeviceGroupType;
            if (!sameGroup) {
                var a_algoType = a.MostProfitableAlgorithm.NiceHashID;
                var b_algoType = b.MostProfitableAlgorithm.NiceHashID;
                // a and b algorithm settings should be the same if we call this function
                return MinerPathChecker[a.DeviceGroupType].GetOptimizedMinerPath(a_algoType, a.Codename, a.IsOptimizedVersion)
                    == MinerPathChecker[b.DeviceGroupType].GetOptimizedMinerPath(b_algoType, b.Codename, b.IsOptimizedVersion);
            }

            return true;
        }

        // we don't want to group CPU devices
        private bool IsGroupBinaryAndAlgorithmSame(ComputeDevice a, ComputeDevice b) {
            return IsNotCpuGroups(a, b)
                && IsAlgorithmSettingsSame(a.MostProfitableAlgorithm, b.MostProfitableAlgorithm)
                && IsSameBinPath(a, b);
        }
        #endregion //Groupping logic

        /// <summary>
        /// SwichMostProfitable should check the best combination for most profit.
        /// Calculate profit for each supported algorithm per device group.
        /// Build from ground up compatible devices and algorithms.
        /// See #region Groupping logic
        /// Device groups are CPU, AMD_OpenCL and NVIDIA CUDA SM.x.x.
        /// NVIDIA SMx.x should be paired separately except for daggerhashimoto.
        /// </summary>
        /// <param name="NiceHashData"></param>
        public void SwichMostProfitableGroupUpMethod(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            var devProfits = GetEnabledDeviceProifitDictionary(_perDeviceSpeedDictionary, NiceHashData);

#if (SWITCH_TESTING)
            SwitchTesting.Instance.SetNext(ref devProfits, _enabledDevices);
#endif
            double CurrentProfit = 0.0d;
            // calculate most profitable algorithm per enabled device
            foreach (var cdev in _enabledDevices) {
                var curDevProfits = devProfits[cdev.UUID];
                double maxProfit = double.MinValue;
                AlgorithmType maxAlgorithmTypeKey = AlgorithmType.NONE;
                var algorithmSettings = cdev.DeviceBenchmarkConfig.AlgorithmSettings;

                foreach (var kvpTypeProfit in curDevProfits) {
                    if (algorithmSettings.ContainsKey(kvpTypeProfit.Key)
                        && !algorithmSettings[kvpTypeProfit.Key].Skip
                        && kvpTypeProfit.Value > 0.0d
                        && maxProfit < kvpTypeProfit.Value) {
                        // extra check if current device can't handle dagger
                        if (AlgorithmType.DaggerHashimoto == kvpTypeProfit.Key && !cdev.IsEtherumCapale) {
                            continue;
                        }
                        maxProfit = kvpTypeProfit.Value;
                        maxAlgorithmTypeKey = kvpTypeProfit.Key;
                    }
                }
                if(maxAlgorithmTypeKey == AlgorithmType.NONE) {
                    cdev.MostProfitableAlgorithm = null;
                } else {
                    cdev.MostProfitableAlgorithm
                        = algorithmSettings[maxAlgorithmTypeKey];
                    // add most profitable to cumulative profit
                    CurrentProfit += maxProfit;
                }
            }

            // now if profitable check
            // TODO FOR NOW USD ONLY
            var currentProfitUSD = (CurrentProfit * Globals.BitcoinRate);
            Helpers.ConsolePrint(TAG,  "Current Global profit: " + currentProfitUSD.ToString("F8") + " USD/Day");
            if (!IsConnectedToInternet || (ConfigManager.Instance.GeneralConfig.MinimumProfit > 0
                    && currentProfitUSD < ConfigManager.Instance.GeneralConfig.MinimumProfit)) {
                IsProfitable = false;
                IsCurrentlyIdle = true;
                if (!IsConnectedToInternet) {
                    // change msg
                    Helpers.ConsolePrint(TAG, "NO INTERNET!!! Stopping mining.");
                    _mainFormRatesComunication.ShowNotProfitable(International.GetText("Form_Main_MINING_NO_INTERNET_CONNECTION"));
                } else {
                    _mainFormRatesComunication.ShowNotProfitable(International.GetText("Form_Main_MINING_NOT_PROFITABLE"));
                }
                // return don't group
                StopAllMinersNonProfitable();
                Helpers.ConsolePrint(TAG, "Current Global profit: NOT PROFITABLE MinProfit " + ConfigManager.Instance.GeneralConfig.MinimumProfit.ToString("F8") + " USD/Day");
                return;
            } else {
                IsProfitable = true;
                IsCurrentlyIdle = false;
                _mainFormRatesComunication.HideNotProfitable();
                string profitabilityInfo = ConfigManager.Instance.GeneralConfig.MinimumProfit == 0 ? "mine always regardless of profit" : ConfigManager.Instance.GeneralConfig.MinimumProfit.ToString("F8") + " USD/Day";
                Helpers.ConsolePrint(TAG, "Current Global profit: IS PROFITABLE MinProfit " + profitabilityInfo);
            }

            // group devices with same supported algorithms
            _previousAllGroupedDevices = _currentAllGroupedDevices;
            _currentAllGroupedDevices = new AllGroupedDevices();
            Dictionary<GroupedDevices, Algorithm> newGroupAndAlgorithm = new Dictionary<GroupedDevices,Algorithm>();
            for (int first = 0; first < _enabledDevices.Count; ++first) {
                var firstDev = _enabledDevices[first];
                // skip if no algorithm is profitable
                if (firstDev.MostProfitableAlgorithm == null) {
                    Helpers.ConsolePrint("SwichMostProfitableGroupUpMethod", String.Format("Device {0}, MostProfitableAlgorithm == null", firstDev.Name));
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
                for (int second = first + 1; second < _enabledDevices.Count; ++second) {
                    var secondDev = _enabledDevices[second];
                    // first check if second device has profitable algorithm
                    if (secondDev.MostProfitableAlgorithm != null) {
                        // check if we should group
                        if (IsDaggerAndSameComputePlatform(firstDev, secondDev)
                            || IsGroupBinaryAndAlgorithmSame(firstDev, secondDev)) {
                            newGroup.Add(secondDev.UUID);
                        }
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
                currentGroupMiners.StartAlgorihtm(algorithm, _miningLocation, _workerBtcStringWorker);
            }

            // stats quick fix code
            if (_currentAllGroupedDevices.Count != _previousAllGroupedDevices.Count) {
                MinerStatsCheck(NiceHashData);
            }
        }

        public void MinerStatsCheck(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            _mainFormRatesComunication.ClearRates(_currentAllGroupedDevices.Count);
            foreach (var group in _currentAllGroupedDevices) {
                var groupMiners = _groupedDevicesMiners[CalcGroupedDevicesKey(group)];
                Miner m = groupMiners.CurrentWorkingMiner;

                // skip if not running
                if (!m.IsRunning) continue;

                APIData AD = m.GetSummary();
                if (AD == null) {
                    Helpers.ConsolePrint(m.MinerTAG(), "GetSummary returned null..");
                }
                // set rates
                if (NiceHashData != null && AD != null) {
                    m.CurrentRate = NiceHashData[AD.AlgorithmID].paying * AD.Speed * 0.000000001;
                } else {
                    m.CurrentRate = 0;
                    // set empty
                    AD = new APIData() {
                        AlgorithmID = m.CurrentAlgorithmType,
                        AlgorithmName = AlgorithmNiceHashNames.GetName(m.CurrentAlgorithmType),
                        Speed = 0.0d
                    };
                }

                // Update GUI
                _mainFormRatesComunication.AddRateInfo(m.MinerTAG(), groupMiners.DevicesInfoString, AD, m.CurrentRate, m.IsAPIReadException);
            }
        }


    }
}
