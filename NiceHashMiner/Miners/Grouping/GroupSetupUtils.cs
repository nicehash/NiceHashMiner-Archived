using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Grouping {
    public static class GroupSetupUtils {
        static readonly string TAG = "GroupSetupUtils";
        public static bool IsAlgoMiningCapable(Algorithm algo) {
            return algo != null && !algo.Skip && algo.BenchmarkSpeed > 0;
        }
        public static bool IsValidMinerPath(ComputeDevice device, Algorithm algo) {
            return MinerPaths.NONE != MinerPaths.GetOptimizedMinerPath(device, algo);
        }

        public static Tuple<ComputeDevice, DeviceMiningStatus> getDeviceMiningStatus(ComputeDevice device) {
            DeviceMiningStatus status = DeviceMiningStatus.CanMine;
            if (device == null) { // C# is null happy
                status = DeviceMiningStatus.DeviceNull;
            } else if (device.Enabled == false) {
                status = DeviceMiningStatus.Disabled;
            } else {
                bool hasEnabledAlgo = false;
                foreach (Algorithm algo in device.AlgorithmSettings.Values) {
                    hasEnabledAlgo |= IsAlgoMiningCapable(algo) && IsValidMinerPath(device, algo);
                }
                if (hasEnabledAlgo == false) {
                    status = DeviceMiningStatus.NoEnabledAlgorithms;
                }
            }
            return new Tuple<ComputeDevice, DeviceMiningStatus>(device, status);
        }

        private static Tuple<List<MiningDevice>, List<Tuple<ComputeDevice, DeviceMiningStatus>>> GetMiningAndNonMiningDevices(List<ComputeDevice> devices) {
            List<Tuple<ComputeDevice, DeviceMiningStatus>> nonMiningDevStatuses = new List<Tuple<ComputeDevice, DeviceMiningStatus>>();
            List<MiningDevice> miningDevices = new List<MiningDevice>();
            foreach (var dev in devices) {
                var devStatus = getDeviceMiningStatus(dev);
                if (devStatus.Item2 == DeviceMiningStatus.CanMine) {
                    miningDevices.Add(new MiningDevice(dev));
                } else {
                    nonMiningDevStatuses.Add(devStatus);
                }
            }
            return new Tuple<List<MiningDevice>, List<Tuple<ComputeDevice, DeviceMiningStatus>>>(miningDevices, nonMiningDevStatuses);
        }

        private static string GetDisabledDeviceStatusString(Tuple<ComputeDevice, DeviceMiningStatus> devStatusTuple) {
            var dev = devStatusTuple.Item1;
            var status = devStatusTuple.Item2;
            if (DeviceMiningStatus.DeviceNull == status) {
                return "Passed Device is NULL";
            } else if (DeviceMiningStatus.Disabled == status) {
                return "DISABLED: " + dev.GetFullName();
            } else if (DeviceMiningStatus.NoEnabledAlgorithms == status) {
                return "No Enabled Algorithms: " + dev.GetFullName();
            }
            return "Invalid status Passed";
        }
        private static void LogMiningNonMiningStatuses(List<MiningDevice> enabledDevices, List<Tuple<ComputeDevice, DeviceMiningStatus>> disabledDevicesStatuses) {
            // print statuses
            if (disabledDevicesStatuses.Count > 0) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("Disabled Devices:");
                foreach (var deviceStatus in disabledDevicesStatuses) {
                    stringBuilder.AppendLine("\t" + GetDisabledDeviceStatusString(deviceStatus));
                }
                Helpers.ConsolePrint(TAG, stringBuilder.ToString());
            }
            if (enabledDevices.Count > 0) {
                // print enabled
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("Enabled Devices for Mining session:");
                foreach (var miningDevice in enabledDevices) {
                    var device = miningDevice.Device;
                    stringBuilder.AppendLine(String.Format("\tENABLED ({0})", device.GetFullName()));
                    foreach (var algo in device.AlgorithmSettings.Values) {
                        var isEnabled = IsAlgoMiningCapable(algo) && IsValidMinerPath(device, algo);
                        stringBuilder.AppendLine(String.Format("\t\tALGORITHM {0} ({1})",
                            isEnabled ? "ENABLED " : "DISABLED", // ENABLED/DISABLED
                            AlgorithmNiceHashNames.GetName(algo.NiceHashID)));
                    }
                }
                Helpers.ConsolePrint(TAG, stringBuilder.ToString());
            }
        }

        public static List<MiningDevice> GetMiningDevices(List<ComputeDevice> devices, bool log) {
            var miningNonMiningDevs = GetMiningAndNonMiningDevices(devices);
            if(log) {
                LogMiningNonMiningStatuses(miningNonMiningDevs.Item1, miningNonMiningDevs.Item2);
            }
            return miningNonMiningDevs.Item1;
        }

        // avarage passed in benchmark values
        public static void AvarageSpeeds(List<MiningDevice> miningDevs) {
            // calculate avarage speeds, to ensure mining stability
            // device name, algo key, algos refs list
            Dictionary<string,
                Dictionary<AlgorithmType,
                List<MiningAlgorithm>>> avarager = new Dictionary<string, Dictionary<AlgorithmType, List<MiningAlgorithm>>>();
            // init empty avarager
            foreach (var device in miningDevs) {
                string devName = device.Device.Name;
                avarager[devName] = new Dictionary<AlgorithmType, List<MiningAlgorithm>>();
                foreach (var key in AlgorithmNiceHashNames.GetAllAvaliableTypes()) {
                    avarager[devName][key] = new List<MiningAlgorithm>();
                }
            }
            // fill avarager
            foreach (var device in miningDevs) {
                string devName = device.Device.Name;
                foreach (var kvp in device.Algorithms) {
                    var key = kvp.Key;
                    MiningAlgorithm algo = kvp.Value;
                    avarager[devName][key].Add(algo);
                }
            }
            // calculate avarages
            foreach (var devDict in avarager.Values) {
                foreach (List<MiningAlgorithm> miningAlgosList in devDict.Values) {
                    // if list not empty calculate avarage
                    if (miningAlgosList.Count > 0) {
                        // calculate avarage
                        double sum = 0;
                        foreach (var algo in miningAlgosList) {
                            sum += algo.AvaragedSpeed;
                        }
                        double avarageSpeed = sum / miningAlgosList.Count;
                        // set avarage
                        foreach (var algo in miningAlgosList) {
                            algo.AvaragedSpeed = avarageSpeed;
                        }
                    }
                }
            }
        }

    }
}
