using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Interfaces;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceQueryManager class is used to query ComputeDevices avaliable on the system.
    /// Query CPUs, GPUs [Nvidia, AMD]
    /// TODO For now it depends on current Miners implementation, bound to change
    /// CPU query stays the same
    /// GPU querying should change
    /// </summary>
    public class ComputeDeviceQueryManager : SingletonTemplate<ComputeDeviceQueryManager>
    {
        // change to protected after .NET upgrade
        public ComputeDeviceQueryManager() { }


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

            if (!Helpers.InternalCheckIsWow64() && !Config.ConfigData.AutoStartMining)
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
            ThreadsPerCPU -= Config.ConfigData.LessThreads;
            if (ThreadsPerCPU < 1)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMiningLessThreadMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            // TODO this is currently tightly couped with miners, who handle device querying
            // TODO bound to change
            Globals.Miners = new Miner[CPUs + 4];

            if (CPUs == 1) {
                Globals.Miners[0] = new cpuminer(0, ThreadsPerCPU, 0);
            }
            else {
                for (int i = 0; i < CPUs; i++) {
                    Globals.Miners[i] = new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask));
                }
            }
        }

        private void QueryNVIDIA() {
            // NVIDIA5x
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA5X"));
            Globals.Miners[CPUs] = new ccminer_sp();
            // NVIDIA3X
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA3X"));
            Globals.Miners[CPUs + 1] = new ccminer_tpruvot();
            // NVIDIA2X
            showMessageAndStep(International.GetText("form1_loadtext_NVIDIA2X"));
            Globals.Miners[CPUs + 2] = new ccminer_tpruvot_sm21();
        }

        private void QueryAMD() {
            showMessageAndStep(International.GetText("form1_loadtext_AMD"));
            Globals.Miners[CPUs + 3] = new sgminer();
        }

        private void UncheckedCPU() {
            // Auto uncheck CPU if any GPU is found
            for (int i = 0; i < CPUs; i++)
            {
                try
                {
                    if ((Globals.Miners[CPUs + 0].CDevs.Count > 0 || Globals.Miners[CPUs + 1].CDevs.Count > 0 || Globals.Miners[CPUs + 2].CDevs.Count > 0 || Globals.Miners[CPUs + 3].CDevs.Count > 0) && i < CPUs)
                    {
                        Globals.Miners[i].CDevs[0].Enabled = false;
                    }
                }
                catch { }
            }
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
            if (name.Contains("SM 5.") || name.Contains("SM 6.")) { // TEMP until support for SM 6.x
                return DeviceGroupType.NVIDIA_5_x;
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
            // TODO rethnk this it makes sense to make one class to query and a miner factory that returns the correct miner for the device
        }

        #endregion // NVIDIA ccminer UNUSED



        #endregion // NEW IMPLEMENTATION

    }
}
