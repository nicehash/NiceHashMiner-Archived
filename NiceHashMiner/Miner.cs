using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Globalization;

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

        public Algorithm(int id, string nhname, string mname)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            MinerName = mname;
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = "";
            UsePassword = null;
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

    public delegate void BenchmarkComplete(string text, object tag);

    public abstract class Miner
    {
        public string MinerDeviceName;
        public List<ComputeDevice> CDevs;
        public Algorithm[] SupportedAlgorithms;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public int CurrentAlgo;
        public double CurrentRate;

        protected string Path;
        protected int APIPort;
        protected Process ProcessHandle;
        protected Timer BenchmarkTimer;
        protected BenchmarkComplete OnBenchmarkComplete;
        protected object BenchmarkTag;
        protected int BenchmarkIndex;
        protected string LastCommandLine;

        public Miner()
        {
            CDevs = new List<ComputeDevice>();

            ExtraLaunchParameters = "";
            UsePassword = null;

            CurrentAlgo = -1;
            CurrentRate = 0;
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
            }
        }

        abstract protected string BenchmarkCreateCommandLine(int index);


        virtual public void BenchmarkStart(int index, BenchmarkComplete oncomplete, object tag)
        {
            OnBenchmarkComplete = oncomplete;

            if (BenchmarkTimer != null)
            {
                OnBenchmarkComplete("Benchmark running", tag);
                return;
            }

            if (ProcessHandle != null)
            {
                OnBenchmarkComplete("Miner running", tag);
                return; // ignore, already running
            }

            if (index >= SupportedAlgorithms.Length)
            {
                OnBenchmarkComplete("Unknown algorithm", tag);
                return;
            }

            if (EnabledDeviceCount() == 0)
            {
                OnBenchmarkComplete("Disabled", tag);
                return; // ignore, disabled device
            }

            BenchmarkTag = tag;
            BenchmarkIndex = index;

            string CommandLine = BenchmarkCreateCommandLine(index);

            Helpers.ConsolePrint(MinerDeviceName + " Starting benchmark: " + CommandLine);

            ProcessHandle = new Process();
            ProcessHandle.StartInfo.FileName = Path;
            ProcessHandle.StartInfo.Arguments = CommandLine;
            ProcessHandle.StartInfo.UseShellExecute = false;
            ProcessHandle.StartInfo.RedirectStandardError = true;
            ProcessHandle.StartInfo.RedirectStandardOutput = true;
            ProcessHandle.StartInfo.CreateNoWindow = true;
            ProcessHandle.EnableRaisingEvents = true;
            ProcessHandle.Exited += Miner_Exited_Benchmark;
            ProcessHandle.Start();

            BenchmarkTimer = new System.Windows.Forms.Timer();
            BenchmarkTimer.Interval = 100;
            BenchmarkTimer.Tick += BenchmarkTimer_Tick;
            BenchmarkTimer.Start();
        }


        abstract protected void BenchmarkParseLine(string outdata);


        virtual protected void BenchmarkTimer_Tick(object sender, EventArgs e)
        {
            string outdata = ProcessHandle.StandardOutput.ReadLine();
            if (outdata != null)
                BenchmarkParseLine(outdata);
        }


        virtual protected void Miner_Exited_Benchmark(object sender, EventArgs e)
        {
            BenchmarkStop();
            OnBenchmarkComplete("Terminated", BenchmarkTag);
        }


        virtual public void BenchmarkStop()
        {
            if (ProcessHandle != null)
            {
                try { ProcessHandle.Kill(); }
                catch { }
                ProcessHandle.Close();
                ProcessHandle = null;
            }

            if (BenchmarkTimer != null)
            {
                BenchmarkTimer.Stop();
                BenchmarkTimer = null;
            }
        }


        virtual protected string GetPassword(Algorithm a)
        {
            if (a.UsePassword != null && a.UsePassword.Length > 0)
                return a.UsePassword;

            if (UsePassword != null && UsePassword.Length > 0)
                return UsePassword;

            return "x";
        }


        virtual protected void _Start()
        {
            if (LastCommandLine.Length == 0 || EnabledDeviceCount() == 0) return;

            Helpers.ConsolePrint(MinerDeviceName + " Starting miner: " + LastCommandLine);

            ProcessHandle = new Process();
            ProcessHandle.StartInfo.FileName = Path;
            ProcessHandle.StartInfo.Arguments = LastCommandLine;
            ProcessHandle.EnableRaisingEvents = true;
            ProcessHandle.Exited += Miner_Exited;
            ProcessHandle.Start();
        }


        virtual protected void Miner_Exited(object sender, EventArgs e)
        {
            Stop();
        }


        virtual public void Restart()
        {
            Stop(); // stop miner first
            _Start(); // start with old command line
        }


        virtual public string PrintSpeed(double spd)
        {
            // print in MH/s
            return (spd * 0.000001).ToString("F2") + " MH/s";
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

                byte[] BytesToSend = ASCIIEncoding.ASCII.GetBytes(DataToSend);
                tcpc.Client.Send(BytesToSend);

                byte[] IncomingBuffer = new byte[1000];
                int offset = 0;
                bool fin = false;

                while (!fin && tcpc.Client.Connected)
                {
                    int r = tcpc.Client.Receive(IncomingBuffer, offset, 1000 - offset, SocketFlags.None);
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
                string[] resps = resp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
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
