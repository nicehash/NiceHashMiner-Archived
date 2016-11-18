using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Timer = System.Timers.Timer;
using System.Timers;

namespace NiceHashMiner.Miners {
    using GroupedDevices = SortedSet<string>;
    using NiceHashMiner.Configs;
    using System.IO;
    public class MiningSession {
        const string TAG = "MiningSession";
        const string DOUBLE_FORMAT = "F12";

        // session varibles fixed
        string _miningLocation = "";
        string _btcAdress = "";
        string _worker = "";
        List<MiningDevice> _miningDevices = new List<MiningDevice>();
        IMainFormRatesComunication _mainFormRatesComunication;

        // session varibles changing
        // GroupDevices hash code doesn't work correctly use string instead
        //Dictionary<GroupedDevices, GroupMiners> _groupedDevicesMiners;
        Dictionary<string, GroupMiners> _groupedDevicesMiners = new Dictionary<string,GroupMiners>();
        List<SortedSet<string>> _previousAllGroupedDevices = new List<GroupedDevices>();
        List<SortedSet<string>> _currentAllGroupedDevices = new List<GroupedDevices>();


        private bool IsProfitable = true;

        private bool IsConnectedToInternet = true;
        private bool IsMiningRegardlesOfProfit = true;

        // timers 
        private Timer _preventSleepTimer;
        // check internet connection 
        private Timer _internetCheckTimer;
        
        // 
        private Timer _miningStatusCheckTimer;
        bool CheckStatus = false;

        public bool IsMiningEnabled {
            get {
                // if this is not empty it means we can mine
                return _miningDevices.Count > 0;
            }
        }

        private bool IsCurrentlyIdle {
            get {
                return !IsMiningEnabled || !IsConnectedToInternet || !IsProfitable;
            }
        }

