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
            
            // name change => "whirlpoolx" => "whirlpool"
            SupportedAlgorithms[AlgorithmType.WhirlpoolX] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpoolx", "whirlpool");     // Needed for new tpruvot's ccminer
            // disable/remove neoscrypt
            SupportedAlgorithms.Remove(AlgorithmType.NeoScrypt);

            if (!Config.ConfigData.DisableDetectionNVidia3X)
                QueryCDevs();
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 3.x
            return name.Contains("SM 3.");
        }
    }
}
