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
        static int seconds = 60;
        public static int SMAMinerCheckInterval = seconds * 1000; // 30s
        public static bool ForcePerCardMiners = false;


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

                foreach (var cDev in ComputeDevice.AllAvaliableDevices) {
                    _curStepCheck[cDev.UUID] = 0;
                    //_allAvaliableAlgoKeys[cDev.Name] = new List<AlgorithmType>(cDev.DeviceBenchmarkConfig.AlgorithmSettings.Keys);
                    _allAvaliableAlgoKeys[cDev.UUID] = new List<AlgorithmType>(new[] { AlgorithmType.DaggerHashimoto, AlgorithmType.Lyra2RE });
                }
            }

            public void SetNext(ref PerDeviceProifitDictionary devProfits, List<ComputeDevice> enabledDevices) {

                foreach (var cDev in enabledDevices) {
                    var devUUID = cDev.UUID;
                    var curStepCheckIndex = _curStepCheck[devUUID];
                    var _mostProfitKey = _allAvaliableAlgoKeys[devUUID][curStepCheckIndex];
                    var mostProfitKeyName = AlgorithmNiceHashNames.GetName(_mostProfitKey);
                    Helpers.ConsolePrint(TAG, String.Format("Setting most MostProfitKey to {0}", mostProfitKeyName));

                    // set new most profit
                    Helpers.ConsolePrint(TAG, String.Format("Setting device {0} to {1}", devUUID, mostProfitKeyName));
                    devProfits[devUUID][_mostProfitKey] = MOST_PROFIT_REPLACE_VAL;

                    ++curStepCheckIndex;
                    if (curStepCheckIndex >= _allAvaliableAlgoKeys[devUUID].Count) curStepCheckIndex = 0;
                    _curStepCheck[devUUID] = curStepCheckIndex;
                }

                
            }

        }
    }
}
#endif