        public MiningSession(List<ComputeDevice> devices,
            IMainFormRatesComunication mainFormRatesComunication,
            string miningLocation, string worker, string btcAdress) {
            // init fixed
            _mainFormRatesComunication = mainFormRatesComunication;
            _miningLocation = miningLocation;

            _btcAdress = btcAdress;
            _worker = worker;

            // initial settup
            {
                // used for logging
                List<Tuple<ComputeDevice, DeviceMiningStatus>> disabledDevicesStatuses = new List<Tuple<ComputeDevice, DeviceMiningStatus>>();
                // logging and settup
                List<ComputeDevice> enabledDevices = new List<ComputeDevice>();
                // get enabled devices
                {
                    // check passed devices statuses
                    List<Tuple<ComputeDevice, DeviceMiningStatus>> devicesStatuses = new List<Tuple<ComputeDevice, DeviceMiningStatus>>();
                    foreach (var device in devices) {
                        devicesStatuses.Add(getDeviceMiningStatus(device));
                    }
                    // sort device statuses
                    foreach (var deviceStatus in devicesStatuses) {
                        if (deviceStatus.Item2 == DeviceMiningStatus.CanMine) {
                            enabledDevices.Add(deviceStatus.Item1);
                        } else {
                            disabledDevicesStatuses.Add(deviceStatus);
                        }
                    }
                }
                // print statuses
                if (disabledDevicesStatuses.Count > 0) {
                    Helpers.ConsolePrint(TAG, "Disabled Devices:");
                    foreach (var deviceStatus in disabledDevicesStatuses) {
                        ComputeDevice device = deviceStatus.Item1;
                        DeviceMiningStatus status = deviceStatus.Item2;
                        if (status == DeviceMiningStatus.DeviceNull) {
                            Helpers.ConsolePrint(TAG, "Critical Device is NULL");
                        } else if (status == DeviceMiningStatus.Disabled) {
                            Helpers.ConsolePrint(TAG, String.Format("DISABLED ({0})", device.GetFullName()));
                        } else if (status == DeviceMiningStatus.NoEnabledAlgorithms) {
                            Helpers.ConsolePrint(TAG, String.Format("No Enabled Algorithms ({0})", device.GetFullName()));
                        }
                    }
                }
                if (enabledDevices.Count > 0) {
                    // print enabled
                    Helpers.ConsolePrint(TAG, "Enabled Devices for Mining session:");
                    foreach (var device in enabledDevices) {
                        Helpers.ConsolePrint(TAG, String.Format("ENABLED ({0})", device.GetFullName()));
                        foreach (var algo in device.DeviceBenchmarkConfig.AlgorithmSettings.Values) {
                            var isEnabled = IsAlgoMiningCapable(algo);
                            Helpers.ConsolePrint(TAG, String.Format("\t\tALGORITHM {0} ({1})",
                                isEnabled ? "ENABLED " : "DISABLED", // ENABLED/DISABLED
                                AlgorithmNiceHashNames.GetName(algo.NiceHashID)));
                        }
                    }
                    // settup mining devices
                    foreach (var device in enabledDevices) {
                        _miningDevices.Add(new MiningDevice(device));
                    }
                }
            }
            if (_miningDevices.Count > 0) {
                // calculate avarage speeds, to ensure mining stability
                // device name, algo key, algos refs list
                Dictionary<string,
                    Dictionary<AlgorithmType,
                    List<MiningAlgorithm>>> avarager = new Dictionary<string, Dictionary<AlgorithmType, List<MiningAlgorithm>>>();
                // init empty avarager
                foreach (var device in _miningDevices) {
                    string devName = device.Device.Name;
                    avarager[devName] = new Dictionary<AlgorithmType, List<MiningAlgorithm>>();
                    foreach (var key in AlgorithmNiceHashNames.GetAllAvaliableTypes()) {
                        avarager[devName][key] = new List<MiningAlgorithm>();
                    }
                }
                // fill avarager
                foreach (var device in _miningDevices) {
                    string devName = device.Device.Name;
                    foreach (var kvp in device.Algorithms) {
                        var key = kvp.Key;
                        MiningAlgorithm algo = kvp.Value;
                        avarager[devName][key].Add(algo);
                    }
                }
                // calculate avarages
                foreach (var devDict in avarager.Values) {
                    foreach (List<MiningAlgorithm> miningAlgosList in devDict.Values) {
                        // if list not empty calculate avarage
                        if (miningAlgosList.Count > 0) {
                            // calculate avarage
                            double sum = 0;
                            foreach (var algo in miningAlgosList) {
                                sum += algo.AvaragedSpeed;
                            }
                            double avarageSpeed = sum / miningAlgosList.Count;
                            // set avarage
                            foreach (var algo in miningAlgosList) {
                                algo.AvaragedSpeed = avarageSpeed;
                            }
                        }
                    }
                }
            }

            // init timer stuff
            _preventSleepTimer = new Timer();
            _preventSleepTimer.Elapsed += PreventSleepTimer_Tick;
            // sleep time is minimal 1 minute
            _preventSleepTimer.Interval = 20 * 1000; // leave this interval, it works

            // set internet checking
            _internetCheckTimer = new Timer();
            _internetCheckTimer.Elapsed += InternetCheckTimer_Tick;
            _internetCheckTimer.Interval = 1000 * 30 * 1; // every minute or 5?? // 1000 * 60 * 1

            _miningStatusCheckTimer = new Timer();
            _miningStatusCheckTimer.Elapsed += MiningStatusCheckTimer_Tick;
            _miningStatusCheckTimer.Interval = 1000 * 30;

            // assume profitable
            IsProfitable = true;
            // assume we have internet
            IsConnectedToInternet = true;

            if (IsMiningEnabled) {
                _preventSleepTimer.Start();
                _internetCheckTimer.Start();
                _miningStatusCheckTimer.Start();
            }

            IsMiningRegardlesOfProfit = ConfigManager.Instance.GeneralConfig.MinimumProfit == 0;
        }

        #region Timers stuff
        private void InternetCheckTimer_Tick(object sender, EventArgs e) {
            if (ConfigManager.Instance.GeneralConfig.ContinueMiningIfNoInternetAccess == false) {
                IsConnectedToInternet = Helpers.IsConnectedToInternet();
            }
        }

        private void PreventSleepTimer_Tick(object sender, ElapsedEventArgs e) {
            // when mining keep system awake, prevent sleep
            Helpers.PreventSleep();
        }

        private void MiningStatusCheckTimer_Tick(object sender, ElapsedEventArgs e) {
            CheckStatus = true;
        }
        #endregion

        #region CanMine Checking

        static bool IsAlgoMiningCapable(Algorithm algo) {
            return algo != null && !algo.Skip && algo.BenchmarkSpeed > 0;
        }

