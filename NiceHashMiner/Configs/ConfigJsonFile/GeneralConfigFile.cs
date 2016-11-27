using NiceHashMiner.Configs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Configs.ConfigJsonFile {
    public class GeneralConfigFile : ConfigFile<GeneralConfig> {
        public GeneralConfigFile()
            : base("General.json", "General_old.json") {
        }
    }
}
