using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;

namespace NiceHashMiner
{
    class ccminer : Miner
    {
        public ccminer()
        {
            Path = "bin\\ccminer.exe";
            APIPort = 4048;

            SupportedAlgorithms = new Algorithm[] { 
                new Algorithm(0, "scrypt", "scrypt"),
                new Algorithm(12, "quark", "quark")
            };

            QueryGPUs();
        }


        public override void Start(string suburl, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running 

            //string CommandLine = "--url=sumplemultialgo+NiceHash+" + suburl + " --userpass=" + username + ":x --api-bind=" + APIPort.ToString() + " --devices ";
            string CommandLine = "--algo=quark --url=stratum+tcp://quark." + suburl + ".nicehash.com:3345 --userpass=" + username + ":x --api-bind=" + APIPort.ToString() + " --devices ";

            foreach (GPUData G in GPUs)
                if (G.Enabled)
                    CommandLine += G.ID.ToString() + ",";

            if (CommandLine.EndsWith(","))
            {
                CommandLine = CommandLine.Remove(CommandLine.Length - 1);
            }
            else
                return; // no GPUs to start mining on

            ProcessHandle = Process.Start(Path, CommandLine);
            ProcessHandle.EnableRaisingEvents = true;
            ProcessHandle.Exited += Miner_Exited;
        }


        private void Miner_Exited(object sender, EventArgs e)
        {
            Stop();
        }


        protected override void QueryGPUs()
        {
            GPUs = new List<GPUData>();

            Process P = new Process();
            P.StartInfo.FileName = Path;
            P.StartInfo.Arguments = "--ndevs";
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.CreateNoWindow = true;
            P.Start();

            string outdata;

            do
            {
                outdata = P.StandardError.ReadLine();
                if (outdata != null)
                    GPUs.Add(new GPUData(GPUs.Count, outdata.Split(':')[1]));
            } while (outdata != null);

            P.WaitForExit();
        }


        public override APIData GetSummary()
        {
            string resp = GetAPIDataccminer(APIPort, "summary");
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


        private string GetAPIDataccminer(int port, string cmd)
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
