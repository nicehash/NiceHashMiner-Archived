using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Parsing {
    public class MinerOptionPackage {
        public string Name;
        public MinerType Type;
        public List<MinerOption> GeneralOptions;
        public List<MinerOption> TemperatureOptions;

        public MinerOptionPackage(MinerType iType, List<MinerOption> iGeneralOptions, List<MinerOption> iTemperatureOptions) {
            this.Type = iType;
            this.GeneralOptions = iGeneralOptions;
            this.TemperatureOptions = iTemperatureOptions;
            this.Name = Enum.GetName(typeof(MinerType), iType);
        }
    }
}
