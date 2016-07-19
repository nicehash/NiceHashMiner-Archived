using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums
{
    public enum DeviceGroupType : int
    {
        CPU = 0,
        AMD_OpenCL,
        NVIDIA_2_1,
        NVIDIA_3_x,
        NVIDIA_5_x, // includes 6.x ATM
        NVIDIA_6_x, // TODO draft not in use
    }
}
