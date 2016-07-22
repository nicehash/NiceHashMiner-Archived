using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    public class Algorithm
    {
        readonly public AlgorithmType NiceHashID;
        readonly public string NiceHashName;
        // Miner name is used for miner ALGO flag parameter
        readonly public string MinerName;
        public double BenchmarkSpeed { get; set; }
        public double CurrentProfit { get; set; }
        public string ExtraLaunchParameters { get; set; }
        public string UsePassword { get; set; }
        public bool Skip { get; set; }
        public bool[] DisabledDevice;

        public Algorithm(AlgorithmType niceHashID, string minerName)
        {
            NiceHashID = niceHashID;
            NiceHashName = AlgorithmNiceHashNames.GetName(niceHashID);
            MinerName = minerName;
            // these defaults are kinda useless
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = "";
            UsePassword = null;
            Skip = false;
        }
    }
}
