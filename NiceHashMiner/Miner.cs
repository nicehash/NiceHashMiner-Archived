using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace NiceHashMiner
{
    class APIData
    {
        public int AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }

    class GPUData
    {
        public int ID;
        public string Name;
        public bool Enabled;

        public GPUData(int id, string n)
        {
            ID = id;
            Name = n;
            Enabled = true;
        }
    }

    abstract class Miner
    {
        public List<GPUData> GPUs;

        protected Algorithm[] SupportedAlgorithms;
        protected string Path;
        protected int APIPort;
        protected Process ProcessHandle;

        public Miner()
        {
        }

        abstract public APIData GetSummary();

        abstract public void Start(string suburl, string username);

        virtual public void Stop()
        {
            if (ProcessHandle != null)
            {
                try { ProcessHandle.Kill(); }
                catch { }
                ProcessHandle.Close();
                ProcessHandle = null;
            }
        }

        abstract protected void QueryGPUs();

        protected void FillAlgorithm(string aname, ref APIData AD)
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].MinerName == aname)
                {
                    AD.AlgorithmID = SupportedAlgorithms[i].NiceHashID;
                    AD.AlgorithmName = SupportedAlgorithms[i].NiceHashName;
                }
            }
        }
    }
}
