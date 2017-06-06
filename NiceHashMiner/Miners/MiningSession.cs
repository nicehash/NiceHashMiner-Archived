using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Configs;
using System.IO;
using NiceHashMiner.Net20_backport;

using Timer = System.Timers.Timer;
using System.Timers;

namespace NiceHashMiner.Miners {
    using GroupedDevices = SortedSet<string>;
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
        Dictionary<string, GroupMiner> _runningGroupMiners = new Dictionary<string,GroupMiner>();
        GroupMiner _ethminerNVIDIAPaused = null;
        GroupMiner _ethminerAMDPaused = null;


        private bool IsProfitable = true;

        private bool IsConnectedToInternet = true;
        private bool IsMiningRegardlesOfProfit = true;

        // timers 
        private Timer _preventSleepTimer;
        // check internet connection 
        private Timer _internetCheckTimer;
        

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
            _miningDevices = GroupSetupUtils.GetMiningDevices(devices, true);
            if (_miningDevices.Count > 0) {
                GroupSetupUtils.AvarageSpeeds(_miningDevices);
            }

            // init timer stuff
            _preventSleepTimer = new Timer();
            _preventSleepTimer.Elapsed += PreventSleepTimer_Tick;
            // sleep time is minimal 1 minute
            _preventSleepTimer.Interval = 20 * 1000; // leave this interval, it works

            // set internet checking
            _internetCheckTimer = new Timer();
            _internetCheckTimer.Elapsed += InternetCheckTimer_Tick;
            _internetCheckTimer.Interval = 1 * 1000 * 60; // every minute

            // assume profitable
            IsProfitable = true;
            // assume we have internet
            IsConnectedToInternet = true;

            if (IsMiningEnabled) {
                _preventSleepTimer.Start();
                _internetCheckTimer.Start();
            }

