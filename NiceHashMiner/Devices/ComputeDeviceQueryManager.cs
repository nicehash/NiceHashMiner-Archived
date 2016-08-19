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

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceQueryManager class is used to query ComputeDevices avaliable on the system.
    /// Query CPUs, GPUs [Nvidia, AMD]
    /// TODO For now it depends on current Miners implementation, bound to change
    /// CPU query stays the same
    /// GPU querying should change
    /// </summary>
    public class ComputeDeviceQueryManager : BaseLazySingleton<ComputeDeviceQueryManager>
    {
        readonly string TAG;
        // change to protected after .NET upgrade
        protected ComputeDeviceQueryManager() {
            TAG = typeof(ComputeDeviceQueryManager).Name;
        }

        public int CPUs { get; private set; }

        public int AMDOpenCLPlatformNum { get; private set; }
        public string AMDOpenCLPlatformStringKey { get; private set; }

        public IMessageNotifier MessageNotifier { get; private set; }

        public void QueryDevices(IMessageNotifier messageNotifier)
        {
            MessageNotifier = messageNotifier;
            // Order important CPU Query must be first
            // #1 CPU
            QueryCPUs();
            // #2 CUDA
            showMessageAndStep("Querying CUDA devices");
            QueryCudaDevices();
            // #3 OpenCL
            showMessageAndStep("Querying OpenCL devices");
            QueryOpenCLDevices();
            // #4 AMD query AMD from OpenCL devices, get serial and add devices
            QueryAMD();
            // #5 uncheck CPU if GPUs present, call it after we Query all devices
            UncheckedCPU();
            // #6 remove reference
            MessageNotifier = null;
        }

        private void showMessageAndStep(string infoMsg) {
            if (MessageNotifier != null) MessageNotifier.SetMessageAndIncrementStep(infoMsg);
        } 

        private void QueryCPUs() {
            // get all CPUs
            CPUs = CPUID.GetPhysicalProcessorCount();

            // get all cores (including virtual - HT can benefit mining)
            int ThreadsPerCPU = CPUID.GetVirtualCoresCount() / CPUs;

            if (!Helpers.InternalCheckIsWow64() && !ConfigManager.Instance.GeneralConfig.AutoStartMining)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMining64bitMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            if (ThreadsPerCPU * CPUs > 64)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMining64CoresMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            int ThreadsPerCPUMask = ThreadsPerCPU;
            ThreadsPerCPU -= ConfigManager.Instance.GeneralConfig.LessThreads;
            if (ThreadsPerCPU < 1)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMiningLessThreadMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }
            
            if (CPUs == 1) {
                MinersManager.Instance.AddCpuMiner(new cpuminer(0, ThreadsPerCPU, 0), 0, CPUID.GetCPUName().Trim());
            }
            else {
                for (int i = 0; i < CPUs; i++) {
                    MinersManager.Instance.AddCpuMiner(new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask)),
                        i, CPUID.GetCPUName().Trim());
                }
            }
        }

        private void QueryAMD() {
            //showMessageAndStep(International.GetText("form1_loadtext_AMD"));
            //var dump = new sgminer(true);

            // get platform version
            showMessageAndStep("Checking AMD OpenCL GPUs");
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
                    var amdOCLDevices = OpenCLJSONData.OCLPlatformDevices[AMDOpenCLPlatformStringKey];
                    if (amdOCLDevices.Count == 0) {
                        Helpers.ConsolePrint(TAG, "AMD GPUs count is 0");
                    } else {
                        Helpers.ConsolePrint(TAG, "AMD GPUs count : " + amdOCLDevices.Count.ToString());
                        Helpers.ConsolePrint(TAG, "AMD Getting device name and serial from ADL");
                        // ADL
                        bool isAdlInit = true;
                        // ADL should get our devices in order
                        HashSet<int> _busIds = new HashSet<int>();
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
                                                        if (!_busIds.Contains(OSAdapterInfoData.ADLAdapterInfo[i].BusNumber)) {
                                                            // we are looking for amd
                                                            // TODO For now Radeon only no FirePro
                                                            var devName = OSAdapterInfoData.ADLAdapterInfo[i].AdapterName;
                                                            if (devName.Contains("AMD")
                                                                && devName.Contains("Radeon")) {
                                                                _busIds.Add(OSAdapterInfoData.ADLAdapterInfo[i].BusNumber);
                                                                _amdDeviceName.Add(devName);
                                                                var udid = OSAdapterInfoData.ADLAdapterInfo[i].UDID;
                                                                var pciVen_id_strSize = 21; // PCI_VEN_XXXX&DEV_XXXX
                                                                _amdDeviceUUID.Add(udid.Substring(0, pciVen_id_strSize));
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
                            if (amdOCLDevices.Count == _amdDeviceUUID.Count) {
                                Helpers.ConsolePrint(TAG, "AMD OpenCL and ADL AMD query COUNTS GOOD/SAME");
                                for (int i_id = 0; i_id < amdOCLDevices.Count; ++i_id) {
                                    var newAmdDev = new AmdGpuDevice(amdOCLDevices[i_id]);
                                    newAmdDev.DeviceName = _amdDeviceName[i_id];
                                    newAmdDev.UUID = _amdDeviceUUID[i_id];
                                }
                            } else {
                                Helpers.ConsolePrint(TAG, "AMD OpenCL and ADL AMD query COUNTS DIFFERENT/BAD");
                            }
                        }
                    }
                }
            }
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

        private void QueryCudaDevices() {
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
                        CudaDevicesDetection.Kill();
                    }
                }
            } catch (Exception ex) {
                // TODO
                Helpers.ConsolePrint(TAG, "CudaDevicesDetection threw Exception: " + ex.Message);
            } finally {
                if (QueryCudaDevicesString != "") {
                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    try {
                        CudaDevices = JsonConvert.DeserializeObject<List<CudaDevice>>(QueryCudaDevicesString, settings);
                    } catch { }
                }
            }
            if (CudaDevices != null && CudaDevices.Count != 0) {
                foreach (var cudaDev in CudaDevices) {
                    // check sm vesrions
                    bool isUnderSM2 = cudaDev.SM_major < 2;
                    bool isOverSM6 = cudaDev.SM_major > 6;
                    bool skip = isUnderSM2;
                    string skipOrAdd = skip ? "SKIPED" : "ADDED";
                    string etherumCapableStr = cudaDev.IsEtherumCapable() ? "YES" : "NO"; 
                    string logMessage = String.Format("CudaDevicesDetection {0} device: {1}",
                        skipOrAdd,
                        String.Format("ID: {0}, NAME: {1}, UUID: {2}, SM: {3}, MEMORY: {4}, ETHEREUM: {5}",
                        cudaDev.DeviceID.ToString(), cudaDev.DeviceName, cudaDev.UUID, cudaDev.SMVersionString,
                        cudaDev.DeviceGlobalMemory.ToString(), etherumCapableStr)
                        );
                    
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
                    Helpers.ConsolePrint(TAG, logMessage);
                }
            } else {
                Helpers.ConsolePrint(TAG, "CudaDevicesDetection found no devices. CudaDevicesDetection returned: " + QueryCudaDevicesString);
            }
        }

        #endregion // CUDA, NVIDIA Query


        #region OpenCL Query
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
            Process OpenCLDevicesDetection = new Process();
            OpenCLDevicesDetection.StartInfo.FileName = "OpenCLDeviceDetection.exe";
            OpenCLDevicesDetection.StartInfo.UseShellExecute = false;
            OpenCLDevicesDetection.StartInfo.RedirectStandardError = true;
            OpenCLDevicesDetection.StartInfo.RedirectStandardOutput = true;
            OpenCLDevicesDetection.StartInfo.CreateNoWindow = true;
            OpenCLDevicesDetection.OutputDataReceived += QueryOpenCLDevicesOutputErrorDataReceived;
            OpenCLDevicesDetection.ErrorDataReceived += QueryOpenCLDevicesOutputErrorDataReceived;

            const int waitTime = 5 * 1000; // 5seconds
            try {
                if (!OpenCLDevicesDetection.Start()) {
                    Helpers.ConsolePrint(TAG, "OpenCLDeviceDetection process could not start");
                } else {
                    OpenCLDevicesDetection.BeginErrorReadLine();
                    OpenCLDevicesDetection.BeginOutputReadLine();
                    if (OpenCLDevicesDetection.WaitForExit(waitTime)) {
                        OpenCLDevicesDetection.Kill();
                    }
                }
            } catch(Exception ex) {
                // TODO
                Helpers.ConsolePrint(TAG, "OpenCLDeviceDetection threw Exception: " + ex.Message);
            } finally {
                if (QueryOpenCLDevicesString != "") {
                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    OpenCLJSONData = JsonConvert.DeserializeObject<OpenCLJSON>(QueryOpenCLDevicesString, settings);
                    try {
                        OpenCLJSONData = JsonConvert.DeserializeObject<OpenCLJSON>(QueryOpenCLDevicesString, settings);
                    } catch { }
                }
            }
            // TODO
            if (OpenCLJSONData == null) {
                Helpers.ConsolePrint(TAG, "OpenCLDeviceDetection found no devices. OpenCLDeviceDetection returned: " + QueryOpenCLDevicesString);
            } else {
                IsOpenCLQuerrySuccess = true;
                Helpers.ConsolePrint(TAG, "OpenCLDeviceDetection found devices success.");
            }
        }

        #endregion OpenCL Query


        #endregion // NEW IMPLEMENTATION

    }
}
