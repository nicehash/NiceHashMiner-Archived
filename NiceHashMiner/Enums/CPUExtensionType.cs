using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums
{
    public enum CPUExtensionType : int {
        Automatic = 0,
        AVX2_AES = 1,
        AVX2 = 2,
        AVX_AES = 3,
        AVX = 4,
        AES = 5,
        SSE2 = 6,
    }
}
