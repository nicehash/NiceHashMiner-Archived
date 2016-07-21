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
        readonly public string MinerName;
        public double BenchmarkSpeed;
        public double CurrentProfit;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public bool Skip;
        public bool[] DisabledDevice;

        public Algorithm(AlgorithmType niceHashID, string niceHashName, string minerName, string extraLaunchParameters = "")
        {
            NiceHashID = niceHashID;
            NiceHashName = niceHashName;
            MinerName = minerName;
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = extraLaunchParameters;
            UsePassword = null;
            Skip = false;
        }
    }
}