        Tuple<ComputeDevice, DeviceMiningStatus> getDeviceMiningStatus(ComputeDevice device) {
            DeviceMiningStatus status = DeviceMiningStatus.CanMine;
            if (device == null) { // C# is null happy
                status = DeviceMiningStatus.DeviceNull;
            } else if(device.Enabled == false) {
                status = DeviceMiningStatus.Disabled;
            } else {
                bool hasEnabledAlgo = false;
                foreach(Algorithm algo in device.DeviceBenchmarkConfig.AlgorithmSettings.Values) {
                    hasEnabledAlgo |= IsAlgoMiningCapable(algo);
                }
                if (hasEnabledAlgo == false) {
                    status = DeviceMiningStatus.NoEnabledAlgorithms;
                }
            }
            return new Tuple<ComputeDevice,DeviceMiningStatus>(device, status);
        }
        #endregion CanMine Checking

        #region Start/Stop
        public void StopAllMiners() {
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
            _miningStatusCheckTimer.Stop();
            Helpers.AllowMonitorPowerdownAndSleep();

            // delete generated bin files
            // check for bins files
            var dirInfo = new DirectoryInfo(MinerPaths.nheqminer.Replace("nheqminer.exe", ""));
            var DONT_DELETE = "equiw200k9.bin";
            var deleteContains = "equiw200k9";
            var alwaysDeleteContains2 = "silentarmy_gpu";
            if (dirInfo != null && dirInfo.Exists) {
                foreach (FileInfo file in dirInfo.GetFiles()) {
                    if (file.Name != DONT_DELETE && file.Name.Contains(deleteContains)) {
                        file.Delete();
                    }
                    if (file.Name.Contains(alwaysDeleteContains2)) {
                        file.Delete();
                    }
                }
            }
        }

        public void StopAllMinersNonProfitable() {
            if (_groupedDevicesMiners != null) {
                foreach (var kv in _groupedDevicesMiners) {
                    kv.Value.End();
                }
            }
            if (_mainFormRatesComunication != null) {
                _mainFormRatesComunication.ClearRates(-1);
            }
        }
        #endregion Start/Stop

        private string CalcGroupedDevicesKey(GroupedDevices group) {
            return string.Join(", ", group);
        }

