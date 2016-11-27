using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs.Data
{ 
    /// <summary>
    /// DeviceDetectionConfig is used to enable/disable detection of certain GPU type devices 
    /// </summary>
    /// 
    [Serializable]
    public class DeviceDetectionConfig {
        public bool DisableDetectionAMD { get; set; }
        public bool DisableDetectionNVIDIA { get; set; }

        public DeviceDetectionConfig()
        {
            DisableDetectionAMD = false;
            DisableDetectionNVIDIA = false;
        }
    }
}
