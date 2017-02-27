using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner {
    // this class mirrors the web profitability, chech what https://www.nicehash.com/?p=calc is using for each algo
    static class ProfitabilityCalculator {
        private const double kHs = 1000;
        private const double MHs = 1000000;
        private const double GHs = 1000000000;
        private const double THs = 1000000000000;

        // divide factor to mirror web values
        readonly static private Dictionary<AlgorithmType, double> _div = new Dictionary<AlgorithmType, double>()
        {
            { AlgorithmType.INVALID , 1 },
            { AlgorithmType.NONE , 1 },
            { AlgorithmType.Scrypt_UNUSED,                  MHs }, // NOT used
            { AlgorithmType.SHA256_UNUSED ,                 THs }, // NOT used
            { AlgorithmType.ScryptNf_UNUSED ,               MHs }, // NOT used
            { AlgorithmType.X11_UNUSED ,                    MHs }, // NOT used
            { AlgorithmType.X13_UNUSED ,                    MHs },
            { AlgorithmType.Keccak_UNUSED ,                 MHs },
            { AlgorithmType.X15_UNUSED ,                    MHs },
            { AlgorithmType.Nist5_UNUSED ,                  MHs },
            { AlgorithmType.NeoScrypt ,                     MHs },
            { AlgorithmType.Lyra2RE ,                       MHs },
            { AlgorithmType.WhirlpoolX_UNUSED ,             MHs },
            { AlgorithmType.Qubit_UNUSED ,                  MHs },
            { AlgorithmType.Quark_UNUSED ,                  MHs },
            { AlgorithmType.Axiom_UNUSED ,                  kHs }, // NOT used
            { AlgorithmType.Lyra2REv2 ,                     MHs },
            { AlgorithmType.ScryptJaneNf16_UNUSED ,         kHs }, // NOT used
            { AlgorithmType.Blake256r8_UNUSED ,             GHs },
            { AlgorithmType.Blake256r14_UNUSED ,            GHs },
            { AlgorithmType.Blake256r8vnl_UNUSED ,          GHs },
            { AlgorithmType.Hodl ,                          kHs },
            { AlgorithmType.DaggerHashimoto ,               MHs },
            { AlgorithmType.Decred ,                        GHs },
            { AlgorithmType.CryptoNight ,                   kHs },
            { AlgorithmType.Lbry ,                          GHs },
            { AlgorithmType.Equihash ,                      1 }, // Sols /s
            { AlgorithmType.Pascal ,                        GHs }, 
            { AlgorithmType.X11Gost ,                       MHs }, 
        };

        public static double GetFormatedSpeed(double speed, AlgorithmType type) {
            if (_div.ContainsKey(type)) {
                return speed / _div[type];
            }
            return speed; // should never happen
        }
    }
}
