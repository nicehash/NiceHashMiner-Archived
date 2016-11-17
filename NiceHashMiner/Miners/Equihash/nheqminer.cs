using Newtonsoft.Json;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class nheqminer : nheqBase {
        public nheqminer()
            : base(DeviceType.ALL, "nheqminer") {
                Path = MinerPaths.nheqminer;
                WorkingDirectory = MinerPaths.nheqminer.Replace("nheqminer.exe", "");
        }

        // CPU aff set from NHM
        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();
            if (CPUs.Count > 0 && CPUs[0].AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, CPUs[0].AffinityMask);

            return P;
        }

        public override void Start(Algorithm miningAlgorithm, string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            CurrentMiningAlgorithm = miningAlgorithm;
            LastCommandLine = GetDevicesCommandString() + " -a " + APIPort + " -l " + url + " -u " + username;
            ProcessHandle = _Start();
        }


        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            if (CPUs.Count > 0) {
                if (CPUs[0].MostProfitableAlgorithm.LessThreads > 0 || !string.IsNullOrEmpty(CPUs[0].MostProfitableAlgorithm.ExtraLaunchParameters)) {
                    // TODO parse
                    deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(CPUs, AlgorithmType.Equihash, DeviceType.CPU, Path);
                }
            } else {
                // disable CPU
                deviceStringCommand += " -t 0 ";
            }

            if (NVIDIAs.Count > 0) {
                deviceStringCommand += " -cd ";
                foreach (var nvidia in NVIDIAs) {
                    deviceStringCommand += nvidia.ID + " ";
                }
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(NVIDIAs, AlgorithmType.Equihash, DeviceType.NVIDIA, Path);
            }

            if (AMDs.Count > 0) {
                deviceStringCommand += " -op " + AMD_OCL_PLATFORM.ToString();
                deviceStringCommand += " -od ";
                foreach (var amd in AMDs) {
                    deviceStringCommand += amd.ID + " ";
                }
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(AMDs, AlgorithmType.Equihash, DeviceType.AMD, Path);
            }

            return deviceStringCommand;
        }

        // benchmark stuff
        protected override bool BenchmarkParseLine(string outdata) {

            if (outdata.Contains(Iter_PER_SEC)) {
                curSpeed = getNumber(outdata, "Speed: ", Iter_PER_SEC) * SolMultFactor;
            }
            if (outdata.Contains(Sols_PER_SEC)) {
                var sols = getNumber(outdata, "Speed: ", Sols_PER_SEC);
                if (sols > 0) {
                    BenchmarkAlgorithm.BenchmarkSpeed = curSpeed;
                    return true;
                }
            }
            return false;
        }
        
    }
}
