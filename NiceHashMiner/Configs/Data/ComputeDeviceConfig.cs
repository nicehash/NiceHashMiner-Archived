using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Configs.Data {
    [Serializable]
    public class ComputeDeviceConfig {
        public string Name = "";
        public bool Enabled = true;
        public string UUID = "";
    }
}
