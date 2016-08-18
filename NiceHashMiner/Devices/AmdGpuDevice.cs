using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Devices {
    public class AmdGpuDevice {
        public string AmdName;
        private OpenCLDevice _openClSubset;
        public AmdGpuDevice(OpenCLDevice openClSubset) {
            _openClSubset = openClSubset;
        }
    }
}
