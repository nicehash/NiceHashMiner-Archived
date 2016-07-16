using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs
{
    /// <summary>
    /// DeviceDetectionConfig is used to enable/disable detection of certain GPU type devices 
    /// </summary>
    public class DeviceDetectionConfig
    {
        // TODO no need to initialize to false but just mirroring legacy for now
        public bool DisableDetectionNVidia5X = false;
        public bool DisableDetectionNVidia3X = false;
        public bool DisableDetectionNVidia2X = false;
        public bool DisableDetectionAMD = false;
    }
}
