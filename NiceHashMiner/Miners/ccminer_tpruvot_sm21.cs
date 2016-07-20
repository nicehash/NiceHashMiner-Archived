using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;

namespace NiceHashMiner
{
    class ccminer_tpruvot_sm21 : ccminer
    {
        public ccminer_tpruvot_sm21() : base()
        {
            MinerDeviceName = "NVIDIA2.1";
            Path = "bin\\ccminer_tpruvot.exe";
            APIPort = 4021;

            // disable neoscrypt & whirlpoolx & daggerhashimoto
            var tmp = new List<Algorithm>(SupportedAlgorithms);
            tmp[GetAlgoIndex("whirlpoolx")].MinerName = "whirlpool";    // Needed for new tpruvot's ccminer
            tmp.RemoveAt(GetAlgoIndex("neoscrypt"));    // Remove NeoScrypt
            tmp.RemoveAt(GetAlgoIndex("daggerhashimoto") - 1);   // Remove Daggerhashimoto
            SupportedAlgorithms = tmp.ToArray();

            if (!Config.ConfigData.DisableDetectionNVidia2X)
                QueryCDevs();
        }


        protected override void AddPotentialCDev(string text)
        {
            if (!text.Contains("GPU")) return;

            Helpers.ConsolePrint(MinerDeviceName, "Detected: " + text);

            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            // add only SM 2.1
            if (name.Contains("SM 2.1"))
            {
                name = name.Substring(8);
                CDevs.Add(new ComputeDevice(id, MinerDeviceName, name, this, true));
                Helpers.ConsolePrint(MinerDeviceName, "Added: " + name);
            }
        }
    }
}
