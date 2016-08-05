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
    public partial class MinersManager {
        /// <summary>
        /// DeviceGroupSettup class saves a list of all subsets that make the full set.
        /// The group settup calculates current profit per grouped sets.
        /// For optimal performance use Unique device uuid sets.
        /// </summary>
        private class DeviceGroupSettup {
            public DeviceSubsetList DeviceUUIDs { get; private set; }
            public List<GroupProfit> PerGroupProfit { get; private set; }
            public double GroupProfit { get; set; }

            public bool IsChange {
                get {
                    foreach (var groupProfit in PerGroupProfit) {
                        if (groupProfit.IsChange) return true;
                    }
                    return false;
                }
            }

            private HashSet<string> _fullSet;
            // helper variable 
            private HashSet<string> _checkSet;

            public DeviceGroupSettup(HashSet<string> fullSet) {
                _fullSet = fullSet;
                _checkSet = new HashSet<string>();
                DeviceUUIDs = new DeviceSubsetList();
                PerGroupProfit = new List<GroupProfit>();
            }

            public void CalculateProfit(PerDeviceProifitDictionary perDeviceProifit, List<AlgorithmType> groupAlgorithms) {
                GroupProfit = 0;
                foreach (var groupProfit in PerGroupProfit) {
                    groupProfit.CalculateMostProfitable(perDeviceProifit, groupAlgorithms);
                    GroupProfit += groupProfit.Profit;
                }
            }

            public void InitializePerGroupProfit(DeviceGroupType deviceGroupType) {
                // if it is a valid GroupSettup initialize GroupProfits 
                if (IsValid() && PerGroupProfit.Count == 0) {
                    foreach (var groupSet in DeviceUUIDs) {
                        PerGroupProfit.Add(new GroupProfit(groupSet, deviceGroupType));
                    }
                }
            }

            #region Settup and validation code
            public void AddSet(SortedSet<string> newSet) {
                // we only want to add sets of devices that arent in the list
                bool shouldAdd = true;
                foreach (var curSet in DeviceUUIDs) {
                    // check set intersection it must return a zero set to be a valid add candidate
                    if (curSet.Intersect(newSet).Count() != 0) {
                        shouldAdd = false;
                        break;
                    }
                }
                if (shouldAdd && _fullSet.Intersect(newSet).Count() > 0) {
                    DeviceUUIDs.Add(newSet);
                    _checkSet.UnionWith(newSet);
                }
            }

            public bool IsValid() {
                return _checkSet.Count == _fullSet.Count && _fullSet.Count == _checkSet.Intersect(_fullSet).Count();
            }

            public void UnionWith(DeviceGroupSettup dgs) {
                this._fullSet.UnionWith(dgs._fullSet);
                foreach (var group in dgs.DeviceUUIDs) {
                    this.AddSet(group);
                }
            }
            #endregion // Settup and validation code
        }
    }
}
