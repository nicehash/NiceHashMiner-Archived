using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NiceHashMiner
{
    public class ethminerAPI
    {
        /// <summary>
        /// Initialize ethminer API instance.
        /// </summary>
        /// <param name="port">ethminer's API port.</param>
        public ethminerAPI(int port)
        {
            m_port = port;
            m_client = new UdpClient("127.0.0.1", port);
        }

        /// <summary>
        /// Call this to start ethminer. If ethminer is already running, nothing happens.
        /// </summary>
        public void StartMining()
        {
            SendUDP(2);
        }

        /// <summary>
        /// Call this to stop ethminer. If ethminer is already stopped, nothing happens.
        /// </summary>
        public void StopMining()
        {
            SendUDP(1);
        }

        /// <summary>
        /// Call this to get current ethminer speed. This method may block up to 2 seconds.
        /// </summary>
        /// <param name="ismining">Set to true if ethminer is not mining (has been stopped).</param>
        /// <param name="speed">Current ethminer speed in MH/s.</param>
        /// <returns>False if ethminer is unreachable (crashed or unresponsive and needs restarting).</returns>
        public bool GetSpeed(out bool ismining, out double speed)
        {
            speed = 0;
            ismining = false;

            SendUDP(3);

            DateTime start = DateTime.Now;

            while ((DateTime.Now - start) < TimeSpan.FromMilliseconds(2000))
            {
                if (m_client.Available > 0)
                {
                    // read
                    try
                    {
                        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_port); 
                        byte[] data = m_client.Receive(ref ipep);
                        if (data.Length != 8) return false;
                        speed = BitConverter.ToDouble(data, 0);
                        if (speed >= 0) ismining = true;
                        else speed = 0;
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    System.Threading.Thread.Sleep(2);
            }

            return false;
        }

        #region PRIVATE

        private int m_port;
        private UdpClient m_client;

        private void SendUDP(int code)
        {
            byte[] data = new byte[1];
            data[0] = (byte)code;
            m_client.Send(data, data.Length);
        }
        #endregion
    }
}
