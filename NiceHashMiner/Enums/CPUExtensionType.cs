using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums
{
    public enum CPUExtensionType : int {
        // 0 - automatic, 1 - AVX2, 2 - AVX, 3 - AES, 4 - SSE2
        Automatic = 0,
        AVX2 = 1,
        AVX = 2,
        AES = 3,
        SSE2 = 4
    }
}
