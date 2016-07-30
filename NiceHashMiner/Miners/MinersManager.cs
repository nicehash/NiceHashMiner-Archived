using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
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

            foreach (var kvp in _allMiners) {
                Miner m = kvp.Value;
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

        public double GetTotalRate() {
            double TotalRate = 0;
            // TODO 
            foreach (var kvp in _allMiners) {
                TotalRate += kvp.Value.CurrentRate;
            }

            return TotalRate;
        }

        /// <summary>
        /// SwichMostProfitable should check the best combination for most profit.
        /// #1 Calculate profit for each suported algorithm per single device.
        /// #2 Calculate profit for each supported algorithm per device group.
        /// 
        /// #3 Device groups are CPU, AMD_OpenCL and NVIDIA CUDA SM.x.x.
        /// NVIDIA SMx.x should be paired separately except for daggerhashimoto.
        /// </summary>
        /// <param name="NiceHashData"></param>
        public void SwichMostProfitable(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            // TODO maybe allocate this only once
            Dictionary<string, Dictionary<AlgorithmType, double>> perDeviceProfit = new Dictionary<string, Dictionary<AlgorithmType, double>>();
            // calculate per device profit
            foreach (var cdev in ComputeDevice.UniqueAvaliableDevices) {
                Dictionary<AlgorithmType, double> profits = new Dictionary<AlgorithmType, double>();
                var deviceConfig = DeviceBenchmarkConfigManager.Instance.GetConfig(cdev.Name);

                foreach (var kvp in deviceConfig.BenchmarkSpeeds) {
                    var key = kvp.Key;
                    var algorithm = kvp.Value;
                    // TODO what is the constant at the end?
                    if (algorithm.Skip) {
                        // for now set to negative value as not profitable
                        profits.Add(key, -1);
                    } else {
                        profits.Add(key, algorithm.BenchmarkSpeed * NiceHashData[key].paying * 0.000000001);
                    }
                }
                perDeviceProfit.Add(cdev.Name, profits);
            }
            Dictionary<string, Dictionary<AlgorithmType, double>> perGroupProfit = new Dictionary<string, Dictionary<AlgorithmType, double>>();

        }

    }
}
