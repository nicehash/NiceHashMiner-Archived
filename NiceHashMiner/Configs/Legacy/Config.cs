using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Configs
{
    public class Algo
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double BenchmarkSpeed;
        public bool Skip;
        public bool[] DisabledDevices;
#pragma warning restore 649
    }

    public class Group
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public int APIBindPort;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double MinimumProfit;
        public int DaggerHashimotoGenerateDevice;
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
        public Version ConfigFileVersion;
        public int Language;
        public string DisplayCurrency;
        public bool DebugConsole;
        public string BitcoinAddress;
        public string WorkerName;
        public int ServiceLocation;
        public bool AutoStartMining;
        public bool HideMiningWindows;
        public bool MinimizeToTray;
        public int LessThreads;
        public int ForceCPUExtension; // 0 - automatic, 1 - SSE2, 2 - AVX, 3 - AVX2
        public int SwitchMinSecondsFixed;
        public int SwitchMinSecondsDynamic;
        public int SwitchMinSecondsAMD;
        public int MinerAPIQueryInterval;
        public int MinerRestartDelayMS;
        public int MinerAPIGraceSeconds;
        public int MinerAPIGraceSecondsAMD;
        public int[] BenchmarkTimeLimitsCPU;
        public int[] BenchmarkTimeLimitsNVIDIA;
        public int[] BenchmarkTimeLimitsAMD;
        public bool DisableDetectionNVidia5X;
        public bool DisableDetectionNVidia3X;
        public bool DisableDetectionNVidia2X;
        public bool DisableDetectionAMD;
        public bool DisableAMDTempControl;
        public bool AutoScaleBTCValues;
        public bool StartMiningWhenIdle;
        public int MinIdleSeconds;
        public bool LogToFile;
        public long LogMaxFileSize;  // in bytes
        public bool ShowDriverVersionWarning;
        public bool DisableWindowsErrorReporting;
        public bool UseNewSettingsPage; // depriciate this
        public bool NVIDIAP0State;
        public int ethminerAPIPortNvidia;
        public int ethminerAPIPortAMD;
        public int ethminerDefaultBlockHeight;
        public Group[] Groups;
