using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class eqm : nheqBase {
        public eqm()
            : base(DeviceType.NVIDIA_CPU, "eqm") {
            Path = MinerPaths.eqm;
            WorkingDirectory = MinerPaths.eqm.Replace("eqm.exe", "");
        }


        // TODO fix extra paramteter parsing
        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            if (CPUs.Count > 0) {
                if (CPUs[0].MostProfitableAlgorithm.LessThreads > 0 || !string.IsNullOrEmpty(CPUs[0].MostProfitableAlgorithm.ExtraLaunchParameters)) {
                    // TODO check parsing for dual CPU
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

            return deviceStringCommand;
        }

        // benchmark stuff
        protected override bool BenchmarkParseLine(string outdata) {

            if (outdata.Contains(Iter_PER_SEC)) {
                curSpeed = getNumber(outdata, "Measured: ", Iter_PER_SEC) * SolMultFactor;
            }
            if (outdata.Contains(Sols_PER_SEC)) {
                var sols = getNumber(outdata, "Measured: ", Sols_PER_SEC);
                if (sols > 0) {
                    BenchmarkAlgorithm.BenchmarkSpeed = curSpeed;
                    return true;
                }
            }
            return false;
        }
    }
}
