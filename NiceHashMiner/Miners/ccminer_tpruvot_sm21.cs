using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

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
            //tmp[GetAlgoIndex("whirlpoolx")].MinerName = "whirlpool";    // Needed for new tpruvot's ccminer
            // name change => "whirlpoolx" => "whirlpool"
            tmp[GetAlgoIndex("whirlpoolx")] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpoolx", "whirlpool");     // Needed for new tpruvot's ccminer
            tmp.RemoveAt(GetAlgoIndex("neoscrypt"));    // Remove NeoScrypt
            tmp.RemoveAt(GetAlgoIndex("daggerhashimoto") - 1);   // Remove Daggerhashimoto
            SupportedAlgorithms = tmp.ToArray();

            if (!Config.ConfigData.DisableDetectionNVidia2X)
                QueryCDevs();
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 2.1
            return name.Contains("SM 2.1");
        }
    }
}
