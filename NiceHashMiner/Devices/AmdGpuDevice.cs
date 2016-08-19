using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Devices {
    [Serializable]
    public class AmdGpuDevice {
        public int DeviceID { get { return (int)_openClSubset.DeviceID; } }
        public string DeviceName; // init this with the ADL
        public string UUID; // init this with the ADL, use PCI_VEN & DEV IDs
        public bool UseOptimizedVersion { get; private set; }
        private OpenCLDevice _openClSubset;
        public AmdGpuDevice(OpenCLDevice openClSubset) {
            _openClSubset = openClSubset;
            // Check for optimized version
            // first if not optimized
            Helpers.ConsolePrint("AmdGpuDevice", "List: " + _openClSubset._CL_DEVICE_NAME);
            if (!( _openClSubset._CL_DEVICE_NAME.Contains("Bonaire")
                || _openClSubset._CL_DEVICE_NAME.Contains("Fiji")
                || _openClSubset._CL_DEVICE_NAME.Contains("Hawaii")
                || _openClSubset._CL_DEVICE_NAME.Contains("Pitcairn")
                || _openClSubset._CL_DEVICE_NAME.Contains("Tahiti")
                || _openClSubset._CL_DEVICE_NAME.Contains("Tonga"))) {
                UseOptimizedVersion = false;
                Helpers.ConsolePrint("AmdGpuDevice", "GPU (" + _openClSubset._CL_DEVICE_NAME + ") is optimized => NOO!");
            } else {
                UseOptimizedVersion = true;
                Helpers.ConsolePrint("AmdGpuDevice", "GPU (" + _openClSubset._CL_DEVICE_NAME + ") is optimized => YES!");
            }
            // TODO set algorithm optimization settings
        }

        private bool _isEtherumCapable = false;
        private bool _isEtherumCapableInit = false;
        public bool IsEtherumCapable() {
            if (!_isEtherumCapableInit) {
                _isEtherumCapableInit = true;
                // check if 2GB device memory
                _isEtherumCapable = _openClSubset._CL_DEVICE_GLOBAL_MEM_SIZE >= ComputeDevice.MEMORY_2GB;

                // exception devices
                if (DeviceName.Contains("750") && DeviceName.Contains("Ti")) {
                    Helpers.ConsolePrint("CudaDevice", "GTX 750Ti found! By default this device will be disabled for ethereum as it is generally too slow to mine on it.");
                    _isEtherumCapable = false;
                }
            }
            return _isEtherumCapable;
        }
    }
}
