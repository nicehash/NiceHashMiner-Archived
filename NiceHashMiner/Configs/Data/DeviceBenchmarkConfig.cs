using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Configs.Data {
    [Serializable]
    public class DeviceBenchmarkConfig {
        public string DeviceUUID = "";
        public string DeviceName = "";
        //public int TimeLimit { get; set; }
        public Dictionary<AlgorithmType, Algorithm> AlgorithmSettings = new Dictionary<AlgorithmType, Algorithm>();
    }
}
