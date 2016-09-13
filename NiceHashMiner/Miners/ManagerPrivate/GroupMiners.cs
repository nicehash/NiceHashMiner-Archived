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
            public Miner CurrentWorkingMiner { get; protected set; }
            public string DevicesInfoString { get; private set; }

            protected List<Miner> _miners;
            protected SortedSet<string> _deviceNames;
            protected string[] _deviceUUIDs;
            protected DeviceGroupType _deviceGroupType = DeviceGroupType.NONE;

            public GroupMiners(GroupedDevices deviceUUIDSet) {
                _miners = new List<Miner>();
                _deviceNames = new SortedSet<string>();
                foreach (var uuid in deviceUUIDSet) {
                    var tmpCdev = ComputeDevice.GetDeviceWithUUID(uuid);
                    _deviceNames.Add(tmpCdev.NameCount);
                    if (_deviceGroupType == DeviceGroupType.NONE) {
                        _deviceGroupType = tmpCdev.DeviceGroupType;
                    }
                }
                // init device uuids
                _deviceUUIDs = deviceUUIDSet.ToArray();
                // init DevicesInfoString
                string[] _deviceNamesCount = new string[_deviceNames.Count];
                {
                    int i = 0;
                    foreach (var devName in _deviceNames) {
                        _deviceNamesCount[i++] =
                            ComputeDevice.GetEnabledDeviceNameCount(devName).ToString()
                            + " * " + devName;
                    }
                }

                DevicesInfoString = "{ " + string.Join(", ", _deviceNames) + " }";
            }

            private void StopMiner(Miner miner) {
                if (miner.IsRunning) {
                    miner.Stop(MinerStopType.SWITCH);
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
                    miner.End();
                }
            }

            public void StartAlgorihtm(Algorithm algorithm, string miningLocation, string worker) {
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
                if (!containsSupportedMiner && _deviceGroupType != DeviceGroupType.CPU) {
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
                SwitchMinerAlgorithm(ref startSwitchMiner, algorithm, miningLocation, worker);
            }

            private void SwitchMinerAlgorithm(ref Miner m, Algorithm algorithm, string miningLocation, string worker) {
                // if is running and the current algorithm is the same skip
                if (m.IsRunning && m.CurrentAlgorithmType == algorithm.NiceHashID) {
                    return;
                }

                if (m.CurrentAlgorithmType != AlgorithmType.NONE && m.CurrentAlgorithmType != AlgorithmType.INVALID) {
                    m.Stop(MinerStopType.SWITCH);
                    // wait 0.5 seconds before going on
                    System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                }

                var MaxProfitKey = algorithm.NiceHashID;

                // Wait before new start
                System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);

                m.Start(algorithm,
                    "stratum+tcp://"
                    + Globals.NiceHashData[MaxProfitKey].name
                    + "." + miningLocation
                    + ".nicehash.com:"
                    + Globals.NiceHashData[MaxProfitKey].port, worker);
            }

        }
    }
}
