using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    public class Group
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public int[] DisabledDevices;
        public double[] BenchmarkSpeeds;
#pragma warning restore 649
    }

    public class Config
    {
#pragma warning disable 649
        public bool DebugConsole;
        public string BitcoinAddress;
        public string WorkerName;
        public int Location;
        public int LessThreads;
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
                CG[i].BenchmarkSpeeds = new double[Form1.Miners[i].SupportedAlgorithms.Length];
                for (int k = 0; k < Form1.Miners[i].SupportedAlgorithms.Length; k++)
                    CG[i].BenchmarkSpeeds[k] = Form1.Miners[i].SupportedAlgorithms[k].BenchmarkSpeed;
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
