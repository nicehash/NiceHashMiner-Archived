using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class MinersApiPortsManager : BaseLazySingleton<MinersApiPortsManager> {
        private HashSet<int> _usedPorts;
        
        protected MinersApiPortsManager() {
            _usedPorts = new HashSet<int>();
        }

        private bool IsPortAvaliable(int port) {
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            // check TCP
            {
                var tcpIpEndpoints = ipGlobalProperties.GetActiveTcpListeners();
                foreach (var tcp in tcpIpEndpoints) {
                    if (tcp.Port == port) {
                        isAvailable = false;
                        break;
                    }
                }
            }
            // check UDP
            if (isAvailable) {
                var udpIpEndpoints = ipGlobalProperties.GetActiveUdpListeners();
                foreach (var udp in udpIpEndpoints) {
                    if (udp.Port == port) {
                        isAvailable = false;
                        break;
                    }
                }
            }
            return isAvailable;
        }

        public int GetAvaliablePort() {
            int port = ConfigManager.Instance.GeneralConfig.ApiBindPortPoolStart;
            int newPortEnd = port + 3000;
            for (; port < newPortEnd; ++port) {
                if (IsPortAvaliable(port) && _usedPorts.Add(port)) {
                    break;
                }
            }
            return port;
        }

        public void RemovePort(int port) {
            _usedPorts.Remove(port);
        }
    }
}
