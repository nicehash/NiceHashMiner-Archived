using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;
using NiceHashMiner.Configs;

namespace NiceHashMiner.Miners {

    // typedefs
    using MinersManager = MinersManager_NEW;
    using GroupedDevices = SortedSet<string>;
    // for switching different miners for different algorithms
    public class GroupMiners {
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
                var tmpCdev = ComputeDeviceManager.Avaliable.GetDeviceWithUUID(uuid);
                _deviceNames.Add(tmpCdev.NameCount);
                if (_deviceGroupType == DeviceGroupType.NONE) {
                    _deviceGroupType = tmpCdev.DeviceGroupType;
                }
            }
            // init device uuids
            _deviceUUIDs = deviceUUIDSet.ToArray();

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

        public void StartAlgorihtm(Algorithm algorithm, string miningLocation, string btcAdress, string worker) {
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
                startSwitchMiner = MinersManager.CreateMiner(_deviceGroupType, algorithmType);
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
            SwitchMinerAlgorithm(ref startSwitchMiner, algorithm, miningLocation, btcAdress, worker);
        }

        private void SwitchMinerAlgorithm(ref Miner m, Algorithm algorithm, string miningLocation, string btcAdress, string worker) {
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

            if (MaxProfitKey == AlgorithmType.Equihash) {
                if (m.IsNHLocked) {
                    m.Start(algorithm, miningLocation, btcAdress, worker);
                } else {
                    m.Start(algorithm,
                    Globals.NiceHashData[MaxProfitKey].name
                    + "." + miningLocation
                    + ".nicehash.com:"
                    + Globals.NiceHashData[MaxProfitKey].port, btcAdress, worker);
                }
            } else {
                m.Start(algorithm,
                "stratum+tcp://"
                + Globals.NiceHashData[MaxProfitKey].name
                + "." + miningLocation
                + ".nicehash.com:"
                + Globals.NiceHashData[MaxProfitKey].port, btcAdress, worker);
            }
        }
    }
}
