using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    class ccminer_tpruvot : ccminer
    {
        public ccminer_tpruvot() :
            base()
        {
            MinerDeviceName = "NVIDIA3.x";
            Path = "bin\\ccminer_tpruvot.exe";
            APIPort = 4049;

            // disable ethereum
            var tmp = new List<Algorithm>(SupportedAlgorithms);
            tmp.RemoveAt(GetAlgoIndex("ethereum"));   // Remove Ethereum
            SupportedAlgorithms = tmp.ToArray();

            if (!Config.ConfigData.DisableDetectionNVidia3X)
                QueryCDevs();
        }


        protected override void AddPotentialCDev(string text)
        {
            if (!text.Contains("GPU")) return;

            Helpers.ConsolePrint(MinerDeviceName, "Detected: " + text);

            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            // add only SM 3.x
            if (name.Contains("SM 3."))
            {
                name = name.Substring(8);
                CDevs.Add(new ComputeDevice(id, MinerDeviceName, name));
                Helpers.ConsolePrint(MinerDeviceName, "Added: " + name);
            }
        }
    }
}
