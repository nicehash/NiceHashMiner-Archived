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

        public IMessageNotifier MessageNotifier { get; private set; }

        public void QueryDevices(IMessageNotifier messageNotifier)
        {
            MessageNotifier = messageNotifier;
            // Order important CPU Query must be first
            // #1 CPU
            QueryCPUs();
            // #2 NVIDIA
            QueryNVIDIA();
            // #3 AMD
            QueryAMD();
            // #4 uncheck CPU if GPUs present, call it after we Query all devices
            UncheckedCPU();
            // #5 remove reference
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

        private void QueryNVIDIA() {
            // TODO international
            showMessageAndStep("Querying CUDA devices");
            QueryCudaDevices();
        }

        private void QueryAMD() {
            showMessageAndStep(International.GetText("form1_loadtext_AMD"));
            var dump = new sgminer(true);
        }

        private void UncheckedCPU() {
            // Auto uncheck CPU if any GPU is found
            var cdgm = ComputeDeviceGroupManager.Instance;
            if (cdgm.ContainsGPUs) cdgm.DisableCpuGroup();
        }

        #region NEW IMPLEMENTATION

        #region NVIDIA Query

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
            } catch {
                // TODO
                Helpers.ConsolePrint(TAG, "CudaDevicesDetection threw Exception");
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

        #endregion // NVIDIA Query



        #endregion // NEW IMPLEMENTATION

    }
}
