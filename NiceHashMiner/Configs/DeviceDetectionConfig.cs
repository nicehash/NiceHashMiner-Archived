using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs
{
    /// <summary>
    /// DeviceDetectionConfig is used to enable/disable detection of certain GPU type devices 
    /// </summary>
    /// 
    [Serializable]
    public class DeviceDetectionConfig
    {
        public bool DisableDetectionNVidia6X { get; set; }
        public bool DisableDetectionNVidia5X { get; set; }
        public bool DisableDetectionNVidia3X { get; set; }
        public bool DisableDetectionNVidia2X { get; set; }
        public bool DisableDetectionAMD { get; set; }

        public DeviceDetectionConfig()
        {
            DisableDetectionNVidia6X = false;
            DisableDetectionNVidia5X = false;
            DisableDetectionNVidia3X = false;
            DisableDetectionNVidia2X = false;
            DisableDetectionAMD = false;
        }
    }
}
