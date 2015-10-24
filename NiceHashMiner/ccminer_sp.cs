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

            QueryCDevs();
        }


        protected override void AddPotentialCDev(string text)
        {
            if (!text.Contains("GPU")) return;

            Helpers.ConsolePrint(MinerDeviceName + " detected: " + text);

            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            // add only SM 5.2 or SM 5.0 devices
            if (name.Contains("SM 5."))
            {
                name = name.Substring(8);
                CDevs.Add(new ComputeDevice(id, MinerDeviceName, name));
                Helpers.ConsolePrint(MinerDeviceName + " added: " + name);
            }
        }


        protected override string BenchmarkGetConsoleOutputLine(Process BenchmarkHandle)
        {
            return BenchmarkHandle.StandardError.ReadLine();
        }
    }
}
