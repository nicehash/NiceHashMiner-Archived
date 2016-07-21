using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

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

            // disable neoscrypt
            var tmp = new List<Algorithm>(SupportedAlgorithms);
            //tmp[GetAlgoIndex("whirlpoolx")].MinerName = "whirlpool";    // Needed for new tpruvot's ccminer
            // name change => "whirlpoolx" => "whirlpool"
            tmp[GetAlgoIndex("whirlpoolx")] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpoolx", "whirlpool");     // Needed for new tpruvot's ccminer
            tmp.RemoveAt(GetAlgoIndex("neoscrypt"));   // Remove Neoscrypt
            SupportedAlgorithms = tmp.ToArray();

            if (!Config.ConfigData.DisableDetectionNVidia3X)
                QueryCDevs();
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 3.x
            return name.Contains("SM 3.");
        }
    }
}
