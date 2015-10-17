using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NiceHashMiner
{
    class CPUID
    {
        [DllImport("cpuid.dll")]
        public static extern string GetCPUName();

        [DllImport("cpuid.dll")]
        public static extern string GetCPUVendor();

        [DllImport("cpuid.dll")]
        public static extern int SupportsSSE2();

        [DllImport("cpuid.dll")]
        public static extern int SupportsAVX();

        [DllImport("cpuid.dll")]
        public static extern int SupportsAVX2();
    }
}
