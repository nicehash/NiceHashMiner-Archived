using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class MinersApiPortsManager : BaseLazySingleton<MinersApiPortsManager> {
        private const int _base_START = 4000;
        private const int _base_JUMP = 100;

        private HashSet<int> _usedPorts;
        
        protected MinersApiPortsManager() {
            _usedPorts = new HashSet<int>();
        }

        private int GetStartPort(int minerTypeInt) {
            return _base_START + (minerTypeInt + 1) * _base_JUMP;
        }

        public int GetAvaliablePort(MinerType minerType) {
            int newPortStart = GetStartPort((int)minerType);
            int newPortEnd = GetStartPort((int)minerType + 1);
            int port = newPortStart;
            for (; port < newPortEnd; ++port) {
                if (_usedPorts.Add(port)) {
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
