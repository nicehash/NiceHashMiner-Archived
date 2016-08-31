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
        //public string PasswordDefault { get; set; } // TODO set to intensity
        public bool Skip { get; set; }

        public static readonly string PasswordDefault = "x"; // TODO rename default

        [JsonIgnore]
        public double CurrentProfit { get; set; }


        public Algorithm(AlgorithmType niceHashID, string minerName)
        {
            NiceHashID = niceHashID;
            NiceHashName = AlgorithmNiceHashNames.GetName(niceHashID);
            MinerName = minerName;

            BenchmarkSpeed = 0.0d;
            ExtraLaunchParameters = "";
            //PasswordDefault = ""; // TODO set intenstity
            Skip = false;
        }
    }
}
