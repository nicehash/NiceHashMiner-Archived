using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Interfaces {
    public interface IBenchmarkComunicator {

        void SetCurrentStatus(string status);
        void OnBenchmarkComplete(bool success, string status);
    }
}
