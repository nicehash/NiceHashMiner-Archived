using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace NiceHashMiner.Miners {
    public static class MinersApiPortsManager {
        private static HashSet<int> _usedPorts = new HashSet<int>();

        public static bool IsPortAvaliable(int port) {
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

        public static int GetAvaliablePort() {
            int port = ConfigManager.GeneralConfig.ApiBindPortPoolStart;
            int newPortEnd = port + 3000;
            for (; port < newPortEnd; ++port) {
                if (MinersSettingsManager.AllReservedPorts.Contains(port) == false && IsPortAvaliable(port) && _usedPorts.Add(port)) {
                    break;
                }
            }
            return port;
        }

        public static void RemovePort(int port) {
            _usedPorts.Remove(port);
        }
    }
}
