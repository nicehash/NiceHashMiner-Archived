using Newtonsoft.Json;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    public class Algorithm : ICloneable
    {
        [JsonIgnore]
        readonly public AlgorithmType NiceHashID;
        readonly public string NiceHashName;
        // Miner name is used for miner ALGO flag parameter
        readonly public string MinerName;
        public double BenchmarkSpeed { get; set; }
        [JsonIgnore]
        public double CurrentProfit { get; set; }
        public string ExtraLaunchParameters { get; set; }
        public string UsePassword { get; set; }
        public bool Skip { get; set; }
        // TODO remove this member
        public bool[] DisabledDevice;


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

        /// <summary>
        /// This constructor is only to be used from the json library when initializing from configs.
        /// </summary>
        /// <param name="niceHashName"></param>
        /// <param name="minerName"></param>
        public Algorithm(string niceHashName, string minerName) {
            NiceHashID = AlgorithmNiceHashNames.GetKey(niceHashName);
            NiceHashName = niceHashName;
            MinerName = minerName;
            // these defaults are kinda useless
            BenchmarkSpeed = 0.0d;
            ExtraLaunchParameters = "";
            UsePassword = null;
            Skip = false;
        }

        public object Clone() {
            // mostly value types and readonly references so we should be safe
            return this.MemberwiseClone();
        }
    }
}
