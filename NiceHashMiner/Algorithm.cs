using Newtonsoft.Json;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    [Serializable]
    public class Algorithm
    {
        readonly public AlgorithmType NiceHashID;
        // ignore the dictionary atribute shows the name
        [JsonIgnore]
        readonly public string NiceHashName;
        // Miner name is used for miner ALGO flag parameter
        readonly public string MinerName;
        public double BenchmarkSpeed { get; set; }
        public string ExtraLaunchParameters { get; set; }
        public string UsePassword { get; set; }
        public bool Skip { get; set; }

        [JsonIgnore]
        public double CurrentProfit { get; set; }


        public Algorithm(AlgorithmType niceHashID, string minerName)
        {
            NiceHashID = niceHashID;
            NiceHashName = AlgorithmNiceHashNames.GetName(niceHashID);
            MinerName = minerName;
            // these defaults are kinda useless
            BenchmarkSpeed = 0.0d;
            ExtraLaunchParameters = "";
            UsePassword = null;
            Skip = false;
        }
    }
}
