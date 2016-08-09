using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Interfaces;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

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
        // change to protected after .NET upgrade
        protected ComputeDeviceQueryManager() { }


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
            List<ccminer> dump = new List<ccminer>();
            // TODO International
            // NVIDIA6x
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA5X"));
            dump.Add(new ccminer_sm6x(true));
            // NVIDIA5x
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA5X"));
            dump.Add(new ccminer_sm5x(true));
            // NVIDIA3X
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA3X"));
            dump.Add(new ccminer_sm3x(true));
            // NVIDIA2X
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA2X"));
            dump.Add(new ccminer_sm21(true));
            dump = null;
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

        // TODO add new GPU guery methods
        /// The new query methods will be based on ccminer and sgminer

        #region NVIDIA ccminer UNUSED
        private DeviceGroupType getNvidiaGroupType(string name) {
            if(name.Contains("SM 2.1")) {
                return DeviceGroupType.NVIDIA_2_1;
            }
            if(name.Contains("SM 3.")) {
                return DeviceGroupType.NVIDIA_3_x;
            }
            if (name.Contains("SM 5.")) {
                return DeviceGroupType.NVIDIA_5_x;
            }
            if (name.Contains("SM 6.")) {
                return DeviceGroupType.NVIDIA_6_x;
            }
            return DeviceGroupType.NONE;
        }

        private void AddPotentialNvidiaCDev(string text)
        {
            if (!text.Contains("GPU")) return;
            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            DeviceGroupType groupType = getNvidiaGroupType(name);

            if (groupType != DeviceGroupType.NONE) {
                string DeviceGroupName = GroupNames.GetName(groupType);
                Helpers.ConsolePrint(DeviceGroupName, "Detected: " + text);
                
                string saveName = name.Substring(8);
                // TODO will be added to global
                // TODO this is NO GOOD
                new ComputeDevice(id, DeviceGroupName, saveName, null, true);
                Helpers.ConsolePrint(DeviceGroupName, "Added: " + saveName);
            }
        }

        private void QueryNVIDIA_ccminers() {
            string[] ccminerPaths = new string[] {
                MinerPaths.ccminer_decred,
                //MinerPaths.ccminer_nanashi_lyra2rev2,
                MinerPaths.ccminer_nanashi,
                MinerPaths.ccminer_neoscrypt,
                MinerPaths.ccminer_sp,
                MinerPaths.ccminer_tpruvot,
            };
            Dictionary<string, List<ComputeDevice>> QueryComputeDevices;

            // TODO rethnk this it makes sense to make one class to query and a miner factory that returns the correct miner for the device
        }

        private List<ComputeDevice> ccminerQueryDev(string ccminerPath) {
            List<ComputeDevice> retDevices = new List<ComputeDevice>();



            return retDevices;
        }



        #endregion // NVIDIA ccminer UNUSED



        #endregion // NEW IMPLEMENTATION

    }
}