        public string GetActiveMinersGroup() {
            if (IsCurrentlyIdle) {
                return "IDLE";
            }

            string ActiveMinersGroup = "";

            //get unique miner groups like CPU, NVIDIA, AMD,...
            HashSet<string> UniqueMinerGroups = new HashSet<string>();
            foreach (var curDevice in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                if (curDevice.Enabled) {
                    UniqueMinerGroups.Add(GroupNames.GetNameGeneral(curDevice.DeviceType));
                }
            }
            if (UniqueMinerGroups.Count > 0 && IsProfitable) {
                ActiveMinersGroup = string.Join("/", UniqueMinerGroups);
            }

            return ActiveMinersGroup;
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

        // full of state
        private bool CheckIfProfitable(double CurrentProfit, bool log = true) {
            // TODO FOR NOW USD ONLY
            var currentProfitUSD = (CurrentProfit * Globals.BitcoinRate);
            IsProfitable = 
                IsMiningRegardlesOfProfit
                || !IsMiningRegardlesOfProfit && currentProfitUSD >= ConfigManager.Instance.GeneralConfig.MinimumProfit;
            if (log) {
                Helpers.ConsolePrint(TAG, "Current Global profit: " + currentProfitUSD.ToString("F8") + " USD/Day");
                if (!IsProfitable) {
                    Helpers.ConsolePrint(TAG, "Current Global profit: NOT PROFITABLE MinProfit " + ConfigManager.Instance.GeneralConfig.MinimumProfit.ToString("F8") + " USD/Day");
                } else {
                    string profitabilityInfo = IsMiningRegardlesOfProfit ? "mine always regardless of profit" : ConfigManager.Instance.GeneralConfig.MinimumProfit.ToString("F8") + " USD/Day";
                    Helpers.ConsolePrint(TAG, "Current Global profit: IS PROFITABLE MinProfit " + profitabilityInfo);
                }
            }
            return IsProfitable;
        }

        private bool CheckIfShouldMine(double CurrentProfit, bool log = true) {
            // if profitable and connected to internet mine
            bool shouldMine = CheckIfProfitable(CurrentProfit, log) && IsConnectedToInternet;
            if (shouldMine) {
                _mainFormRatesComunication.HideNotProfitable();
            } else {
                if (!IsConnectedToInternet) {
                    // change msg
                    if (log) Helpers.ConsolePrint(TAG, "NO INTERNET!!! Stopping mining.");
                    _mainFormRatesComunication.ShowNotProfitable(International.GetText("Form_Main_MINING_NO_INTERNET_CONNECTION"));
                } else {
                    _mainFormRatesComunication.ShowNotProfitable(International.GetText("Form_Main_MINING_NOT_PROFITABLE"));
                }
                // return don't group
                StopAllMinersNonProfitable();
            }
            return shouldMine;
        }

        public void SwichMostProfitableGroupUpMethod(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData, bool log = true) {
            List<MiningDevice> profitableDevices = new List<MiningDevice>();
            double CurrentProfit = 0.0d;
            foreach (var device in _miningDevices) {
                // calculate profits
                device.CalculateProfits(NiceHashData);
                // check if device has profitable algo
                if (device.MostProfitableKey != AlgorithmType.NONE) {
                    profitableDevices.Add(device);
                    CurrentProfit += device.GetCurrentMostProfitValue;
                    device.Device.MostProfitableAlgorithm = device.Algorithms[device.MostProfitableKey].algoRef;
                }
            }
            // print profit statuses
            if (log) {
                StringBuilder stringBuilderFull = new StringBuilder();
                stringBuilderFull.AppendLine("Current device profits:");
                foreach (var device in _miningDevices) {
                    StringBuilder stringBuilderDevice = new StringBuilder();
                    stringBuilderDevice.AppendLine(String.Format("\tProfits for {0} ({1}):", device.Device.UUID, device.Device.Name));
                    foreach (var algo in device.Algorithms) {
                        stringBuilderDevice.AppendLine(String.Format("\t\tPROFIT = {0}\t(SPEED = {1}\t\t| NHSMA = {2})\t[{3}]",
                            algo.Value.CurrentProfit.ToString(DOUBLE_FORMAT), // Profit
                            algo.Value.AvaragedSpeed, // Speed
                            algo.Value.CurNhmSMADataVal, // NiceHashData
                            AlgorithmNiceHashNames.GetName(algo.Key) // Name
                        ));
                    }
                    // most profitable
                    stringBuilderDevice.AppendLine(String.Format("\t\tMOST PROFITABLE ALGO: {0}, PROFIT: {1}",
                        AlgorithmNiceHashNames.GetName(device.MostProfitableKey),
                        device.GetCurrentMostProfitValue.ToString(DOUBLE_FORMAT)));
                    stringBuilderFull.AppendLine(stringBuilderDevice.ToString());
                }
                Helpers.ConsolePrint(TAG, stringBuilderFull.ToString());
            }

            // check if should mine
            if (CheckIfShouldMine(CurrentProfit, log) == false) {
                return;
            }

            // group devices with same supported algorithms
            _previousAllGroupedDevices = _currentAllGroupedDevices;
            _currentAllGroupedDevices = new List<SortedSet<string>>();
            Dictionary<GroupedDevices, Algorithm> newGroupAndAlgorithm = new Dictionary<GroupedDevices, Algorithm>();
            for (int first = 0; first < profitableDevices.Count; ++first) {
                var firstDev = profitableDevices[first].Device;
                // skip if no algorithm is profitable
                if (firstDev.MostProfitableAlgorithm == null) {
                    if (log) Helpers.ConsolePrint("SwichMostProfitableGroupUpMethod", String.Format("Device {0}, MostProfitableAlgorithm == null", firstDev.Name));
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
                for (int second = first + 1; second < profitableDevices.Count; ++second) {
                    var secondDev = profitableDevices[second].Device;
                    // first check if second device has profitable algorithm
                    if (secondDev.MostProfitableAlgorithm != null) {
                        // check if we should group
                        bool isEquihashGroup = GroupingLogic.IsEquihashGroupLogic(firstDev, secondDev);
                        bool isDaggerAndSameComputePlatform = GroupingLogic.IsDaggerAndSameComputePlatform(firstDev, secondDev);
                        bool isGroupBinaryAndAlgorithmSame = GroupingLogic.IsGroupBinaryAndAlgorithmSame(firstDev, secondDev);
                        if (isEquihashGroup
                            || isDaggerAndSameComputePlatform
                            || isGroupBinaryAndAlgorithmSame) {
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
            foreach (var kvpGroupAlgorithm in newGroupAndAlgorithm) {
                var group = kvpGroupAlgorithm.Key;
                var algorithm = kvpGroupAlgorithm.Value;

                GroupMiners currentGroupMiners;
                // try find if it doesn't exist create new
                string groupStringKey = CalcGroupedDevicesKey(group);
                if (!_groupedDevicesMiners.TryGetValue(groupStringKey, out currentGroupMiners)) {
                    currentGroupMiners = new GroupMiners(group);
                    _groupedDevicesMiners.Add(groupStringKey, currentGroupMiners);
                }
                currentGroupMiners.StartAlgorihtm(algorithm, _miningLocation, _btcAdress, _worker);
            }

            // stats quick fix code
            if (_currentAllGroupedDevices.Count != _previousAllGroupedDevices.Count) {
                MinerStatsCheck(NiceHashData);
            }
        }

        public void MinerStatsCheck(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            double CurrentProfit = 0.0d;
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
                CurrentProfit += m.CurrentRate;
                // Update GUI
                _mainFormRatesComunication.AddRateInfo(m.MinerTAG(), groupMiners.DevicesInfoString, AD, m.CurrentRate, m.IsAPIReadException);
            }
            // check if profitabile
            if (CheckStatus && !IsMiningRegardlesOfProfit) {
                CheckStatus = false;
                if (IsProfitable) {
                    // check current profit
                    CheckIfShouldMine(CurrentProfit, true);
                } else if (!IsProfitable) {
                    // recalculate and switch
                    SwichMostProfitableGroupUpMethod(NiceHashData, true);
                }
            }
        }

        #region Private  // Private classes, enums, structs
        private enum DeviceMiningStatus {
            Disabled,
            NoEnabledAlgorithms,
            DeviceNull,
            CanMine
        }
        private class MiningAlgorithm {
            public MiningAlgorithm(Algorithm algo) {
                algoRef = algo;
                // init speed that will be avaraged later
                AvaragedSpeed = algo.BenchmarkSpeed;
            }
            public Algorithm algoRef { get; private set; }
            // avarage speed of same devices to increase mining stability
            public double AvaragedSpeed = 0;
            public double CurrentProfit = 0;
            public double CurNhmSMADataVal = 0;
        }
        private class MiningDevice {
            public MiningDevice(ComputeDevice device) {
                Device = device;
                foreach (var kvp in Device.DeviceBenchmarkConfig.AlgorithmSettings) {
                    AlgorithmType key = kvp.Key;
                    Algorithm algo = kvp.Value;
                    if (IsAlgoMiningCapable(algo)) {
                        Algorithms[key] = new MiningAlgorithm(algo);
                    }
                }
            }
            public ComputeDevice Device { get; private set; }
            public Dictionary<AlgorithmType, MiningAlgorithm> Algorithms = new Dictionary<AlgorithmType,MiningAlgorithm>();

            public AlgorithmType MostProfitableKey { get; private set; }

            public double GetCurrentMostProfitValue {
                get {
                    if (AlgorithmType.NONE != MostProfitableKey) {
                        return Algorithms[MostProfitableKey].CurrentProfit;
                    }
                    return 0;
                }
            }

            public void CalculateProfits(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
                // assume none is profitable
                MostProfitableKey = AlgorithmType.NONE;
                // calculate new profits
                foreach (var miningAlgo in Algorithms) {
                    AlgorithmType key = miningAlgo.Key;
                    MiningAlgorithm algo = miningAlgo.Value;
                    if (NiceHashData.ContainsKey(key)) {
                        algo.CurNhmSMADataVal = NiceHashData[key].paying;
                        algo.CurrentProfit = algo.CurNhmSMADataVal * algo.AvaragedSpeed * 0.000000001;
                    } else {
                        algo.CurrentProfit = 0;
                    }
                }
                // find max paying value and save key
                double maxProfit = 0;
                foreach (var miningAlgo in Algorithms) {
                    AlgorithmType key = miningAlgo.Key;
                    MiningAlgorithm algo = miningAlgo.Value;
                    if (maxProfit < algo.CurrentProfit) {
                        maxProfit = algo.CurrentProfit;
                        MostProfitableKey = key;
                    }
                }
            }
        }
        #endregion Private  // Private classes, enums, structs
    }
}
