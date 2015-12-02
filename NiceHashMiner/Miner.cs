using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace NiceHashMiner
{
    public class Algorithm
    {
        public int NiceHashID;
        public string NiceHashName;
        public string MinerName;
        public double BenchmarkSpeed;
        public double CurrentProfit;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public bool Skip;

        public Algorithm(int id, string nhname, string mname)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            MinerName = mname;
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = "";
            UsePassword = null;
            Skip = false;
        }

        public Algorithm(int id, string nhname, string mname, string xtraparam)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            MinerName = mname;
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = xtraparam;
            UsePassword = null;
            Skip = false;
        }
    }


    public class APIData
    {
        public int AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }


    public class ComputeDevice
    {
        public int ID;
        public string Vendor;
        public string Name;
        public bool Enabled;

        public ComputeDevice(int id, string v, string n)
        {
            ID = id;
            Vendor = v;
            Name = n;
            Enabled = true;
        }
    }

    public delegate void BenchmarkComplete(bool success, string text, object tag);

    public abstract class Miner
    {
        public string MinerDeviceName;
        public int APIPort;
        public List<ComputeDevice> CDevs;
        public Algorithm[] SupportedAlgorithms;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public int CurrentAlgo;
        public double CurrentRate;
        public bool BenchmarkSignalQuit;
        public int NumRetries;

        protected string Path;
        protected string WorkingDirectory;
        protected Process ProcessHandle;
        protected BenchmarkComplete OnBenchmarkComplete;
        protected object BenchmarkTag;
        protected int BenchmarkIndex;
        protected string LastCommandLine;
        protected double PreviousTotalMH;

        public Miner()
        {
            CDevs = new List<ComputeDevice>();

            WorkingDirectory = "";
            ExtraLaunchParameters = "";
            UsePassword = null;

            CurrentAlgo = -1;
            CurrentRate = 0;
            PreviousTotalMH = 0.0;
        }

        abstract public void Start(int nhalgo, string url, string username);

        virtual public void Stop()
        {
            if (ProcessHandle != null)
            {
                Helpers.ConsolePrint(MinerDeviceName + " Shutting down miner");
                try { ProcessHandle.Kill(); }
                catch { }
                ProcessHandle.Close();
                ProcessHandle = null;
                PreviousTotalMH = 0.0;
            }
        }

        abstract protected string BenchmarkCreateCommandLine(int index, int time);


        virtual public void BenchmarkStart(int index, int time, BenchmarkComplete oncomplete, object tag)
        {
            OnBenchmarkComplete = oncomplete;

            if (index >= SupportedAlgorithms.Length)
            {
                OnBenchmarkComplete(false, "Unknown algorithm", tag);
                return;
            }

            if (EnabledDeviceCount() == 0)
            {
                OnBenchmarkComplete(false, "Disabled", tag);
                return; // ignore, disabled device
            }

            BenchmarkTag = tag;
            BenchmarkIndex = index;

            string CommandLine = BenchmarkCreateCommandLine(index, time);

            Thread BenchmarkThread = new Thread(BenchmarkThreadRoutine);
            BenchmarkThread.Start(CommandLine);
        }


        virtual protected bool BenchmarkParseLine(string outdata)
        {
            // parse line
            if (outdata.Contains("Benchmark: ") && outdata.Contains("/s"))
            {
                int i = outdata.IndexOf("Benchmark:");
                int k = outdata.IndexOf("/s");
                string hashspeed = outdata.Substring(i + 11, k - i - 9);

                // save speed
                int b = hashspeed.IndexOf(" ");
                double spd = Double.Parse(hashspeed.Substring(0, b), CultureInfo.InvariantCulture);
                if (hashspeed.Contains("kH/s"))
                    spd *= 1000;
                else if (hashspeed.Contains("MH/s"))
                    spd *= 1000000;
                else if (hashspeed.Contains("GH/s"))
                    spd *= 1000000000;
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = spd;

                OnBenchmarkComplete(true, PrintSpeed(spd), BenchmarkTag);
                return true;
            }
            else if (outdata.Contains("Average hashrate:") && outdata.Contains("/s"))
            {
                int i = outdata.IndexOf(": ");
                int k = outdata.IndexOf("/s");

                // save speed
                string hashSpeed = outdata.Substring(i + 2, k - i + 2);
                hashSpeed = hashSpeed.Substring(0, hashSpeed.IndexOf(" "));
                double speed = Double.Parse(hashSpeed, CultureInfo.InvariantCulture);

                //Helpers.ConsolePrint("hashSpeed: " + hashSpeed);

                if (outdata.Contains("Kilohash"))
                    speed *= 1000;
                else if (outdata.Contains("Megahash"))
                    speed *= 1000000;
                
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = speed;

                OnBenchmarkComplete(true, PrintSpeed(speed), BenchmarkTag);
                return true;
            }

            return false;
        }


        virtual protected string BenchmarkGetConsoleOutputLine(Process BenchmarkHandle)
        {
            return BenchmarkHandle.StandardOutput.ReadLine();
        }


        virtual protected Process BenchmarkStartProcess(string CommandLine)
        {
            Helpers.ConsolePrint(MinerDeviceName + " Starting benchmark: " + CommandLine);

            Process BenchmarkHandle = new Process();
            BenchmarkHandle.StartInfo.FileName = Path;
            BenchmarkHandle.StartInfo.Arguments = (string)CommandLine;
            BenchmarkHandle.StartInfo.UseShellExecute = false;
            BenchmarkHandle.StartInfo.RedirectStandardError = true;
            BenchmarkHandle.StartInfo.RedirectStandardOutput = true;
            BenchmarkHandle.StartInfo.CreateNoWindow = true;
            if (!BenchmarkHandle.Start()) return null;

            return BenchmarkHandle;
        }


        virtual protected void BenchmarkThreadRoutine(object CommandLine)
        {
            Thread.Sleep(Config.ConfigData.MinerRestartDelayMS);

            BenchmarkSignalQuit = false;

            Process BenchmarkHandle = null;

            try
            {
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);

                while (true)
                {
                    string outdata = BenchmarkGetConsoleOutputLine(BenchmarkHandle);
                    if (outdata != null)
                    {
                        //Helpers.ConsolePrint(outdata);
                        if (outdata.Contains("Cuda error"))
                            throw new Exception("CUDA error");
                        if (outdata.Contains("is not supported"))
                            throw new Exception("N/A");
                        if (outdata.Contains("illegal memory access"))
                            throw new Exception("CUDA error");
                        if (outdata.Contains("unknown error"))
                            throw new Exception("Unknown error");
                        if (outdata.Contains("No servers could be used! Exiting."))
                            throw new Exception("No pools or work can be used for benchmarking");
                        if (BenchmarkParseLine(outdata))
                            break;
                    }
                    if (BenchmarkSignalQuit)
                        throw new Exception("Termined by user request");
                }
            }
            catch (Exception ex)
            {
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = 0;

                Helpers.ConsolePrint(ex.Message);

                try { if (BenchmarkHandle != null) BenchmarkHandle.Kill(); }
                catch { }
                
                OnBenchmarkComplete(false, "Terminated", BenchmarkTag);
            }

            if (BenchmarkHandle != null)
                BenchmarkHandle.Close();
        }


        virtual protected string GetPassword(Algorithm a)
        {
            if (a.UsePassword != null && a.UsePassword.Length > 0)
                return a.UsePassword;

            if (UsePassword != null && UsePassword.Length > 0)
                return UsePassword;

            return "x";
        }


        virtual protected Process _Start()
        {
            PreviousTotalMH = 0.0;
            if (LastCommandLine.Length == 0 || EnabledDeviceCount() == 0) return null;

            Helpers.ConsolePrint(MinerDeviceName + " Starting miner: " + LastCommandLine);

            Process P = new Process();

            if (WorkingDirectory.Length > 1)
            {
                P.StartInfo.WorkingDirectory = WorkingDirectory;
            }

            P.StartInfo.FileName = Path;
            P.StartInfo.Arguments = LastCommandLine;
            P.StartInfo.CreateNoWindow = Config.ConfigData.HideMiningWindows;
            P.StartInfo.UseShellExecute = !Config.ConfigData.HideMiningWindows;
            P.EnableRaisingEvents = true;
            P.Exited += Miner_Exited;

            try
            {
                if (P.Start()) return P;
                else return null;
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint(ex.Message);
                return null;
            }
        }


        virtual protected void Miner_Exited(object sender, EventArgs e)
        {
            Stop();
        }


        virtual public void Restart()
        {
            Stop(); // stop miner first
            ProcessHandle = _Start(); // start with old command line
        }


        virtual public string PrintSpeed(double spd)
        {
            // print in MH/s
            return (spd * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s";
        }


        public int EnabledDeviceCount()
        {
            int en = 0;

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled) en++;

            return en;
        }
        

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


        protected Algorithm GetMinerAlgorithm(int nhid)
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].NiceHashID == nhid)
                {
                    return SupportedAlgorithms[i];
                }
            }

            return null;
        }


        protected string GetAPIData(int port, string cmd)
        {
            string ResponseFromServer = null;
            try
            {
                TcpClient tcpc = new TcpClient("127.0.0.1", port);
                string DataToSend = "GET /" + cmd + " HTTP/1.1\r\n" +
                                    "Host: 127.0.0.1\r\n" +
                                    "User-Agent: NiceHashMiner/" + Application.ProductVersion + "\r\n" +
                                    "\r\n";

                if (MinerDeviceName == "AMD_OpenCL")
                    DataToSend = cmd;

                byte[] BytesToSend = ASCIIEncoding.ASCII.GetBytes(DataToSend);
                tcpc.Client.Send(BytesToSend);

                byte[] IncomingBuffer = new byte[5000];
                int offset = 0;
                bool fin = false;

                while (!fin && tcpc.Client.Connected)
                {
                    int r = tcpc.Client.Receive(IncomingBuffer, offset, 5000 - offset, SocketFlags.None);
                    for (int i = offset; i < offset + r; i++)
                    {
                        if (IncomingBuffer[i] == 0x7C || IncomingBuffer[i] == 0x00)
                        {
                            fin = true;
                            break;
                        }
                    }
                    offset += r;
                }

                tcpc.Close();

                if (offset > 0)
                    ResponseFromServer = ASCIIEncoding.ASCII.GetString(IncomingBuffer);
            }
            catch
            {
                return null;
            }

            return ResponseFromServer;
        }


        public APIData GetSummary()
        {
            string resp = GetAPIData(APIPort, "summary");
            if (resp == null) return null;

            string aname = null;
            APIData ad = new APIData();

            try
            {
                string[] resps;

                if (MinerDeviceName != "AMD_OpenCL")
                {
                    resps = resp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < resps.Length; i++)
                    {
                        string[] optval = resps[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (optval.Length != 2) continue;
                        if (optval[0] == "ALGO")
                            aname = optval[1];
                        else if (optval[0] == "KHS")
                            ad.Speed = double.Parse(optval[1], CultureInfo.InvariantCulture) * 1000; // HPS
                    }
                }
                else
                {
                    // Checks if all the GPUs are Alive first
                    string resp2 = GetAPIData(APIPort, "devs");
                    if (resp2 == null) return null;

                    string [] checkGPUStatus = resp2.Split(new char [] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 1; i < checkGPUStatus.Length - 1; i++)
                    {
                        if (!checkGPUStatus[i].Contains("Status=Alive"))
                        {
                            Helpers.ConsolePrint("GPU " + i + ": Sick/Dead/NoStart/Initialising/Disabled/Rejecting/Unknown");
                            return null;
                        }
                    }
                    Helpers.ConsolePrint("AMD_OpenCL: All GPUs are alive");

                    resps = resp.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (resps.Length == 3)
                    {
                        string[] data = resps[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Get miner's current total speed
                        string[] speed = data[4].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        // Get miner's current total MH
                        double total_mh = Double.Parse(data[18].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        
                        ad.Speed = Double.Parse(speed[1]) * 1000;

                        aname = SupportedAlgorithms[CurrentAlgo].MinerName;

                        if (total_mh <= PreviousTotalMH)
                        {
                            Helpers.ConsolePrint("AMD_OpenCL: sgminer might be stuck as no new hashes are being produced");
                            Helpers.ConsolePrint("Prev Total MH: " + PreviousTotalMH + " .. Current Total MH: " + total_mh);
                            return null;
                        }

                        PreviousTotalMH = total_mh;
                    }
                    else
                    {
                        ad.Speed = 0;
                    }
                }
            }
            catch
            {
                return null;
            }

            FillAlgorithm(aname, ref ad);

            return ad;
        }


        virtual public int GetMaxProfitIndex(NiceHashSMA[] NiceHashData)
        {
            double MaxProfit = 0;
            int MaxProfitIndex = 0;

            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                SupportedAlgorithms[i].CurrentProfit = SupportedAlgorithms[i].BenchmarkSpeed *
                    NiceHashData[SupportedAlgorithms[i].NiceHashID].paying * 0.000000001;

                Helpers.ConsolePrint(MinerDeviceName + " " + NiceHashData[SupportedAlgorithms[i].NiceHashID].name + " paying " + SupportedAlgorithms[i].CurrentProfit.ToString("F8") + " BTC/Day");

                if (SupportedAlgorithms[i].CurrentProfit > MaxProfit)
                {
                    MaxProfit = SupportedAlgorithms[i].CurrentProfit;
                    MaxProfitIndex = i;
                }
            }

            return MaxProfitIndex;
        }
    }
}
