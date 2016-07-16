using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs
{
    /// <summary>
    /// ComputeDevicesSettup serves to save and compare ComputeDevice settings changes
    /// which is useful when saving and comparing benchmark settings.
    /// The changes can be of two scenarios:
    /// #1 Detect if the device is enabled/disabled,
    /// #2 Detect hardware changes/upgrades such as CPUs and GPUs.
    /// </summary>
    public class ComputeDevicesSettup
    {
        readonly public long SettupID;
        readonly public ComputeDevice[] DevicesSettup;

        public ComputeDevicesSettup(long settupID, List<ComputeDevice> devicesSettup)
        {
            SettupID = settupID;
            DevicesSettup = devicesSettup.ToArray();
        }
    }
}
