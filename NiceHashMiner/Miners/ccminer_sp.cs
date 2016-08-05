using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Miners;
using NiceHashMiner.Enums;

namespace NiceHashMiner
{
    class ccminer_sp : ccminer
    {
        public ccminer_sp(bool queryComputeDevices) :
            base(queryComputeDevices)
        {
            MinerDeviceName = "NVIDIA5.x";
            Path = MinerPaths.ccminer_sp;
            APIPort = 4048;

            SupportedAlgorithms = GroupAlgorithms.CreateDefaultsForGroup(DeviceGroupType.NVIDIA_5_x);

            TryQueryCDevs();
        }

        protected override bool IsGroupQueryEnabled() {
            return !Config.ConfigData.DisableDetectionNVidia5X;
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
