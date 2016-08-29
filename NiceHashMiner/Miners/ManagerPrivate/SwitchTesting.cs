using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#if (SWITCH_TESTING)
namespace NiceHashMiner.Miners {
    using NiceHashMiner.Devices;
    using PerDeviceProifitDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;
    public partial class MinersManager {
        // globals testing variables
        static int seconds = 15;
        public static int SMAMinerCheckInterval = seconds * 1000; // 30s
        public static bool ForcePerCardMiners = true;


        private class SwitchTesting : BaseLazySingleton<SwitchTesting> {

            readonly string TAG;
            // dev names
            Dictionary<string, int> _curStepCheck;
            Dictionary<string, List<AlgorithmType>> _allAvaliableAlgoKeys;
            const double MOST_PROFIT_REPLACE_VAL = 500000.0d;

            protected SwitchTesting() {
                TAG = this.GetType().Name;

                _curStepCheck = new Dictionary<string, int>();
                _allAvaliableAlgoKeys = new Dictionary<string, List<AlgorithmType>>();

                foreach (var cDev in ComputeDevice.UniqueAvaliableDevices) {
                    _curStepCheck[cDev.Name] = 0;
                    _allAvaliableAlgoKeys[cDev.Name] = new List<AlgorithmType>(cDev.DeviceBenchmarkConfig.AlgorithmSettings.Keys);
                    //_allAvaliableAlgoKeys[cDev.Name] = new List<AlgorithmType>(new []{AlgorithmType.DaggerHashimoto, AlgorithmType.CryptoNight, AlgorithmType.Lyra2RE, AlgorithmType.Lyra2REv2});
                }

                if (MinersManager.ForcePerCardMiners) {
                    int repat = 0;
                    foreach (var cDev in ComputeDevice.UniqueAvaliableDevices) {
                        var pass = new String('x', ++repat);
                        foreach (var algo in cDev.DeviceBenchmarkConfig.AlgorithmSettings) {
                            algo.Value.UsePassword = pass; 
                        }
                    }
                }
            }

            public void SetNext(ref PerDeviceProifitDictionary devProfits, List<ComputeDevice> enabledDevices) {

                foreach (var cDev in enabledDevices) {
                    var devName = cDev.Name;
                    var curStepCheckIndex = _curStepCheck[devName];
                    var _mostProfitKey = _allAvaliableAlgoKeys[devName][curStepCheckIndex];
                    var mostProfitKeyName = AlgorithmNiceHashNames.GetName(_mostProfitKey);
                    Helpers.ConsolePrint(TAG, String.Format("Setting most MostProfitKey to {0}", mostProfitKeyName));

                    // set new most profit
                    Helpers.ConsolePrint(TAG, String.Format("Setting device {0} to {1}", devName, mostProfitKeyName));
                    devProfits[devName][_mostProfitKey] = MOST_PROFIT_REPLACE_VAL;

                    ++curStepCheckIndex;
                    if (curStepCheckIndex >= _allAvaliableAlgoKeys[devName].Count) curStepCheckIndex = 0;
                    _curStepCheck[devName] = curStepCheckIndex;
                }

                
            }

        }
    }
}
#endif