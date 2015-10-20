using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    public class Algo
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double BenchmarkSpeed;
#pragma warning restore 649
    }

    public class Group
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
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
        public int LessThreads;
        public int SwitchMinSecondsFixed;
        public int SwitchMinSecondsDynamic;
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

            try { ConfigData = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")); }
            catch { }

            if (ConfigData.SwitchMinSecondsFixed <= 0)
                ConfigData.SwitchMinSecondsFixed = 3 * 60;
            if (ConfigData.SwitchMinSecondsDynamic <= 0)
                ConfigData.SwitchMinSecondsDynamic = 3 * 60;
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
