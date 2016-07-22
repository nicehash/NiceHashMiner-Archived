using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

namespace NiceHashMiner
{
    class ccminer_tpruvot_sm21 : ccminer
    {
        public ccminer_tpruvot_sm21() : base()
        {
            MinerDeviceName = "NVIDIA2.1";
            Path = MinerPaths.ccminer_tpruvot;
            APIPort = 4021;

            // minerName change => "whirlpoolx" => "whirlpool"
            SupportedAlgorithms[AlgorithmType.WhirlpoolX] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpool");     // Needed for new tpruvot's ccminer
            // disable/remove neoscrypt, daggerhashimoto
            SupportedAlgorithms.Remove(AlgorithmType.NeoScrypt);
            SupportedAlgorithms.Remove(AlgorithmType.DaggerHashimoto);

            if (!Config.ConfigData.DisableDetectionNVidia2X)
                QueryCDevs();
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 2.1
            return name.Contains("SM 2.1");
        }
    }
}
