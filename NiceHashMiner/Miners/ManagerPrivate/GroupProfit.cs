using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    using NiceHashMiner.Configs;
    using PerDeviceProifitDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    public partial class MinersManager {
        private class GroupProfit {
            private string[] _deviceNames;
            public double Profit { get; private set; }
            public AlgorithmType MostProfitAlgorithmType { get; private set; }
            public Algorithm MostProfitAlgorithm { get; private set; }

            private AlgorithmType PrevMostProfitAlgorithmType = AlgorithmType.NONE;
            public bool IsChange { get { return PrevMostProfitAlgorithmType != MostProfitAlgorithmType; } }
            public MinerKey MinerKey { get; private set; }
            public string DevicesInfoString { get; private set; }


            public void CalculateMostProfitable(PerDeviceProifitDictionary perDeviceProifit, List<AlgorithmType> groupAlgorithms) {
                Profit = double.MinValue;
                foreach (var algoKey in groupAlgorithms) {
                    double newProfitValue = 0;
                    foreach (var devName in _deviceNames) {
                        newProfitValue += perDeviceProifit[devName][algoKey];
                    }
                    if (IsAlgorithmSettingsSame(algoKey) && newProfitValue > Profit) {
                        Profit = newProfitValue;
                        MostProfitAlgorithmType = algoKey;
                        MostProfitAlgorithm = DeviceBenchmarkConfigManager.Instance.GetConfig(_deviceNames.First()).AlgorithmSettings[algoKey];
                    }
                }
            }

            private bool IsAlgorithmSettingsSame(AlgorithmType algorithmType) {
                var deviceBCM = DeviceBenchmarkConfigManager.Instance;
                Algorithm curAlgorithm = deviceBCM.GetConfig(_deviceNames.First()).AlgorithmSettings[algorithmType];
                for (int i = 1; i < _deviceNames.Length; ++i) {
                    Algorithm compareAlgorithm = deviceBCM.GetConfig(_deviceNames[i]).AlgorithmSettings[algorithmType];
                    // TODO make sure these strings are Trimmed
                    if (string.IsNullOrEmpty(curAlgorithm.UsePassword) != string.IsNullOrEmpty(compareAlgorithm.UsePassword)) return false;
                    // TODO make sure these strings are Trimmed and sorted the same way
                    if (string.IsNullOrEmpty(curAlgorithm.ExtraLaunchParameters) != string.IsNullOrEmpty(compareAlgorithm.ExtraLaunchParameters)) return false;
                    //if (curAlgorithm.Intensity != compareAlgorithm.Intensity) return false;
                }

                return true;
            }

            public GroupProfit(SortedSet<string> deviceUUIDSet, DeviceGroupType deviceGroupType) {
                _deviceNames = new string[deviceUUIDSet.Count];
                int devNamesIndex = 0;
                foreach (var uuid in deviceUUIDSet) {
                    _deviceNames[devNamesIndex++] = ComputeDevice.GetDeviceWithUUID(uuid).Name;
                }
                MostProfitAlgorithmType = AlgorithmType.NONE;
                MinerKey = new MinerKey() {
                    DeviceUUIDs = ComputeDevice.GetEnabledDevicesUUUIDsForNames(_deviceNames),
                    Group = GroupNames.GetName(deviceGroupType),
                    DeviceGroupType = deviceGroupType
                };
                DevicesInfoString = "{ ";
                foreach (var devName in _deviceNames) {
                    DevicesInfoString +=
                        ComputeDevice.GetDeviceNameCount(devName).ToString() 
                        + " * " + devName + ", ";
                }
                DevicesInfoString = DevicesInfoString.TrimEnd(',') + "}";
            }
        }
    }
}
