using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Devices;

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

        /// <summary>
        /// IsSameDeviceSettup, checks for hardware/device changes.
        /// We check [Name, Vendor, ID, for now order sensitive]
        /// Make sure to call this function after all ComputeDevices are registered/found
        /// </summary>
        /// <returns>True if hardware settup is the same, False otherwise</returns>
        public bool IsSameDeviceSettup() {
            bool isSame = DevicesSettup.Length == ComputeDevice.AllAvaliableDevices.Count;
            if (isSame) {
                for (int i = 0; i < DevicesSettup.Length; ++i) {
                    var first = DevicesSettup[i];
                    var second = ComputeDevice.AllAvaliableDevices[i];
                    isSame = 
                           first.ID     == second.ID
                        && first.Name   == second.Name
                        && first.Vendor == second.Vendor;
                    if (isSame == false) {
                        // we have a change stop
                        break;
                    }
                }
            }

            return isSame;
        }
    }
}
