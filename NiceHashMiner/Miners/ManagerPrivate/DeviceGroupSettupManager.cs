using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners {

    // typedefs
    using DeviceSubsetList = List<SortedSet<string>>;
    using PerDeviceProifitDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    using PerDeviceSpeedDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    using NiceHashMiner.Devices;
    public partial class MinersManager {
        private class DeviceGroupSettupManager {
            public DeviceGroupType GroupID { get; private set; }
            List<DeviceGroupSettup> _deviceGroupSettupList;
            List<AlgorithmType> _algorithmKeys;

            public DeviceGroupSettup MostProfitable {
                get { return _deviceGroupSettupList[_curMostProfitableIndex]; }
            }
            public DeviceGroupSettup LastMostProfitable {
                get { return _deviceGroupSettupList[_oldMostProfitableIndex]; }
            }

            public bool IsChange {
                get {
                    if (_oldMostProfitableIndex != _curMostProfitableIndex) {
                        return true;
                    }
                    return MostProfitable.IsChange;
                }
            }
            public bool IsMostProfitableSettupChange {
                get {
                    return _oldMostProfitableIndex != _curMostProfitableIndex && _oldMostProfitableIndex != -1;
                }
            }
            int _oldMostProfitableIndex = -1;
            int _curMostProfitableIndex = -1;

            public DeviceGroupSettupManager(DeviceGroupType deviceGroupType, HashSet<string> uniqueDeviceModelsUuids) {
                GroupID = deviceGroupType;

                if (uniqueDeviceModelsUuids.Count < 7) {
                    _deviceGroupSettupList = GetDeviceGroupSettups(uniqueDeviceModelsUuids);
                } else {
                    // only two options all grouped and all separated
                    DeviceGroupSettup allGrouped = new DeviceGroupSettup(uniqueDeviceModelsUuids);
                    allGrouped.AddSet(new SortedSet<string>(uniqueDeviceModelsUuids));
                    DeviceGroupSettup allSeparated = new DeviceGroupSettup(uniqueDeviceModelsUuids);
                    foreach (var uuid in uniqueDeviceModelsUuids) {
                        allSeparated.AddSet(new SortedSet<string>(new string[] { uuid }));
                    }

                    _deviceGroupSettupList = new List<DeviceGroupSettup>();
                    _deviceGroupSettupList.Add(allGrouped);
                    _deviceGroupSettupList.Add(allSeparated);
                }
                _algorithmKeys = GroupAlgorithms.GetAlgorithmKeysForGroup(deviceGroupType);
                // initialize GroupProfits
                foreach (var settup in _deviceGroupSettupList) {
                    settup.InitializePerGroupProfit(GroupID);
                }
            }

            public void CalculateMostProfitable(PerDeviceProifitDictionary perDeviceProifit) {
                _oldMostProfitableIndex = _curMostProfitableIndex;
                double currentMaxProfit = double.MinValue;
                int curIndex = 0;
                foreach (var settup in _deviceGroupSettupList) {
                    settup.CalculateProfit(perDeviceProifit, _algorithmKeys);
                    if (settup.GroupProfit > currentMaxProfit
                        || (settup.GroupProfit == currentMaxProfit
                        && settup.DeviceUUIDs.Count < MostProfitable.DeviceUUIDs.Count)
                        ) {
                        _curMostProfitableIndex = curIndex;
                        currentMaxProfit = settup.GroupProfit;
                    }
                    ++curIndex;
                }
            }
        }
    }
}
