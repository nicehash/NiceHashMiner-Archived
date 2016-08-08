using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners
{
    class ccminer_sm5x : ccminer
    {
        public ccminer_sm5x(bool queryComputeDevices) :
            base(queryComputeDevices)
        {
            MinerDeviceName = "NVIDIA5.x";
            Path = MinerPaths.ccminer_sp;

            TryQueryCDevs();
        }

        protected override MinerType GetMinerType() {
            return MinerType.ccminer_sm5x;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_5_x);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        protected override bool IsGroupQueryEnabled() {
            return !ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia5X;
        }

        protected override string GetOptimizedMinerPath(AlgorithmType algorithmType) {
            if (AlgorithmType.Decred == algorithmType) {
                return MinerPaths.ccminer_decred;
            }
            if (AlgorithmType.NeoScrypt == algorithmType) {
                return MinerPaths.ccminer_neoscrypt;
            }
            if (AlgorithmType.Lyra2RE == algorithmType || AlgorithmType.Lyra2REv2 == algorithmType) {
                return MinerPaths.ccminer_nanashi;
            }

            return MinerPaths.ccminer_sp;
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 5.2 or SM 5.0 devices (or 6.x)
            return name.Contains("SM 5.") || name.Contains("SM 6.");
        }


        protected override string BenchmarkGetConsoleOutputLine(Process BenchmarkHandle)
        {
            if (IsCurrentAlgo(AlgorithmType.Lyra2REv2) || IsCurrentAlgo(AlgorithmType.Decred))
                return BenchmarkHandle.StandardOutput.ReadLine();

            return BenchmarkHandle.StandardError.ReadLine();
        }
    }
}
