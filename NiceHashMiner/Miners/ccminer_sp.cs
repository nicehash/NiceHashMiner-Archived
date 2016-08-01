using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace NiceHashMiner
{
    class ccminer_sp : ccminer
    {
        public ccminer_sp() :
            base()
        {
            MinerDeviceName = "NVIDIA5.x";
            Path = "bin\\ccminer_sp.exe";
            APIPort = 4048;

            if (!Config.ConfigData.DisableDetectionNVidia5X)
                QueryCDevs();
        }


        protected override void AddPotentialCDev(string text)
        {
            if (!text.Contains("GPU")) return;

            Helpers.ConsolePrint(MinerDeviceName, "Detected: " + text);

            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            // add only SM 5.2 or SM 5.0 devices (or 6.x)
            if (name.Contains("SM 5.") || name.Contains("SM 6."))
            {
                name = name.Substring(8);
                CDevs.Add(new ComputeDevice(id, MinerDeviceName, name));
                Helpers.ConsolePrint(MinerDeviceName, "Added: " + name);
            }
        }


        protected override string BenchmarkGetConsoleOutputLine(Process BenchmarkHandle)
        {
            if (AlgoNameIs("lyra2re") || AlgoNameIs("lyra2rev2") || AlgoNameIs("decred"))
                return BenchmarkHandle.StandardOutput.ReadLine();

            return BenchmarkHandle.StandardError.ReadLine();
        }
    }
}
