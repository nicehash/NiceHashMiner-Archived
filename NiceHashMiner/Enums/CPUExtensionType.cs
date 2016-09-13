using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums
{
    public enum CPUExtensionType : int {
        // 0 - automatic, 1 - SSE2, 2 - AVX, 3 - AVX2, 4 - AES
        Automatic = 0,
        SSE2 = 1,
        AVX = 2,
        AVX2 = 3,
        AES = 4
    }
}
