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

            // group device combinations
            // nvidia group set of uuids
            var nvidiaGroup = ComputeDeviceGroupManager.Instance.GetEnabledDevicesUUIDsForGroup(DeviceGroupType.NVIDIA_5_x);
            // group combinations
            var groups = GetAllSubsets(nvidiaGroup);
            // calculate per group
            Dictionary<SortedSet<string>, Dictionary<AlgorithmType, double>> perGroupHashRate = new Dictionary<SortedSet<string>, Dictionary<AlgorithmType, double>>();
            foreach (var group in groups) {
                List<DeviceBenchmarkConfig> benchmarks = new List<DeviceBenchmarkConfig>();
                bool areAllDevsEnabled = true;
                foreach (var uuid in group) {
                    var device = ComputeDevice.GetDeviceWithUUID(uuid);
                    if (device.Enabled) {
                        benchmarks.Add(DeviceBenchmarkConfigManager.Instance.GetConfig(device.Name));
                    } else {
                        areAllDevsEnabled = false;
                        break;
                    }
                }
                if (areAllDevsEnabled && benchmarks.Count > 0) {
                    Dictionary<AlgorithmType, double> currentGroupHashRate = new Dictionary<AlgorithmType,double>();
                    // firs add keys and initialize to 0
                    foreach (var key in benchmarks.First().BenchmarkSpeeds.Keys) {
                        currentGroupHashRate.Add(key, 0.0d);
                    }
                    foreach (var bench in benchmarks) {
                        foreach (var kvp in bench.BenchmarkSpeeds) {
                            currentGroupHashRate[kvp.Key] += kvp.Value.BenchmarkSpeed;
                        }
                    }

                    perGroupHashRate.Add(group, currentGroupHashRate);
                }
            }
            
            // calculate most profitable
            var currentProfit = MemoryHelper.DeepClone(perGroupHashRate);
            foreach (var groupHashKvp in perGroupHashRate) {

            }
        }

        // brute force method for getting all subsets from set
        private List<SortedSet<string>> GetAllSubsets(HashSet<string> hashSet) {
            List<SortedSet<string>> allSubsets = new List<SortedSet<string>>();

            List<string> mainSet = hashSet.ToList();
            // sort easier to debug
            mainSet.Sort();

            var retVal = GetAllSubsets(new List<string>(), mainSet);
            foreach (var l in retVal) {
                // we don't want empty sets
                if(l.Count != 0) {
                    allSubsets.Add(new SortedSet<string>(l));
                }
            }

            return allSubsets;
        }

        /// <summary>
        /// GetAllSubsets recursive function to get all subsetts from a set
        /// usage GetAllSubsets(empty, all);
        /// </summary>
        /// <param name="soFar"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        private List<List<string>> GetAllSubsets(List<string> soFar, List<string> rest) {
            if (rest.Count == 0) {
                var ret = new List<List<string>>();
                ret.Add(soFar);
                return ret;
            } else {
                var ret1 = new List<List<string>>();
                var ret2 = new List<List<string>>();
                var newSofar = new List<string>(soFar);
                newSofar.AddRange(rest.GetRange(0, 1));
                ret1.AddRange(
                    GetAllSubsets(newSofar,
                    rest.GetRange(1, rest.Count-1) )
                    );
                ret2.AddRange(
                    GetAllSubsets(soFar,
                    rest.GetRange(1, rest.Count - 1))
                    );
                ret1.AddRange(ret2.GetRange(0, ret2.Count));
                return ret1;
            }
        }

    }
}