#pragma warning restore 649

        // TODO split this into normal and benchmark settings
        public static Config ConfigData;

        public static void InitializeConfig()
        {
            // Set defaults
            ConfigData = new Config();
            SetDefaults();

            try
            {
                if (new FileInfo("config.json").Length > 17000)
                    ConfigData = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
                else
                {
                    File.Delete("config.json");
                }
            }
            catch { }

            if (ConfigData.ConfigFileVersion == null || 
                ConfigData.ConfigFileVersion.CompareTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) != 0)
            {
                Helpers.ConsolePrint("CONFIG", "Config file is from an older version of NiceHashMiner..");
                Helpers.ConsolePrint("CONFIG", "Backing up config.json to config_old.json..");
                try
                {
                    if (File.Exists("config_old.json"))
                        File.Delete("config_old.json");
                    File.Move("config.json", "config_old.json");
                } catch { }

                SetDefaults();
            }

            if (ConfigData.SwitchMinSecondsFixed <= 0)
                ConfigData.SwitchMinSecondsFixed = 90;
            if (ConfigData.SwitchMinSecondsDynamic <= 0)
                ConfigData.SwitchMinSecondsDynamic = 30;
            if (ConfigData.SwitchMinSecondsAMD <= 0)
                ConfigData.SwitchMinSecondsAMD = 60;
            if (ConfigData.MinerAPIQueryInterval <= 0)
                ConfigData.MinerAPIQueryInterval = 5;
            if (ConfigData.MinerRestartDelayMS <= 0)
                ConfigData.MinerRestartDelayMS = 500;
            if (ConfigData.MinerAPIGraceSeconds < 0)
                ConfigData.MinerAPIGraceSeconds = 0;
            if (ConfigData.MinerAPIGraceSecondsAMD < 0)
                ConfigData.MinerAPIGraceSecondsAMD = 0;
            if (ConfigData.BenchmarkTimeLimitsCPU == null || ConfigData.BenchmarkTimeLimitsCPU.Length < 3)
                ConfigData.BenchmarkTimeLimitsCPU = new int[] { 10, 20, 60 };
            if (ConfigData.BenchmarkTimeLimitsNVIDIA == null || ConfigData.BenchmarkTimeLimitsNVIDIA.Length < 3)
                ConfigData.BenchmarkTimeLimitsNVIDIA = new int[] { 10, 20, 60 };
            if (ConfigData.BenchmarkTimeLimitsAMD == null || ConfigData.BenchmarkTimeLimitsAMD.Length < 3)
                ConfigData.BenchmarkTimeLimitsAMD = new int[] { 120, 180, 240 };
            if (ConfigData.MinIdleSeconds <= 0)
                ConfigData.MinIdleSeconds = 60;
            if (ConfigData.LogMaxFileSize <= 0)
                ConfigData.LogMaxFileSize = 1048576;
            if (ConfigData.DisplayCurrency == null)
                ConfigData.DisplayCurrency = "USD";
        }

        public static void SetDefaults()
        {
            ConfigData.ConfigFileVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ConfigData.Language = 0;
            ConfigData.BitcoinAddress = "";
            ConfigData.WorkerName = "worker1";
            ConfigData.ServiceLocation = 0;
            ConfigData.LessThreads = 0;
            ConfigData.Groups = new Group[0];
            ConfigData.DebugConsole = false;
            ConfigData.HideMiningWindows = false;
            ConfigData.MinimizeToTray = false;
            ConfigData.AutoStartMining = false;
            ConfigData.DisableDetectionNVidia5X = false;
            ConfigData.DisableDetectionNVidia3X = false;
            ConfigData.DisableDetectionNVidia2X = false;
            ConfigData.DisableDetectionAMD = false;
            ConfigData.DisableAMDTempControl = false;
            ConfigData.AutoScaleBTCValues = true;
            ConfigData.StartMiningWhenIdle = false;
            ConfigData.LogToFile = true;
            ConfigData.LogMaxFileSize = 1048576;
            ConfigData.ShowDriverVersionWarning = true;
            ConfigData.DisableWindowsErrorReporting = true;
            ConfigData.UseNewSettingsPage = true;
            ConfigData.NVIDIAP0State = false;
            ConfigData.MinerRestartDelayMS = 500;
            ConfigData.ethminerAPIPortNvidia = 34561;
            ConfigData.ethminerAPIPortAMD = 34562;
            ConfigData.ethminerDefaultBlockHeight = 1700000;
            ConfigData.MinerAPIGraceSeconds = 30;
            ConfigData.MinerAPIGraceSecondsAMD = 60;
            ConfigData.SwitchMinSecondsFixed = 90;
            ConfigData.SwitchMinSecondsDynamic = 30;
            ConfigData.SwitchMinSecondsAMD = 90;
            ConfigData.MinIdleSeconds = 60;
            ConfigData.DisplayCurrency = "USD";

        }

        public static void Commit()
        {
            try { File.WriteAllText("config.json", JsonConvert.SerializeObject(ConfigData, Formatting.Indented)); }
            catch { }

        }

        public static void RebuildGroups()
        {
            //// rebuild config groups
            //Group[] CG = new Group[Globals.Miners.Length];
            //for (int i = 0; i < Globals.Miners.Length; i++)
            //{
            //    CG[i] = new Group();
            //    CG[i].Name = Globals.Miners[i].MinerDeviceName;
            //    CG[i].APIBindPort = Globals.Miners[i].APIPort;
            //    CG[i].ExtraLaunchParameters = Globals.Miners[i].ExtraLaunchParameters;
            //    CG[i].UsePassword = Globals.Miners[i].UsePassword;
            //    CG[i].MinimumProfit = Globals.Miners[i].MinimumProfit;
            //    CG[i].DaggerHashimotoGenerateDevice = Globals.Miners[i].DaggerHashimotoGenerateDevice;
            //    // scope stuff here for PORTED
            //    {
            //        // get keys
            //        var CurrentMinerKeys = Globals.Miners[i].SupportedAlgorithms.Keys;
            //        CG[i].Algorithms = new Algo[CurrentMinerKeys.Count];
            //        int k = 0;
            //        foreach (AlgorithmType algType in CurrentMinerKeys)
            //        {
            //            Algorithm curAlgo = Globals.Miners[i].SupportedAlgorithms[algType];
            //            // for ported START
            //            CG[i].Algorithms[k] = new Algo();
            //            CG[i].Algorithms[k].Name = curAlgo.NiceHashName;
            //            CG[i].Algorithms[k].BenchmarkSpeed = curAlgo.BenchmarkSpeed;
            //            CG[i].Algorithms[k].ExtraLaunchParameters = curAlgo.ExtraLaunchParameters;
            //            CG[i].Algorithms[k].UsePassword = curAlgo.UsePassword;
            //            CG[i].Algorithms[k].Skip = curAlgo.Skip;

            //            CG[i].Algorithms[k].DisabledDevices = new bool[Globals.Miners[i].CDevs.Count];
            //            for (int j = 0; j < Globals.Miners[i].CDevs.Count; j++)
            //            {
            //                CG[i].Algorithms[k].DisabledDevices[j] = curAlgo.DisabledDevice[j];
            //            }
            //            // for ported END
            //            // increment k
            //            ++k;
            //        }
            //    }
            //    List<int> DD = new List<int>();
            //    for (int k = 0; k < Globals.Miners[i].CDevs.Count; k++)
            //    {
            //        if (!Globals.Miners[i].CDevs[k].Enabled)
            //            DD.Add(k);
            //    }
            //    CG[i].DisabledDevices = DD.ToArray();
            //}
            //ConfigData.Groups = CG;
            Config.Commit();
        }

        public static bool ConfigFileExist()
        {
            if (File.Exists("config.json"))
                return true;

            return false;
        }

        // old code new methods

        // Call this after miners are initialized
        public static void SetComputeDeviceConfig() {
            //for (int i = 0; i < Globals.Miners.Length; i++) {
            //    for (int k = 0; k < Globals.Miners[i].CDevs.Count; k++) {
            //        ComputeDevice D = Globals.Miners[i].CDevs[k];
            //        if (Config.ConfigData.Groups.Length > i) {
            //            D.Enabled = true;
            //            for (int z = 0; z < Config.ConfigData.Groups[i].DisabledDevices.Length; z++) {
            //                if (Config.ConfigData.Groups[i].DisabledDevices[z] == k) {
            //                    D.Enabled = false;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public static void SetGroupAlgorithmSettup() {
            //// get algorithm settup from settings
            //for (int i = 0; i < Globals.Miners.Length; i++) {
            //    if (Config.ConfigData.Groups.Length > i) {
            //        Globals.Miners[i].ExtraLaunchParameters = Config.ConfigData.Groups[i].ExtraLaunchParameters;
            //        Globals.Miners[i].UsePassword = Config.ConfigData.Groups[i].UsePassword;
            //        Globals.Miners[i].MinimumProfit = Config.ConfigData.Groups[i].MinimumProfit;
            //        Globals.Miners[i].DaggerHashimotoGenerateDevice = Config.ConfigData.Groups[i].DaggerHashimotoGenerateDevice;
            //        if (Config.ConfigData.Groups[i].APIBindPort > 0)
            //            Globals.Miners[i].APIPort = Config.ConfigData.Groups[i].APIBindPort;

            //        // Dictionary order is non deterministic, so find a way to merge this crap
            //        bool isSameSize = Config.ConfigData.Groups[i].Algorithms.Length == Globals.Miners[i].SupportedAlgorithms.Count;
            //        if (isSameSize == false) Helpers.ConsolePrint("Miner Algos settup", "SIZE IS NOT SAME");
            //        foreach (var key in Globals.Miners[i].SupportedAlgorithms.Keys) {
            //            var currentAlg = Globals.Miners[i].SupportedAlgorithms[key];
            //            Algo configAlgo = null;
            //            // find configAlgo
            //            for (int z = 0; z < Config.ConfigData.Groups[i].Algorithms.Length; z++) {
            //                Algo compareConfigAlgo = Config.ConfigData.Groups[i].Algorithms[z];
            //                if (currentAlg.NiceHashName == compareConfigAlgo.Name) {
            //                    configAlgo = compareConfigAlgo;
            //                    break;
            //                }
            //            }
            //            if (configAlgo != null) {
            //                currentAlg.BenchmarkSpeed = configAlgo.BenchmarkSpeed;
            //                currentAlg.ExtraLaunchParameters = configAlgo.ExtraLaunchParameters;
            //                currentAlg.UsePassword = configAlgo.UsePassword;
            //                currentAlg.Skip = configAlgo.Skip;

            //                //currentAlg.DisabledDevice = new bool[Globals.Miners[i].CDevs.Count];
            //                //if (configAlgo.DisabledDevices != null) {
            //                //    if (configAlgo.DisabledDevices.Length < Globals.Miners[i].CDevs.Count) {
            //                //        for (int j = 0; j < configAlgo.DisabledDevices.Length; j++)
            //                //            currentAlg.DisabledDevice[j] = configAlgo.DisabledDevices[j];
            //                //        for (int j = configAlgo.DisabledDevices.Length; j < Globals.Miners[i].CDevs.Count; j++)
            //                //            currentAlg.DisabledDevice[j] = false;
            //                //    } else {
            //                //        for (int j = 0; j < Globals.Miners[i].CDevs.Count; j++)
            //                //            currentAlg.DisabledDevice[j] = configAlgo.DisabledDevices[j];
            //                //    }
            //                //} else {
            //                //    Globals.Miners[i].GetDisabledDevicePerAlgo();
            //                //}
            //            } else {
            //                Helpers.ConsolePrint("Miner Algos settup", "DIDN'T FIND configAlgo!!!");
            //            }
            //        }
            //    } else {
            //        Globals.Miners[i].GetDisabledDevicePerAlgo();
            //    }
            //}
        }

    }
}
