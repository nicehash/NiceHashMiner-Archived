using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class MinersManager : BaseLazySingleton<MinersManager> {

        public class MinerKey {
            public int DeviceID;
            public string Group;
            public string DeviceName;
            public DeviceGroupType DeviceGroupType;
        }
        // we save cpu miners
        Dictionary<MinerKey, cpuminer> _cpuMiners = new Dictionary<MinerKey,cpuminer>();

        // gpu miner are made as we go
        Dictionary<MinerKey, ccminer> _ccminerMiners;


        Dictionary<MinerKey, Miner> _allMiners = new Dictionary<MinerKey,Miner>();

        protected MinersManager() {
        }


        public void AddCpuMiner(cpuminer miner, int deviceID, string deviceName) {
            MinerKey key = new MinerKey() {
                DeviceID = deviceID,
                Group = miner.MinerDeviceName,
                DeviceName = deviceName,
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

            foreach (var kp in _allMiners) {
                Miner m = kp.Value;
                if (m.IsRunning) {
                    ActiveMinersGroup += m.MinerDeviceName + "/";
                }
            }

            return ActiveMinersGroup;
        }

        // TODO check how will it work for double CPU settup
        public Miner CreateBenchmarkMiner(DeviceGroupType type) {
            
            // return first cpuminer
            if (DeviceGroupType.CPU == type && _cpuMiners.Count > 0) {
                return _cpuMiners[_cpuMiners.Keys.First()];
            }

            switch (type) {
                case DeviceGroupType.AMD_OpenCL:
                    return new sgminer(false);
                case DeviceGroupType.NVIDIA_2_1:
                    return new ccminer_tpruvot_sm21(false);
                case DeviceGroupType.NVIDIA_3_x:
                    return new ccminer_tpruvot(false);
                case DeviceGroupType.NVIDIA_5_x:
                    return new ccminer_sp(false);
            }

            return null;
        }

        

    }
}
