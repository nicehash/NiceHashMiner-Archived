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


        /// TODO replace this maybe with liner-integer programming solution.
        /// Current solution is only feasible for a group of 7 MAX!
        /// look maybe at Microsoft Solver Foundation 3.1
        #region Miner settups set combinations logic

        #region PowerSet functions
        /// <summary>
        /// GetAllSubsets returns a set of sets, basically a PowerSet without Identity or null set
        /// </summary>
        /// <param name="hashSet"></param>
        /// <returns></returns>
        static private List<SortedSet<string>> GetAllSubsets(HashSet<string> hashSet) {
            List<SortedSet<string>> allSubsets = new List<SortedSet<string>>();

            List<string> mainSet = hashSet.ToList();
            // sort easier to debug
            mainSet.Sort();

            var retVal = GetAllSubsets(new List<string>(), mainSet);
            foreach (var l in retVal) {
                // we don't want empty sets
                if (l.Count != 0) {
                    allSubsets.Add(new SortedSet<string>(l));
                }
            }
            allSubsets.Sort((a, b) => a.Count - b.Count);

            return allSubsets;
        }

        /// <summary>
        /// GetAllSubsets recursive function to get all subsetts or a PowerSet from a set
        /// usage GetAllSubsets(empty, all);
        /// </summary>
        /// <param name="soFar"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        static private List<List<string>> GetAllSubsets(List<string> soFar, List<string> rest) {
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
                    rest.GetRange(1, rest.Count - 1))
                    );
                ret2.AddRange(
                    GetAllSubsets(soFar,
                    rest.GetRange(1, rest.Count - 1))
                    );
                ret1.AddRange(ret2.GetRange(0, ret2.Count));
                return ret1;
            }
        }
        #endregion //PowerSet functions

        /// <summary>
        /// GetDeviceGroupSettups returns all valid group settups.
        /// IMPORTANT!!! Do not run this function for a set bigger then 7.
        /// In case there are bigger sets, use the fallback method
        /// </summary>
        /// <param name="hashSet"></param>
        /// <returns></returns>
        static private List<DeviceGroupSettup> GetDeviceGroupSettups(HashSet<string> hashSet) {
            List<DeviceGroupSettup> settups = new List<DeviceGroupSettup>();

            // group subsets by their count numbers
            Dictionary<int, DeviceSubsetList> countGroupedSubsetLists = new Dictionary<int, DeviceSubsetList>();
            // init countGroupedSubsetLists scope
            {
                DeviceSubsetList deviceSubsetList = GetAllSubsets(hashSet);
                // init countGroupedSubsetLists, 
                for (int count = 1; count <= hashSet.Count; ++count) {
                    countGroupedSubsetLists.Add(count, new DeviceSubsetList());
                }
                // fill countGroupedSubsetLists
                foreach (var uuidsSet in deviceSubsetList) {
                    countGroupedSubsetLists[uuidsSet.Count].Add(uuidsSet);
                }
            }
            // init settups scope
            {
                List<int[]> validCountCombinations = GetCombinationsCount(hashSet.Count);
                foreach (var validSettup in validCountCombinations) {
                    var tmp = BuildUpSetCombinations(validSettup, hashSet, countGroupedSubsetLists);
                    settups.AddRange(tmp);
                }
            }

            return settups;
        }

        /// <summary>
        /// BuildUpSetCombinations builds a list of validSettup combinations for a given targetSet.
        /// </summary>
        /// <param name="validSettup"></param>
        /// <param name="remainderSet"></param>
        /// <param name="countGroupedSubsetLists"></param>
        /// <returns></returns>
        static private List<DeviceGroupSettup> BuildUpSetCombinations(int[] validSettup, HashSet<string> remainderSet, Dictionary<int, DeviceSubsetList> countGroupedSubsetLists) {
            // check if it is return
            if (remainderSet.Count == 0 || IsValid(validSettup, 0)) {
                return new List<DeviceGroupSettup>(); // return Unit
            }

            var retList = new List<DeviceGroupSettup>();

            int minValue = int.MaxValue;
            int minIndex = -1;
            // get min nonzero index with biggest index value
            for (int i = 0; i < validSettup.Length; ++i) {
                if (minValue > validSettup[i] && validSettup[i] != 0) {
                    minValue = validSettup[i];
                    minIndex = i;
                }
                if (minIndex < i && minValue == validSettup[i]) {
                    minIndex = i;
                }
            }
            var minCountID = minIndex + 1;
            int minElementsCount = minCountID * minValue;

            if (minElementsCount == remainderSet.Count) {
                int end = (remainderSet.Count - minElementsCount) + 1;
                for (int start = 0; start < end; ++start) {
                    var newDeviceGroupSettup = new DeviceGroupSettup(remainderSet);
                    var startSubSet = countGroupedSubsetLists[minCountID][start];
                    foreach (var addSubsetCandidate in countGroupedSubsetLists[minCountID]) {
                        newDeviceGroupSettup.AddSet(addSubsetCandidate);
                        if (newDeviceGroupSettup.IsValid()) break;
                    }
                    if (newDeviceGroupSettup.IsValid()) {
                        retList.Add(newDeviceGroupSettup);
                    }
                }
            } else {
                foreach (var curSubSet in countGroupedSubsetLists[minCountID]) {
                    var newDeviceGroupSettup = new DeviceGroupSettup(remainderSet);
                    newDeviceGroupSettup.AddSet(curSubSet);
                    // get complement set
                    var newRemainderSet = new HashSet<string>(remainderSet.Except(curSubSet));
                    var newValidSettup = MemoryHelper.DeepClone(validSettup);
                    newValidSettup[minIndex]--;
                    var remainderList = BuildUpSetCombinations(newValidSettup, newRemainderSet, countGroupedSubsetLists);

                    foreach (var part in remainderList) {
                        part.UnionWith(newDeviceGroupSettup);
                        if (part.IsValid()) {
                            retList.Add(part);
                        }
                    }
                }
            }

            return retList;
        }

        #region Coin changing problem algo
        /// Here are the methods for the uuid grouping
        /// The algorithm set is from 0-setCount

        // do NOT RUN THIS FUNCTION IF target is > 11 it might take too long on some machines
        static List<int[]> GetCombinationsCount(int setCount) {
            int[] limitsMaxCounts = new int[setCount];
            for (int i = 0; i < setCount; ++i) {
                limitsMaxCounts[i] = (setCount / (i + 1));
            }

            List<int[]> candidateResults = new List<int[]>();
            // init candidate list scope
            {
                long cantitateListCount = 1;
                foreach (var tmpLimit in limitsMaxCounts) cantitateListCount *= (tmpLimit + 1);
                for (long i = 0; i < cantitateListCount; ++i) {
                    candidateResults.Add(new int[setCount]);
                }
            }
            // fill candidates scope
            {
                int canditateListIndex = 0;
                int[] limitsIndexes = new int[setCount];
                while (canditateListIndex < candidateResults.Count) {
                    for (int limitsIndex = 0; limitsIndex < limitsIndexes.Length; ++limitsIndex) {
                        candidateResults[canditateListIndex][limitsIndex] = limitsIndexes[limitsIndex];
                    }
                    Increment(ref limitsIndexes, limitsMaxCounts, 0);
                    ++canditateListIndex;
                }
            }
            // filter invalid
            List<int[]> results = new List<int[]>();
            List<int> cprodResults = new List<int>();
            foreach (var candidate in candidateResults) {
                if (IsValid(candidate, setCount)) {
                    results.Add(candidate);
                }
            }

            return results;
        }

        static bool IsValid(int[] candidate, int setCount) {
            int sum = 0;
            for (int i = 0; i < candidate.Length; ++i) {
                sum += candidate[i] * (i + 1);
            }
            return sum == setCount;
        }

        static void Increment(ref int[] limitsIndexes, int[] limitsIndexesMax, int indexIncrement) {
            if (indexIncrement >= limitsIndexes.Length) return;
            limitsIndexes[indexIncrement]++;
            if (limitsIndexes[indexIncrement] > limitsIndexesMax[indexIncrement]) {
                limitsIndexes[indexIncrement] = 0;
                Increment(ref limitsIndexes, limitsIndexesMax, indexIncrement + 1);
            }
        }

        #endregion //Coin changing problem algo

        #endregion Miner settups set combinations logic


    }
}
