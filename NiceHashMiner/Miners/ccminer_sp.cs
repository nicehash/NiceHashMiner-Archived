using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Miners;

namespace NiceHashMiner
{
    class ccminer_sp : ccminer
    {
        public ccminer_sp() :
            base()
        {
            MinerDeviceName = "NVIDIA5.x";
            Path = MinerPaths.ccminer_sp;
            APIPort = 4048;

            if (!Config.ConfigData.DisableDetectionNVidia5X)
                QueryCDevs();
        }


        protected override bool IsPotentialDevSM(string name) {
            // add only SM 5.2 or SM 5.0 devices (or 6.x)
            return name.Contains("SM 5.") || name.Contains("SM 6.");
        }


        protected override string BenchmarkGetConsoleOutputLine(Process BenchmarkHandle)
        {
            if (AlgoNameIs("lyra2rev2") || AlgoNameIs("decred"))
                return BenchmarkHandle.StandardOutput.ReadLine();

            return BenchmarkHandle.StandardError.ReadLine();
        }
    }
}
