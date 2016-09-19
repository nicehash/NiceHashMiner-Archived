using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            { AlgorithmType.Scrypt_UNUSED,          MHs }, // NOT used
            { AlgorithmType.SHA256_UNUSED ,         THs }, // NOT used
            { AlgorithmType.ScryptNf_UNUSED ,       MHs }, // NOT used
            { AlgorithmType.X11_UNUSED ,            MHs }, // NOT used
            { AlgorithmType.X13 ,                   MHs },
            { AlgorithmType.Keccak ,                MHs },
            { AlgorithmType.X15 ,                   MHs },
            { AlgorithmType.Nist5 ,                 MHs },
            { AlgorithmType.NeoScrypt ,             MHs },
            { AlgorithmType.Lyra2RE ,               MHs },
            { AlgorithmType.WhirlpoolX ,            MHs },
            { AlgorithmType.Qubit ,                 MHs },
            { AlgorithmType.Quark ,                 MHs },
            { AlgorithmType.Axiom_UNUSED ,          kHs }, // NOT used
            { AlgorithmType.Lyra2REv2 ,             MHs },
            { AlgorithmType.ScryptJaneNf16_UNUSED , kHs }, // NOT used
            { AlgorithmType.Blake256r8 ,            GHs },
            { AlgorithmType.Blake256r14 ,           GHs },
            { AlgorithmType.Blake256r8vnl ,         GHs },
            { AlgorithmType.Hodl ,                  kHs },
            { AlgorithmType.DaggerHashimoto ,       MHs },
            { AlgorithmType.Decred ,                GHs },
            { AlgorithmType.CryptoNight ,           kHs },
            { AlgorithmType.Lbry ,                  GHs },
        };

        public static double GetFormatedSpeed(double speed, AlgorithmType type) {
            if (_div.ContainsKey(type)) {
                return speed / _div[type];
            }
            return speed; // should never happen
        }
    }
}
