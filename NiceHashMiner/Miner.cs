using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace NiceHashMiner
{
    class APIData
    {
        public int AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }

    class ComputeDevice
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

    abstract class Miner
    {
        public List<ComputeDevice> CDevs;

        protected Algorithm[] SupportedAlgorithms;
        protected string Path;
        protected int APIPort;
        protected Process ProcessHandle;

        public Miner()
        {
            CDevs = new List<ComputeDevice>();
        }

        abstract public APIData GetSummary();

        abstract public void Start(int nhalgo, string url, string username);

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

        protected string GetMinerAlgorithmName(int nhid)
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].NiceHashID == nhid)
                {
                    return SupportedAlgorithms[i].MinerName;
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
    }
}
