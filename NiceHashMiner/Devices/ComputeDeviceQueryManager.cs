using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Interfaces;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;
using System.Diagnostics;
using Newtonsoft.Json;
using ATI.ADL;
using System.Runtime.InteropServices;
using System.Management;
using System.IO;
using System.Globalization;
using NiceHashMiner.Utils;

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceQueryManager class is used to query ComputeDevices avaliable on the system.
    /// Query CPUs, GPUs [Nvidia, AMD]
    /// </summary>
    public class ComputeDeviceQueryManager : BaseLazySingleton<ComputeDeviceQueryManager>
    {

        const int AMD_VENDOR_ID = 1002;
        readonly string TAG;

        const double NVIDIA_RECOMENDED_DRIVER = 372.54;
        const double NVIDIA_MIN_DETECTION_DRIVER = 362.61;
        double _currentNvidiaOpenCLDriver = -1;
        JsonSerializerSettings _jsonSettings = null;

        // for downloading
        public bool HasNVIDIA { get; private set; }
        public bool HasAMD { get; private set; }
            
        protected ComputeDeviceQueryManager() {
            TAG = this.GetType().Name;
            HasNVIDIA = false;
            HasAMD = false;
            _jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Culture = CultureInfo.InvariantCulture
            };
        }

        public int CPUs { get; private set; }

        public int AMDOpenCLPlatformNum { get; private set; }
        public string AMDOpenCLPlatformStringKey { get; private set; }

        public IMessageNotifier MessageNotifier { get; private set; }

        public void QueryDevices(IMessageNotifier messageNotifier)
        {
            MessageNotifier = messageNotifier;
            // #0 get video controllers, used for cross checking
            QueryVideoControllers();
            // Order important CPU Query must be first
            // #1 CPU
            QueryCPUs();
            // #2 CUDA
            showMessageAndStep(International.GetText("Compute_Device_Query_Manager_CUDA_Query"));
            QueryCudaDevices();
            // #3 OpenCL
            showMessageAndStep(International.GetText("Compute_Device_Query_Manager_OpenCL_Query"));
            QueryOpenCLDevices();
            // #4 AMD query AMD from OpenCL devices, get serial and add devices
            QueryAMD();
            // #5 uncheck CPU if GPUs present, call it after we Query all devices
            UncheckedCPU();
            // add numberings to same devices
            if (ComputeDevice.AllAvaliableDevices.Count != ComputeDevice.UniqueAvaliableDevices.Count) {
                // name count
                Dictionary<string, int> namesCount = new Dictionary<string, int>();
                // init keys and counters
                foreach (var uniqueCdev in ComputeDevice.UniqueAvaliableDevices) {
                    namesCount.Add(uniqueCdev.Name, 0);
                }
                // count 
                foreach (var cDev in ComputeDevice.AllAvaliableDevices) {
                    namesCount[cDev.Name]++;
                }
                foreach (var nameCount in namesCount) {
                    string name = nameCount.Key;
                    int deviceCount = nameCount.Value;
                    if (deviceCount > 1) {
                        int numID = 1;
                        foreach (var cDev in ComputeDevice.AllAvaliableDevices) {
                            if (cDev.Name == name) {
                                cDev.Name = cDev.Name + " #" + numID.ToString();
                                ++numID;
                            }
                        }
                    }
                }
            }


            // TODO update this to report undetected hardware
            // #6 check NVIDIA, AMD devices count
            {
                int NVIDIA_count = 0;
                int AMD_count = 0;
                foreach (var vidCtrl in AvaliableVideoControllers) {
                    NVIDIA_count += (vidCtrl.Name.ToLower().Contains("nvidia")) ? 1 : 0;
                    AMD_count += (vidCtrl.Name.ToLower().Contains("amd")) ? 1 : 0;
                }
                if (NVIDIA_count == CudaDevices.Count) {
                    Helpers.ConsolePrint(TAG, "Cuda NVIDIA/CUDA device count GOOD");
                } else {
                    Helpers.ConsolePrint(TAG, "Cuda NVIDIA/CUDA device count BAD!!!");
                }
                if (AMD_count == amdGpus.Count) {
                    Helpers.ConsolePrint(TAG, "AMD GPU device count GOOD");
                } else {
                    Helpers.ConsolePrint(TAG, "AMD GPU device count BAD!!!");
                }
            }
            // #7 init ethminer ID mappings offset
            if (OpenCLJSONData != null) {
                // helper vars
                Dictionary<ComputePlatformType, int> openCLGpuCount = new Dictionary<ComputePlatformType,int>();
                Dictionary<ComputePlatformType, int> openCLPlatformIds = new Dictionary<ComputePlatformType,int>();
                foreach (var oclPlatform in OpenCLJSONData.OCLPlatforms) {
                    ComputePlatformType current = GetPlatformType(oclPlatform.Key);
                    if(current != ComputePlatformType.NONE) {
                        openCLPlatformIds[current] = oclPlatform.Value;
                    } else {
                        Helpers.ConsolePrint(TAG, "ethminer platforms mapping NONE");
                    }
                }
                foreach (var oclDevs in OpenCLJSONData.OCLPlatformDevices) {
                    ComputePlatformType current = GetPlatformType(oclDevs.Key);
                    if (current != ComputePlatformType.NONE) {
                        foreach (var oclDev in oclDevs.Value) {
                            if (oclDev._CL_DEVICE_TYPE.Contains("GPU")) {
                                if (openCLGpuCount.ContainsKey(current)) {
                                    openCLGpuCount[current]++;
                                } else {
                                    openCLGpuCount[current] = 1;
                                }
                            }
                        }
                    } else {
                        Helpers.ConsolePrint(TAG, "ethminer platforms mapping NONE");
                    }
                }
                // sort platforms by platform values
                Dictionary<int, ComputePlatformType> openCLPlatformIdsRev = new Dictionary<int,ComputePlatformType>();
                List<int> platformIds = new List<int>();
                foreach (var platId in openCLPlatformIds) {
                    openCLPlatformIdsRev[platId.Value] = platId.Key;
                    platformIds.Add(platId.Value);
                }
                platformIds.Sort();
                // set mappings
                int cumulativeCount = 0;
                foreach (var curId in platformIds) {
                    var key = openCLPlatformIdsRev[curId];
                    if (openCLGpuCount.ContainsKey(key)) {
                        _ethminerIdsOffet[key] = cumulativeCount;
                        cumulativeCount += openCLGpuCount[key]; 
                    }
                }
            }
            // allerts
            _currentNvidiaOpenCLDriver = GetNvidiaOpenCLDriver();
            // if we have nvidia cards but no CUDA devices tell the user to upgrade driver
            bool isNvidiaErrorShown = false; // to prevent showing twice
            if (ConfigManager.Instance.GeneralConfig.ShowDriverVersionWarning && HasNvidiaVideoController() && CudaDevices.Count == 0) {
                isNvidiaErrorShown = true;
                var minDriver = NVIDIA_MIN_DETECTION_DRIVER.ToString();
                var recomendDrvier = NVIDIA_RECOMENDED_DRIVER.ToString();
                MessageBox.Show(String.Format(International.GetText("Compute_Device_Query_Manager_NVIDIA_Driver_Detection"),
                    minDriver, recomendDrvier),
                                                      International.GetText("Compute_Device_Query_Manager_NVIDIA_RecomendedDriver_Title"),
                                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // recomended driver
            if (ConfigManager.Instance.GeneralConfig.ShowDriverVersionWarning && HasNvidiaVideoController() && _currentNvidiaOpenCLDriver < NVIDIA_RECOMENDED_DRIVER && !isNvidiaErrorShown && _currentNvidiaOpenCLDriver > -1) {
                var recomendDrvier = NVIDIA_RECOMENDED_DRIVER.ToString();
                var nvdriverString = _currentNvidiaOpenCLDriver > -1 ? String.Format(International.GetText("Compute_Device_Query_Manager_NVIDIA_Driver_Recomended_PART"), _currentNvidiaOpenCLDriver.ToString())
                : "";
                MessageBox.Show(String.Format(International.GetText("Compute_Device_Query_Manager_NVIDIA_Driver_Recomended"),
                    recomendDrvier, nvdriverString, recomendDrvier),
                                                      International.GetText("Compute_Device_Query_Manager_NVIDIA_RecomendedDriver_Title"),
                                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // #x remove reference
            MessageNotifier = null;
        }

        private Dictionary<ComputePlatformType, int> _ethminerIdsOffet = new Dictionary<ComputePlatformType,int>();
        public int GetEthminerOpenCLID(ComputePlatformType platformType, int id) {
            return _ethminerIdsOffet[platformType] + id;
        }

        private ComputePlatformType GetPlatformType(string name) {
            if (name.Contains("Intel")) {
                return ComputePlatformType.Intel;
            }
            if (name.Contains("AMD")) {
                return ComputePlatformType.AMD;
            }
            if (name.Contains("NVIDIA")) {
                return ComputePlatformType.NVIDIA;
            }
            return ComputePlatformType.NONE;
        }

        private void showMessageAndStep(string infoMsg) {
            if (MessageNotifier != null) MessageNotifier.SetMessageAndIncrementStep(infoMsg);
        }

        #region Video controllers, driver versions

        private class VideoControllerData {
            public string Name { get; set; }
            public string Description { get; set; }
            public string PNPDeviceID { get; set; }
            public string DriverVersion { get; set; }
            public string Status { get; set; }
        }

        private List<VideoControllerData> AvaliableVideoControllers = new List<VideoControllerData>();

        private void QueryVideoControllers() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("QueryVideoControllers: ");
            ManagementObjectCollection moc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get();
            bool allVideoContollersOK = true;
            foreach (var manObj in moc) {
                var vidController = new VideoControllerData() {
                    Name = manObj["Name"] as string,
                    Description = manObj["Description"] as string,
                    PNPDeviceID = manObj["PNPDeviceID"] as string,
                    DriverVersion = manObj["DriverVersion"] as string,
                    Status = manObj["Status"] as string
                };
                stringBuilder.AppendLine("\tWin32_VideoController detected:");
                stringBuilder.AppendLine(String.Format("\t\tName {0}", vidController.Name));
                stringBuilder.AppendLine(String.Format("\t\tDescription {0}", vidController.Description));
                stringBuilder.AppendLine(String.Format("\t\tPNPDeviceID {0}", vidController.PNPDeviceID));
                stringBuilder.AppendLine(String.Format("\t\tDriverVersion {0}", vidController.DriverVersion));
                stringBuilder.AppendLine(String.Format("\t\tStatus {0}", vidController.Status));

                // check if controller ok
                if (allVideoContollersOK && !vidController.Status.ToLower().Equals("ok")) {
                    allVideoContollersOK = false;
                }

                AvaliableVideoControllers.Add(vidController);
            }
            Helpers.ConsolePrint(TAG, stringBuilder.ToString());
            if (ConfigManager.Instance.GeneralConfig.ShowDriverVersionWarning && !allVideoContollersOK) {
                string msg = International.GetText("QueryVideoControllers_NOT_ALL_OK_Msg");
                foreach (var vc in AvaliableVideoControllers) {
                    if(!vc.Status.ToLower().Equals("ok")) {
                        msg += Environment.NewLine
                            + String.Format(International.GetText("QueryVideoControllers_NOT_ALL_OK_Msg_Append"), vc.Name, vc.Status, vc.PNPDeviceID);
                    }
                }
                MessageBox.Show(msg,
                                International.GetText("QueryVideoControllers_NOT_ALL_OK_Title"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool HasNvidiaVideoController() {
            foreach (var vctrl in AvaliableVideoControllers) {
                if (vctrl.Name.ToLower().Contains("nvidia")) return true;
            }
            return false;
        }

        #endregion // Video controllers, driver versions

        private void QueryCPUs() {
            Helpers.ConsolePrint(TAG, "QueryCPUs START");
            // get all CPUs
            CPUs = CPUID.GetPhysicalProcessorCount();

            // get all cores (including virtual - HT can benefit mining)
            int ThreadsPerCPU = CPUID.GetVirtualCoresCount() / CPUs;

            if (!Helpers.InternalCheckIsWow64())
            {
                MessageBox.Show(International.GetText("Form_Main_msgbox_CPUMining64bitMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            if (ThreadsPerCPU * CPUs > 64)
            {
                MessageBox.Show(International.GetText("Form_Main_msgbox_CPUMining64CoresMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            // TODO important move this to settings
            int ThreadsPerCPUMask = ThreadsPerCPU;
            Globals.ThreadsPerCPU = ThreadsPerCPU;
            
            if (CPUs == 1) {
                MinersManager.Instance.AddCpuMiner(new cpuminer(0, ThreadsPerCPU, 0), 0, CPUID.GetCPUName().Trim());
            }
            else {
                for (int i = 0; i < CPUs; i++) {
                    MinersManager.Instance.AddCpuMiner(new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask)),
                        i, CPUID.GetCPUName().Trim());
                }
            }
            Helpers.ConsolePrint(TAG, "QueryCPUs END");
        }

        List<OpenCLDevice> amdGpus = new List<OpenCLDevice>();
        private void QueryAMD() {
            Helpers.ConsolePrint(TAG, "QueryAMD START");
            //showMessageAndStep(International.GetText("Form_Main_loadtext_AMD"));
            //var dump = new sgminer(true);

            if(ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionAMD) {
                Helpers.ConsolePrint(TAG, "Skipping AMD device detection, settings set to disabled");
                showMessageAndStep(International.GetText("Compute_Device_Query_Manager_AMD_Query_Skip"));
                return;
            }

            #region AMD driver check, ADL returns 0
            // check the driver version bool EnableOptimizedVersion = true;
            Dictionary<string, bool> deviceDriverOld = new Dictionary<string, bool>();
            string minerPath = MinerPaths.sgminer_5_5_0_general;
            bool ShowWarningDialog = false;

            foreach (var vidContrllr in AvaliableVideoControllers) {
                Helpers.ConsolePrint(TAG, String.Format("Checking AMD device (driver): {0} ({1})", vidContrllr.Name, vidContrllr.DriverVersion));

                deviceDriverOld[vidContrllr.Name] = false;
                // TODO checking radeon drivers only?
                if ((vidContrllr.Name.Contains("AMD") || vidContrllr.Name.Contains("Radeon")) && ShowWarningDialog == false) {
                    Version AMDDriverVersion = new Version(vidContrllr.DriverVersion);

                    if (AMDDriverVersion.Major < 15) {
                        ShowWarningDialog = true;
                        deviceDriverOld[vidContrllr.Name] = true;
                        Helpers.ConsolePrint(TAG, "WARNING!!! Old AMD GPU driver detected! All optimized versions disabled, mining " +
                            "speed will not be optimal. Consider upgrading AMD GPU driver. Recommended AMD GPU driver version is 15.7.1.");
                    } else if (AMDDriverVersion.Major == 16 && AMDDriverVersion.Minor >= 150) {
                        if (MinersDownloadManager.Instance.IsMinerBinFolder()) {
                            // TODO why this copy?
                            string src = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" +
                                         minerPath.Split('\\')[0] + "\\" + minerPath.Split('\\')[1] + "\\kernel";

                            foreach (var file in Directory.GetFiles(src)) {
                                string dest = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\" + System.IO.Path.GetFileName(file);
                                if (!File.Exists(dest)) File.Copy(file, dest, false);
                            }
                        }
                    }
                }
            }
            if (ConfigManager.Instance.GeneralConfig.ShowDriverVersionWarning && ShowWarningDialog == true) {
                Form WarningDialog = new DriverVersionConfirmationDialog();
                WarningDialog.ShowDialog();
                WarningDialog = null;
            }
            #endregion // AMD driver check

            // get platform version
            showMessageAndStep(International.GetText("Compute_Device_Query_Manager_AMD_Query"));
            if (IsOpenCLQuerrySuccess) {
                bool amdPlatformNumFound = false;
                foreach (var kvp in OpenCLJSONData.OCLPlatforms) {
                    if (kvp.Key.Contains("AMD") || kvp.Key.Contains("amd")) {
                        amdPlatformNumFound = true;
                        AMDOpenCLPlatformStringKey = kvp.Key;
                        AMDOpenCLPlatformNum = kvp.Value;
                        Helpers.ConsolePrint(TAG, String.Format("AMD platform found: Key: {0}, Num: {1}",
                            AMDOpenCLPlatformStringKey,
                            AMDOpenCLPlatformNum.ToString()));
                        break;
                    }
                }
                if (amdPlatformNumFound) {
                    // get only AMD gpus
                    {
                        var amdOCLDevices = OpenCLJSONData.OCLPlatformDevices[AMDOpenCLPlatformStringKey];
                        foreach (var oclDev in amdOCLDevices) {
                            if (oclDev._CL_DEVICE_TYPE.Contains("GPU")) {
                                amdGpus.Add(oclDev);
                            } 
                        }
                    }
                    if (amdGpus.Count == 0) {
                        Helpers.ConsolePrint(TAG, "AMD GPUs count is 0");
                    } else {
                        Helpers.ConsolePrint(TAG, "AMD GPUs count : " + amdGpus.Count.ToString());
                        Helpers.ConsolePrint(TAG, "AMD Getting device name and serial from ADL");
                        // ADL
                        bool isAdlInit = true;
                        // ADL does not get devices in order map devices by bus number
                        // bus id, <name, uuid>
                        Dictionary<int, Tuple<string, string>> _busIdsInfo = new Dictionary<int, Tuple<string, string>>();
                        List<string> _amdDeviceName = new List<string>();
                        List<string> _amdDeviceUUID = new List<string>();
                        try {
                            int ADLRet = -1;
                            int NumberOfAdapters = 0;
                            if (null != ADL.ADL_Main_Control_Create)
                                // Second parameter is 1: Get only the present adapters
                                ADLRet = ADL.ADL_Main_Control_Create(ADL.ADL_Main_Memory_Alloc, 1);
                            if (ADL.ADL_SUCCESS == ADLRet) {
                                if (null != ADL.ADL_Adapter_NumberOfAdapters_Get) {
                                    ADL.ADL_Adapter_NumberOfAdapters_Get(ref NumberOfAdapters);
                                }
                                Helpers.ConsolePrint(TAG, "Number Of Adapters: " + NumberOfAdapters.ToString());

                                if (0 < NumberOfAdapters) {
                                    // Get OS adpater info from ADL
                                    ADLAdapterInfoArray OSAdapterInfoData;
                                    OSAdapterInfoData = new ADLAdapterInfoArray();

                                    if (null != ADL.ADL_Adapter_AdapterInfo_Get) {
                                        IntPtr AdapterBuffer = IntPtr.Zero;
                                        int size = Marshal.SizeOf(OSAdapterInfoData);
                                        AdapterBuffer = Marshal.AllocCoTaskMem((int)size);
                                        Marshal.StructureToPtr(OSAdapterInfoData, AdapterBuffer, false);

                                        if (null != ADL.ADL_Adapter_AdapterInfo_Get) {
                                            ADLRet = ADL.ADL_Adapter_AdapterInfo_Get(AdapterBuffer, size);
                                            if (ADL.ADL_SUCCESS == ADLRet) {
                                                OSAdapterInfoData = (ADLAdapterInfoArray)Marshal.PtrToStructure(AdapterBuffer, OSAdapterInfoData.GetType());
                                                int IsActive = 0;

                                                for (int i = 0; i < NumberOfAdapters; i++) {
                                                    // Check if the adapter is active
                                                    if (null != ADL.ADL_Adapter_Active_Get)
                                                        ADLRet = ADL.ADL_Adapter_Active_Get(OSAdapterInfoData.ADLAdapterInfo[i].AdapterIndex, ref IsActive);

                                                    if (ADL.ADL_SUCCESS == ADLRet) {
                                                        // we are looking for amd
                                                        // TODO check discrete and integrated GPU separation
                                                        var vendorID = OSAdapterInfoData.ADLAdapterInfo[i].VendorID;
                                                        var devName = OSAdapterInfoData.ADLAdapterInfo[i].AdapterName;
                                                        if (vendorID == AMD_VENDOR_ID
                                                            || devName.ToLower().Contains("amd")
                                                            || devName.ToLower().Contains("radeon")
                                                            || devName.ToLower().Contains("firepro")) {

                                                            string PNPStr = OSAdapterInfoData.ADLAdapterInfo[i].PNPString;
                                                            var backSlashLast = PNPStr.LastIndexOf('\\');
                                                            var serial = PNPStr.Substring(backSlashLast, PNPStr.Length - backSlashLast);
                                                            var end_0 = serial.IndexOf('&');
                                                            var end_1 = serial.IndexOf('&', end_0 + 1);
                                                            // get serial
                                                            serial = serial.Substring(end_0 + 1, (end_1 - end_0) - 1);

                                                            var udid = OSAdapterInfoData.ADLAdapterInfo[i].UDID;
                                                            var pciVen_id_strSize = 21; // PCI_VEN_XXXX&DEV_XXXX
                                                            var uuid = udid.Substring(0, pciVen_id_strSize) + "_" + serial;
                                                            int budId = OSAdapterInfoData.ADLAdapterInfo[i].BusNumber;
                                                            if (!_amdDeviceUUID.Contains(uuid)) {
                                                                try {
                                                                    Helpers.ConsolePrint(TAG, String.Format("ADL device added BusNumber:{0}  NAME:{1}  UUID:{2}"),
                                                                        budId,
                                                                        devName,
                                                                        uuid);
                                                                } catch { }
                                                                
                                                                _amdDeviceUUID.Add(uuid);
                                                                //_busIds.Add(OSAdapterInfoData.ADLAdapterInfo[i].BusNumber);
                                                                _amdDeviceName.Add(devName);
                                                                if (!_busIdsInfo.ContainsKey(budId)) {
                                                                    var nameUuid = new Tuple<string, string>(devName, uuid);
                                                                    _busIdsInfo.Add(budId, nameUuid);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            } else {
                                                Helpers.ConsolePrint(TAG, "ADL_Adapter_AdapterInfo_Get() returned error code " + ADLRet.ToString());
                                            }
                                        }
                                        // Release the memory for the AdapterInfo structure
                                        if (IntPtr.Zero != AdapterBuffer)
                                            Marshal.FreeCoTaskMem(AdapterBuffer);
                                    }
                                }
                                if (null != ADL.ADL_Main_Control_Destroy)
                                    ADL.ADL_Main_Control_Destroy();
                            } else {
                                // TODO
                                Helpers.ConsolePrint(TAG, "ADL_Main_Control_Create() returned error code " + ADLRet.ToString());
                                Helpers.ConsolePrint(TAG, "Check if ADL is properly installed!");
                            }
                        } catch (Exception ex) {
                            Helpers.ConsolePrint(TAG, "AMD ADL exception: " + ex.Message);
                            isAdlInit = false;
                        }
                        if(isAdlInit) {
                            if (amdGpus.Count == _amdDeviceUUID.Count) {
                                Helpers.ConsolePrint(TAG, "AMD OpenCL and ADL AMD query COUNTS GOOD/SAME");
                            } else {
                                Helpers.ConsolePrint(TAG, "AMD OpenCL and ADL AMD query COUNTS DIFFERENT/BAD");
                            }
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.AppendLine("");
                            stringBuilder.AppendLine("QueryAMD devices: ");
                            for (int i_id = 0; i_id < amdGpus.Count; ++i_id) {
                                HasAMD = true;

                                int busID = amdGpus[i_id].AMD_BUS_ID;
                                if (busID != -1 && _busIdsInfo.ContainsKey(busID)) {
                                    var deviceName = _busIdsInfo[busID].Item1;
                                    var newAmdDev = new AmdGpuDevice(amdGpus[i_id], deviceDriverOld[deviceName]);
                                    newAmdDev.DeviceName = deviceName;
                                    newAmdDev.UUID = _busIdsInfo[busID].Item2;
                                    bool isDisabledGroup = ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionAMD;
                                    string skipOrAdd = isDisabledGroup ? "SKIPED" : "ADDED";
                                    string isDisabledGroupStr = isDisabledGroup ? " (AMD group disabled)" : "";
                                    string etherumCapableStr = newAmdDev.IsEtherumCapable() ? "YES" : "NO";

                                    new ComputeDevice(newAmdDev, true, true);
                                    // just in case 
                                    try {
                                        stringBuilder.AppendLine(String.Format("\t{0} device{1}:", skipOrAdd, isDisabledGroupStr));
                                        stringBuilder.AppendLine(String.Format("\t\tID: {0}", newAmdDev.DeviceID.ToString()));
                                        stringBuilder.AppendLine(String.Format("\t\tNAME: {0}", newAmdDev.DeviceName));
                                        stringBuilder.AppendLine(String.Format("\t\tCODE_NAME: {0}", newAmdDev.Codename));
                                        stringBuilder.AppendLine(String.Format("\t\tUUID: {0}", newAmdDev.UUID));
                                        stringBuilder.AppendLine(String.Format("\t\tMEMORY: {0}", newAmdDev.DeviceGlobalMemory.ToString()));
                                        stringBuilder.AppendLine(String.Format("\t\tETHEREUM: {0}", etherumCapableStr));
                                    } catch { }
                                } else {
                                    stringBuilder.AppendLine(String.Format("\tDevice not added, Bus No. {0} not found:", busID));
                                }
                            }
                            Helpers.ConsolePrint(TAG, stringBuilder.ToString());
                        }
                    }
                }
            }
            Helpers.ConsolePrint(TAG, "QueryAMD END");
        }

        private void UncheckedCPU() {
            // Auto uncheck CPU if any GPU is found
            var cdgm = ComputeDeviceGroupManager.Instance;
            if (cdgm.ContainsGPUs) cdgm.DisableCpuGroup();
        }

        #region NEW IMPLEMENTATION

        #region CUDA, NVIDIA Query

        string QueryCudaDevicesString = "";
        List<CudaDevice> CudaDevices = new List<CudaDevice>();
        private void QueryCudaDevicesOutputErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data != null) {
                QueryCudaDevicesString += e.Data;
            }
        }

        private bool IsSMGroupSkip(int sm_major) {
            if (sm_major == 6) {
                return ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia6X;
            }
            if (sm_major == 5) {
                return ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia5X;
            }
            if (sm_major == 3) {
                return ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia3X;
            }
            if (sm_major == 2) {
                return ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia2X;
            }
            return false;
        }

        private void QueryCudaDevices() {
            Helpers.ConsolePrint(TAG, "QueryCudaDevices START");
            Process CudaDevicesDetection = new Process();
            CudaDevicesDetection.StartInfo.FileName = "CudaDeviceDetection.exe";
            CudaDevicesDetection.StartInfo.UseShellExecute = false;
            CudaDevicesDetection.StartInfo.RedirectStandardError = true;
            CudaDevicesDetection.StartInfo.RedirectStandardOutput = true;
            CudaDevicesDetection.StartInfo.CreateNoWindow = true;
            CudaDevicesDetection.OutputDataReceived += QueryCudaDevicesOutputErrorDataReceived;
            CudaDevicesDetection.ErrorDataReceived += QueryCudaDevicesOutputErrorDataReceived;

            const int waitTime = 5 * 1000; // 5seconds
            try {
                if (!CudaDevicesDetection.Start()) {
                    Helpers.ConsolePrint(TAG, "CudaDevicesDetection process could not start");
                } else {
                    CudaDevicesDetection.BeginErrorReadLine();
                    CudaDevicesDetection.BeginOutputReadLine();
                    if (CudaDevicesDetection.WaitForExit(waitTime)) {
                        CudaDevicesDetection.Close();
                    }
                }
            } catch (Exception ex) {
                // TODO
                Helpers.ConsolePrint(TAG, "CudaDevicesDetection threw Exception: " + ex.Message);
            } finally {
                if (QueryCudaDevicesString != "") {
                    try {
                        CudaDevices = JsonConvert.DeserializeObject<List<CudaDevice>>(QueryCudaDevicesString, Globals.JsonSettings);
                    } catch { }
                }
            }
            if (CudaDevices != null && CudaDevices.Count != 0) {
                HasNVIDIA = true;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("CudaDevicesDetection:");
                foreach (var cudaDev in CudaDevices) {
                    // check sm vesrions
                    bool isUnderSM21;
                    {
                        bool isUnderSM2_major = cudaDev.SM_major < 2;
                        bool isUnderSM1_minor = cudaDev.SM_minor < 1;
                        isUnderSM21 = isUnderSM2_major && isUnderSM1_minor;
                    }
                    //bool isOverSM6 = cudaDev.SM_major > 6;
                    bool isDisabledGroup = IsSMGroupSkip(cudaDev.SM_major);
                    bool skip = isUnderSM21 || isDisabledGroup;
                    string skipOrAdd = skip ? "SKIPED" : "ADDED";
                    string isDisabledGroupStr = isDisabledGroup ? " (SM group disabled)" : "";
                    string etherumCapableStr = cudaDev.IsEtherumCapable() ? "YES" : "NO";
                    stringBuilder.AppendLine(String.Format("\t{0} device{1}:", skipOrAdd, isDisabledGroupStr));
                    stringBuilder.AppendLine(String.Format("\t\tID: {0}", cudaDev.DeviceID.ToString()));
                    stringBuilder.AppendLine(String.Format("\t\tNAME: {0}", cudaDev.GetName()));
                    stringBuilder.AppendLine(String.Format("\t\tVENDOR: {0}", cudaDev.VendorName));
                    stringBuilder.AppendLine(String.Format("\t\tUUID: {0}", cudaDev.UUID));
                    stringBuilder.AppendLine(String.Format("\t\tSM: {0}", cudaDev.SMVersionString));
                    stringBuilder.AppendLine(String.Format("\t\tMEMORY: {0}", cudaDev.DeviceGlobalMemory.ToString()));
                    stringBuilder.AppendLine(String.Format("\t\tETHEREUM: {0}", etherumCapableStr));
                    
                    if (!skip) {
                        string group;
                        switch (cudaDev.SM_major) {
                            case 2:
                                group = GroupNames.GetName(DeviceGroupType.NVIDIA_2_1);
                                break;
                            case 3:
                                group = GroupNames.GetName(DeviceGroupType.NVIDIA_3_x);
                                break;
                            case 5:
                                group = GroupNames.GetName(DeviceGroupType.NVIDIA_5_x);
                                break;
                            case 6:
                                group = GroupNames.GetName(DeviceGroupType.NVIDIA_6_x);
                                break;
                            default:
                                group = GroupNames.GetName(DeviceGroupType.NVIDIA_6_x);
                                break;
                        }
                        new ComputeDevice(cudaDev, group, true);
                    }
                }
                Helpers.ConsolePrint(TAG, stringBuilder.ToString());
            } else {
                Helpers.ConsolePrint(TAG, "CudaDevicesDetection found no devices. CudaDevicesDetection returned: " + QueryCudaDevicesString);
            }
            Helpers.ConsolePrint(TAG, "QueryCudaDevices END");
        }

        #endregion // CUDA, NVIDIA Query


        #region OpenCL Query

        private double GetNvidiaOpenCLDriver() {
            if (OpenCLJSONData != null) {
                List<OpenCLDevice> nvidiaOCLs = null;
                foreach (var oclPlatDevs in OpenCLJSONData.OCLPlatformDevices) {
                    if (oclPlatDevs.Key.ToLower().Contains("nvidia")) {
                        nvidiaOCLs = oclPlatDevs.Value;
                    }
                }

                if (nvidiaOCLs != null && nvidiaOCLs.Count > 0) {
                    if (Double.TryParse(nvidiaOCLs[0]._CL_DRIVER_VERSION,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out _currentNvidiaOpenCLDriver)) {
                        return _currentNvidiaOpenCLDriver;
                    }
                }
            }

            return -1;
        }

        class OpenCLJSON {
            public Dictionary<string, int> OCLPlatforms = new Dictionary<string,int>();
            public Dictionary<string, List<OpenCLDevice>> OCLPlatformDevices = new Dictionary<string,List<OpenCLDevice>>();
        }
        string QueryOpenCLDevicesString = "";
        OpenCLJSON OpenCLJSONData = new OpenCLJSON();
        bool IsOpenCLQuerrySuccess = false;
        private void QueryOpenCLDevicesOutputErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data != null) {
                QueryOpenCLDevicesString += e.Data;
            }
        }

        private void QueryOpenCLDevices() {
            Helpers.ConsolePrint(TAG, "QueryOpenCLDevices START");
            Process OpenCLDevicesDetection = new Process();
            OpenCLDevicesDetection.StartInfo.FileName = "AMDOpenCLDeviceDetection.exe";
            OpenCLDevicesDetection.StartInfo.UseShellExecute = false;
            OpenCLDevicesDetection.StartInfo.RedirectStandardError = true;
            OpenCLDevicesDetection.StartInfo.RedirectStandardOutput = true;
            OpenCLDevicesDetection.StartInfo.CreateNoWindow = true;
            OpenCLDevicesDetection.OutputDataReceived += QueryOpenCLDevicesOutputErrorDataReceived;
            OpenCLDevicesDetection.ErrorDataReceived += QueryOpenCLDevicesOutputErrorDataReceived;

            const int waitTime = 5 * 1000; // 5seconds
            try {
                if (!OpenCLDevicesDetection.Start()) {
                    Helpers.ConsolePrint(TAG, "AMDOpenCLDeviceDetection process could not start");
                } else {
                    OpenCLDevicesDetection.BeginErrorReadLine();
                    OpenCLDevicesDetection.BeginOutputReadLine();
                    if (OpenCLDevicesDetection.WaitForExit(waitTime)) {
                        OpenCLDevicesDetection.Close();
                    }
                }
            } catch(Exception ex) {
                // TODO
                Helpers.ConsolePrint(TAG, "AMDOpenCLDeviceDetection threw Exception: " + ex.Message);
            } finally {
                if (QueryOpenCLDevicesString != "") {
                    try {
                        OpenCLJSONData = JsonConvert.DeserializeObject<OpenCLJSON>(QueryOpenCLDevicesString, Globals.JsonSettings);
                    } catch { }
                }
            }

            if (OpenCLJSONData == null) {
                Helpers.ConsolePrint(TAG, "AMDOpenCLDeviceDetection found no devices. AMDOpenCLDeviceDetection returned: " + QueryOpenCLDevicesString);
            } else {
                IsOpenCLQuerrySuccess = true;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("AMDOpenCLDeviceDetection found devices success:");
                foreach (var kvp in OpenCLJSONData.OCLPlatformDevices) {
                    stringBuilder.AppendLine(String.Format("\tFound devices for platform: {0}", kvp.Key));
                    foreach (var oclDev in kvp.Value) {
                        stringBuilder.AppendLine("\t\tDevice:");
                        stringBuilder.AppendLine(String.Format("\t\t\tDevice ID {0}", oclDev.DeviceID));
                        stringBuilder.AppendLine(String.Format("\t\t\tDevice NAME {0}", oclDev._CL_DEVICE_NAME));
                        stringBuilder.AppendLine(String.Format("\t\t\tDevice TYPE {0}", oclDev._CL_DEVICE_TYPE));
                    }
                }
                Helpers.ConsolePrint(TAG, stringBuilder.ToString());
            }
            Helpers.ConsolePrint(TAG, "QueryOpenCLDevices END");
        }

        #endregion OpenCL Query


        #endregion // NEW IMPLEMENTATION

    }
}
