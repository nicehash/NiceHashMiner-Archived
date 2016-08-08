using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    using NiceHashMiner.Enums;
    using NiceHashMiner.Devices;
    using NiceHashMiner.Configs;
    // typedefs
    using GroupedDevices = SortedSet<string>;
    using GroupedDevicesKey = SortedSet<SortedSet<string>>;
    
    public partial class MinersManager {
        // for switching different miners for different algorithms
        private class GroupMiners {
            public Miner CurrentWorkingMiner { get; private set; }
            List<Miner> _miners;
            private string[] _deviceNames;
            private string[] _deviceUUIDs;
            private DeviceGroupType _deviceGroupType = DeviceGroupType.NONE;
            public string DevicesInfoString { get; private set; }

            public GroupMiners(GroupedDevices deviceUUIDSet) {
                _miners = new List<Miner>();
                _deviceNames = new string[deviceUUIDSet.Count];
                int devNamesIndex = 0;
                foreach (var uuid in deviceUUIDSet) {
                    var tmpCdev = ComputeDevice.GetDeviceWithUUID(uuid);
                    _deviceNames[devNamesIndex++] = tmpCdev.Name;
                    if (_deviceGroupType == DeviceGroupType.NONE) {
                        _deviceGroupType = tmpCdev.DeviceGroupType;
                    }
                }
                // init device uuids
                _deviceUUIDs = ComputeDevice.GetEnabledDevicesUUUIDsForNames(_deviceNames);
                // init DevicesInfoString
                string[] _deviceNamesCount = new string[_deviceNames.Length];
                for (int i = 0; i < _deviceNames.Length; ++i ) {
                    var devName = _deviceNames[i];
                    _deviceNamesCount[i] =
                        ComputeDevice.GetDeviceNameCount(devName).ToString()
                        + " * " + devName;
                }
                DevicesInfoString = "{ " + string.Join(", ", _deviceNamesCount) + " }";
            }

            private void StopMiner(Miner miner) {
                if (miner.IsRunning) {
                    miner.Stop(true);
                    // wait 0.5 seconds before going on
                    System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                }
            }

            public void Stop() {
                foreach (var miner in _miners) {
                    StopMiner(miner);
                }
            }

            public void End() {
                foreach (var miner in _miners) {
                    if (miner.IsRunning) {
                        miner.Stop(false);
                        miner.IsRunning = false;
                        miner.CurrentAlgo = AlgorithmType.NONE;
                        miner.CurrentRate = 0;
                    }
                }
            }

            public void StartAlgorihtm(Algorithm algorithm, string Worker) {
                bool containsSupportedMiner = false;
                Miner startSwitchMiner = null;
                var algorithmType = algorithm.NiceHashID;
                foreach (var miner in _miners) {
                    if (miner.IsSupportedMinerAlgorithms(algorithmType)) {
                        containsSupportedMiner = true;
                        startSwitchMiner = miner;
                        break;
                    }
                }
                // check if contains miner if not create one
                if (!containsSupportedMiner) {
                    startSwitchMiner = CreateMiner(_deviceGroupType, algorithmType);
                    startSwitchMiner.SetCDevs(_deviceUUIDs);
                    _miners.Add(startSwitchMiner);
                }

                // hanlde CurrentWorkingMiner change
                if (CurrentWorkingMiner != null && CurrentWorkingMiner != startSwitchMiner) {
                    StopMiner(CurrentWorkingMiner);
                    CurrentWorkingMiner = startSwitchMiner;
                } else {
                    CurrentWorkingMiner = startSwitchMiner;
                }
                SwitchMinerAlgorithm(ref startSwitchMiner, algorithm, Worker);
            }

            private void SwitchMinerAlgorithm(ref Miner m, Algorithm algorithm, string Worker) {
                // if is running and the current algorithm is the same skip
                if (m.IsRunning && m.CurrentAlgo == algorithm.NiceHashID) {
                    return;
                }

                if (m.CurrentAlgo != AlgorithmType.NONE && m.CurrentAlgo != AlgorithmType.INVALID) {
                    m.Stop(true);
                    // wait 0.5 seconds before going on
                    System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                }

                m.CurrentAlgo = algorithm.NiceHashID;
                var MaxProfitKey = algorithm.NiceHashID;

                m.Start(algorithm,
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
