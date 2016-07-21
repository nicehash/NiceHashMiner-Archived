using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Interfaces;

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceQueryManager class is used to query ComputeDevices avaliable on the system.
    /// Query CPUs, GPUs [Nvidia, AMD]
    /// TODO For now it depends on current Miners implementation, bound to change
    /// CPU query stays the same
    /// GPU querying must change
    /// </summary>
    public class ComputeDeviceQueryManager
    {
        #region SINGLETON Stuff
        private static ComputeDeviceQueryManager _instance = new ComputeDeviceQueryManager();

        public static ComputeDeviceQueryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ComputeDeviceQueryManager();
                }
                return _instance;
            }
        }
        #endregion

        private ComputeDeviceQueryManager() { }


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
            // uncheck CPU if GPUs present
            UncheckedCPU();
            // remove reference
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

            if (CPUs == 1)
                Globals.Miners[0] = new cpuminer(0, ThreadsPerCPU, 0);
            else
            {
                for (int i = 0; i < CPUs; i++)
                    Globals.Miners[i] = new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask));
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

    }
}
