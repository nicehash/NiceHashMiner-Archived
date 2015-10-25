using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    public class SubConfigClass
    { }

    public class Algo : SubConfigClass
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double BenchmarkSpeed;
        public bool Skip;
#pragma warning restore 649
    }

    public class Group : SubConfigClass
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public int APIBindPort;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public int[] DisabledDevices;
        public Algo[] Algorithms;
#pragma warning restore 649

        public Group()
        {
            DisabledDevices = new int[0];
            Algorithms = new Algo[0];
        }
    }

    public class Config
    {
#pragma warning disable 649
        public bool DebugConsole;
        public string BitcoinAddress;
        public string WorkerName;
        public int Location;
        public bool AutoStartMining;
        public bool HideMiningWindows;
        public int LessThreads;
        public int ForceCPUExtension; // 0 - automatic, 1 - SSE2, 2 - AVX, 3 - AVX2
        public int SwitchMinSecondsFixed;
        public int SwitchMinSecondsDynamic;
        public int MinerAPIQueryInterval;
        public int MinerRestartDelayMS;
        public int[] BenchmarkTimeLimits;
        public bool StartMiningWhenIdle;
        public int MinIdleSeconds;
        public Group[] Groups;
#pragma warning restore 649

        public static Config ConfigData;

        static Config()
        {
            // Set defaults
            ConfigData = new Config();
            ConfigData.BitcoinAddress = "1PJ5HWjAniHPMuvfu89L6D2CmnL1De1syn";
            ConfigData.WorkerName = "worker1";
            ConfigData.Location = 0;
            ConfigData.LessThreads = 0;
            ConfigData.Groups = new Group[0];
            ConfigData.DebugConsole = false;
            ConfigData.HideMiningWindows = false;
            ConfigData.AutoStartMining = false;
            ConfigData.StartMiningWhenIdle = false;

            try { ConfigData = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")); }
            catch { }

            if (ConfigData.SwitchMinSecondsFixed <= 0)
                ConfigData.SwitchMinSecondsFixed = 3 * 60;
            if (ConfigData.SwitchMinSecondsDynamic <= 0)
                ConfigData.SwitchMinSecondsDynamic = 3 * 60;
            if (ConfigData.MinerAPIQueryInterval <= 0)
                ConfigData.MinerAPIQueryInterval = 5;
            if (ConfigData.MinerRestartDelayMS <= 0)
                ConfigData.MinerRestartDelayMS = 200;
            if (ConfigData.BenchmarkTimeLimits == null || ConfigData.BenchmarkTimeLimits.Length < 3)
                ConfigData.BenchmarkTimeLimits = new int[] { 10, 20, 60 };
            if (ConfigData.MinIdleSeconds <= 0)
                ConfigData.MinIdleSeconds = 60;
        }

        public static void Commit()
        {
            try { File.WriteAllText("config.json", JsonConvert.SerializeObject(ConfigData, Formatting.Indented)); }
            catch { }
        }

        public static void RebuildGroups()
        {
            // rebuild config groups
            Group[] CG = new Group[Form1.Miners.Length];
            for (int i = 0; i < Form1.Miners.Length; i++)
            {
                CG[i] = new Group();
                CG[i].Name = Form1.Miners[i].MinerDeviceName;
                CG[i].APIBindPort = Form1.Miners[i].APIPort;
                CG[i].ExtraLaunchParameters = Form1.Miners[i].ExtraLaunchParameters;
                CG[i].UsePassword = Form1.Miners[i].UsePassword;
                CG[i].Algorithms = new Algo[Form1.Miners[i].SupportedAlgorithms.Length];
                for (int k = 0; k < Form1.Miners[i].SupportedAlgorithms.Length; k++)
                {
                    CG[i].Algorithms[k] = new Algo();
                    CG[i].Algorithms[k].Name = Form1.Miners[i].SupportedAlgorithms[k].NiceHashName;
                    CG[i].Algorithms[k].BenchmarkSpeed = Form1.Miners[i].SupportedAlgorithms[k].BenchmarkSpeed;
                    CG[i].Algorithms[k].ExtraLaunchParameters = Form1.Miners[i].SupportedAlgorithms[k].ExtraLaunchParameters;
                    CG[i].Algorithms[k].UsePassword = Form1.Miners[i].SupportedAlgorithms[k].UsePassword;
                    CG[i].Algorithms[k].Skip = Form1.Miners[i].SupportedAlgorithms[k].Skip;
                }
                List<int> DD = new List<int>();
                for (int k = 0; k < Form1.Miners[i].CDevs.Count; k++)
                {
                    if (!Form1.Miners[i].CDevs[k].Enabled)
                        DD.Add(k);
                }
                CG[i].DisabledDevices = DD.ToArray();
            }
            ConfigData.Groups = CG;
            Config.Commit();
        }
    }
}
