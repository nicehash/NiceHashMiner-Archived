using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Interfaces {
    public interface IBenchmarkComunicator {

        void SetCurrentStatus(string status);
        void OnBenchmarkComplete(bool success, string status);
    }
}