            IsMiningRegardlesOfProfit = ConfigManager.GeneralConfig.MinimumProfit == 0;
        }

        #region Timers stuff
        private void InternetCheckTimer_Tick(object sender, EventArgs e) {
            if (ConfigManager.GeneralConfig.IdleWhenNoInternetAccess) {
                IsConnectedToInternet = Helpers.IsConnectedToInternet();
            }
        }

        private void PreventSleepTimer_Tick(object sender, ElapsedEventArgs e) {
            // when mining keep system awake, prevent sleep
            Helpers.PreventSleep();
        }

        #endregion

        #region Start/Stop
        public void StopAllMiners() {
            if (_runningGroupMiners != null) {
                foreach (var groupMiner in _runningGroupMiners.Values) {
                    groupMiner.End();
                }
                _runningGroupMiners = new Dictionary<string, GroupMiner>();
            }
            if (_ethminerNVIDIAPaused != null) {
                _ethminerNVIDIAPaused.End();
                _ethminerNVIDIAPaused = null;
            }
            if (_ethminerAMDPaused != null) {
                _ethminerAMDPaused.End();
                _ethminerAMDPaused = null;
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
            if (_runningGroupMiners != null) {
                foreach (var groupMiner in _runningGroupMiners.Values) {
                    groupMiner.End();
                }
                _runningGroupMiners = new Dictionary<string, GroupMiner>();
            }
            if (_ethminerNVIDIAPaused != null) {
                _ethminerNVIDIAPaused.End();
                _ethminerNVIDIAPaused = null;
            }
            if (_ethminerAMDPaused != null) {
                _ethminerAMDPaused.End();
                _ethminerAMDPaused = null;
            }
            if (_mainFormRatesComunication != null) {
                _mainFormRatesComunication.ClearRates(-1);
            }
        }
        #endregion Start/Stop

        private string CalcGroupedDevicesKey(GroupedDevices group) {
            return StringHelper.Join(", ", group);
        }

        public string GetActiveMinersGroup() {
            if (IsCurrentlyIdle) {
                return "IDLE";
            }

            string ActiveMinersGroup = "";

            //get unique miner groups like CPU, NVIDIA, AMD,...
            HashSet<string> UniqueMinerGroups = new HashSet<string>();
            foreach (var miningDevice in _miningDevices) {
                //if (miningDevice.MostProfitableKey != AlgorithmType.NONE) {
                    UniqueMinerGroups.Add(GroupNames.GetNameGeneral(miningDevice.Device.DeviceType));
                //}
            }
            if (UniqueMinerGroups.Count > 0 && IsProfitable) {
                ActiveMinersGroup = StringHelper.Join("/", UniqueMinerGroups);
            }

            return ActiveMinersGroup;
        }

        public double GetTotalRate() {
            double TotalRate = 0;

            if (_runningGroupMiners != null) {
                foreach (var groupMiner in _runningGroupMiners.Values) {
                    TotalRate += groupMiner.CurrentRate;
                }
            }

            return TotalRate;
        }

        // full of state
        private bool CheckIfProfitable(double CurrentProfit, bool log = true) {
            // TODO FOR NOW USD ONLY
            var currentProfitUSD = (CurrentProfit * Globals.BitcoinUSDRate);
            IsProfitable = 
                IsMiningRegardlesOfProfit
                || !IsMiningRegardlesOfProfit && currentProfitUSD >= ConfigManager.GeneralConfig.MinimumProfit;
            if (log) {
                Helpers.ConsolePrint(TAG, "Current Global profit: " + currentProfitUSD.ToString("F8") + " USD/Day");
                if (!IsProfitable) {
                    Helpers.ConsolePrint(TAG, "Current Global profit: NOT PROFITABLE MinProfit " + ConfigManager.GeneralConfig.MinimumProfit.ToString("F8") + " USD/Day");
                } else {
                    string profitabilityInfo = IsMiningRegardlesOfProfit ? "mine always regardless of profit" : ConfigManager.GeneralConfig.MinimumProfit.ToString("F8") + " USD/Day";
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
#if (SWITCH_TESTING)
            MiningDevice.SetNextTest();
#endif
            List<MiningPair> profitableDevices = new List<MiningPair>();
            double CurrentProfit = 0.0d;
            double PrevStateProfit = 0.0d;
            foreach (var device in _miningDevices) {
                // calculate profits
                device.CalculateProfits(NiceHashData);
                // check if device has profitable algo
                if (device.HasProfitableAlgo()) {
                    profitableDevices.Add(device.GetMostProfitablePair());
                    CurrentProfit += device.GetCurrentMostProfitValue;
                    PrevStateProfit += device.GetPrevMostProfitValue;
                }
            }

            // print profit statuses
            if (log) {
                StringBuilder stringBuilderFull = new StringBuilder();
                stringBuilderFull.AppendLine("Current device profits:");
                foreach (var device in _miningDevices) {
                    StringBuilder stringBuilderDevice = new StringBuilder();
                    stringBuilderDevice.AppendLine(String.Format("\tProfits for {0} ({1}):", device.Device.UUID, device.Device.GetFullName()));
                    foreach (var algo in device.Algorithms) {
                       stringBuilderDevice.AppendLine(String.Format("\t\tPROFIT = {0}\t(SPEED = {1}\t\t| NHSMA = {2})\t[{3}]",
                            algo.CurrentProfit.ToString(DOUBLE_FORMAT), // Profit
                            algo.AvaragedSpeed + (algo.IsDual() ? "/" + algo.SecondaryAveragedSpeed : ""), // Speed
                            algo.CurNhmSMADataVal + (algo.IsDual() ? "/" + algo.SecondaryCurNhmSMADataVal : ""), // NiceHashData
                            algo.AlgorithmStringID // Name
                        ));
                    }
                    // most profitable
                    stringBuilderDevice.AppendLine(String.Format("\t\tMOST PROFITABLE ALGO: {0}, PROFIT: {1}",
                        device.GetMostProfitableString(),
                        device.GetCurrentMostProfitValue.ToString(DOUBLE_FORMAT)));
                    stringBuilderFull.AppendLine(stringBuilderDevice.ToString());
                }
                Helpers.ConsolePrint(TAG, stringBuilderFull.ToString());
            }

            // check profit threshold
            Helpers.ConsolePrint(TAG, String.Format("PrevStateProfit {0}, CurrentProfit {1}", PrevStateProfit, CurrentProfit));
            if (PrevStateProfit > 0 && CurrentProfit > 0) {
                double a = Math.Max(PrevStateProfit, CurrentProfit);
                double b = Math.Min(PrevStateProfit, CurrentProfit);
                //double percDiff = Math.Abs((PrevStateProfit / CurrentProfit) - 1);
                double percDiff = ((a - b)) / b;
                if (percDiff < ConfigManager.GeneralConfig.SwitchProfitabilityThreshold) {
                    // don't switch
                    Helpers.ConsolePrint(TAG, String.Format("Will NOT switch profit diff is {0}, current threshold {1}", percDiff, ConfigManager.GeneralConfig.SwitchProfitabilityThreshold));
                    // RESTORE OLD PROFITS STATE
                    foreach (var device in _miningDevices) {
                        device.RestoreOldProfitsState();
                    }
                    return;
                } else {
                    Helpers.ConsolePrint(TAG, String.Format("Will SWITCH profit diff is {0}, current threshold {1}", percDiff, ConfigManager.GeneralConfig.SwitchProfitabilityThreshold));
                }
            }

            // check if should mine
            // Only check if profitable inside this method when getting SMA data, cheching during mining is not reliable
            if (CheckIfShouldMine(CurrentProfit, log) == false) {
                return;
            }

            // group new miners 
            Dictionary<string, List<MiningPair>> newGroupedMiningPairs = new Dictionary<string,List<MiningPair>>();
            // group devices with same supported algorithms
            {
                var currentGroupedDevices = new List<GroupedDevices>();
                for (int first = 0; first < profitableDevices.Count; ++first) {
                    var firstDev = profitableDevices[first].Device;
                    // check if is in group
                    bool isInGroup = false;
                    foreach (var groupedDevices in currentGroupedDevices) {
                        if (groupedDevices.Contains(firstDev.UUID)) {
                            isInGroup = true;
                            break;
                        }
                    }
                    // if device is not in any group create new group and check if other device should group
                    if (isInGroup == false) {
                        var newGroup = new GroupedDevices();
                        var miningPairs = new List<MiningPair>() { profitableDevices[first] };
                        newGroup.Add(firstDev.UUID);
                        for (int second = first + 1; second < profitableDevices.Count; ++second) {
                            // check if we should group
                            var firstPair = profitableDevices[first];
                            var secondPair = profitableDevices[second];
                            if (GroupingLogic.ShouldGroup(firstPair, secondPair)) {
                                var secondDev = profitableDevices[second].Device;
                                newGroup.Add(secondDev.UUID);
                                miningPairs.Add(profitableDevices[second]);
                            }
                        }
                        currentGroupedDevices.Add(newGroup);
                        newGroupedMiningPairs[CalcGroupedDevicesKey(newGroup)] = miningPairs;
                    }
                }
            }
            //bool IsMinerStatsCheckUpdate = false;
            {
                // check which groupMiners should be stopped and which ones should be started and which to keep running
                Dictionary<string, GroupMiner> toStopGroupMiners = new Dictionary<string, GroupMiner>();
                Dictionary<string, GroupMiner> toRunNewGroupMiners = new Dictionary<string, GroupMiner>();
                // check what to stop/update
                foreach (string runningGroupKey in _runningGroupMiners.Keys) {
                    if (newGroupedMiningPairs.ContainsKey(runningGroupKey) == false) {
                        // runningGroupKey not in new group definately needs to be stopped and removed from curently running
                        toStopGroupMiners[runningGroupKey] = _runningGroupMiners[runningGroupKey];
                    } else {
                        // runningGroupKey is contained but needs to check if mining algorithm is changed
                        var miningPairs = newGroupedMiningPairs[runningGroupKey];
                        var newAlgoType = GetMinerPairAlgorithmType(miningPairs);
                        if(newAlgoType != AlgorithmType.NONE && newAlgoType != AlgorithmType.INVALID) {
                            // if algoType valid and different from currently running update
                            if (newAlgoType != _runningGroupMiners[runningGroupKey].AlgorithmType) {
                                // remove current one and schedule to stop mining
                                toStopGroupMiners[runningGroupKey] = _runningGroupMiners[runningGroupKey];
                                // create new one TODO check if DaggerHashimoto
                                GroupMiner newGroupMiner = null;
                                if (newAlgoType == AlgorithmType.DaggerHashimoto) {
                                    if (_ethminerNVIDIAPaused != null && _ethminerNVIDIAPaused.Key == runningGroupKey) {
                                        newGroupMiner = _ethminerNVIDIAPaused;
                                    }
                                    if (_ethminerAMDPaused != null && _ethminerAMDPaused.Key == runningGroupKey) {
                                        newGroupMiner = _ethminerAMDPaused;
                                    }
                                }
                                if (newGroupMiner == null) {
                                    newGroupMiner = new GroupMiner(miningPairs, runningGroupKey);
                                }
                                toRunNewGroupMiners[runningGroupKey] = newGroupMiner;
                            }
                        }
                    }
                }
                // check brand new
                foreach (var kvp in newGroupedMiningPairs) {
                    var key = kvp.Key;
                    var miningPairs = kvp.Value;
                    if (_runningGroupMiners.ContainsKey(key) == false) {
                        GroupMiner newGroupMiner = new GroupMiner(miningPairs, key);
                        toRunNewGroupMiners[key] = newGroupMiner;
                    }
                }
                // stop old miners
                foreach (var toStop in toStopGroupMiners.Values) {
                    toStop.Stop();
                    _runningGroupMiners.Remove(toStop.Key);
                    // TODO check if daggerHashimoto and save
                    if(toStop.AlgorithmType == AlgorithmType.DaggerHashimoto) {
                        if (toStop.DeviceType == DeviceType.NVIDIA) {
                            _ethminerNVIDIAPaused = toStop;
                        } else if (toStop.DeviceType == DeviceType.AMD) {
                            _ethminerAMDPaused = toStop;
                        }
                    }
                }
                // start new miners
                foreach (var toStart in toRunNewGroupMiners.Values) {
                    toStart.Start(_miningLocation, _btcAdress, _worker);
                    _runningGroupMiners[toStart.Key] = toStart;
                }
            }

            // stats quick fix code
            //if (_currentAllGroupedDevices.Count != _previousAllGroupedDevices.Count) {
                MinerStatsCheck(NiceHashData);
            //}
        }

        private AlgorithmType GetMinerPairAlgorithmType(List<MiningPair> miningPairs) {
            if (miningPairs.Count > 0) {
                return miningPairs[0].Algorithm.NiceHashID;
            }
            return AlgorithmType.NONE;
        }

        public void MinerStatsCheck(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            double CurrentProfit = 0.0d;
            _mainFormRatesComunication.ClearRates(_runningGroupMiners.Count);
            foreach (var groupMiners in _runningGroupMiners.Values) {
                Miner m = groupMiners.Miner;

                // skip if not running
                if (!m.IsRunning) continue;

                APIData AD = m.GetSummary();
                if (AD == null) {
                    Helpers.ConsolePrint(m.MinerTAG(), "GetSummary returned null..");
                }
                // set rates
                if (NiceHashData != null && AD != null) {
                    groupMiners.CurrentRate = NiceHashData[AD.AlgorithmID].paying * AD.Speed * 0.000000001;
                    if (NiceHashData.ContainsKey(AD.SecondaryAlgorithmID)) {
                        groupMiners.CurrentRate += NiceHashData[AD.SecondaryAlgorithmID].paying * AD.SecondarySpeed * 0.000000001;
                    }
                } else {
                    groupMiners.CurrentRate = 0;
                    // set empty
                    AD = new APIData(groupMiners.AlgorithmType);
                }
                CurrentProfit += groupMiners.CurrentRate;
                // Update GUI
                _mainFormRatesComunication.AddRateInfo(m.MinerTAG(), groupMiners.DevicesInfoString, AD, groupMiners.CurrentRate, m.IsAPIReadException);
            }
        }

    }
}
