using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Enums;

namespace NiceHashMiner
{
    /// <summary>
    /// AlgorithmNiceHashNames class is just a data container for mapping NiceHash JSON API names to algo type
    /// </summary>
    public static class AlgorithmNiceHashNames
    {
        readonly static private Dictionary<AlgorithmType, string> _names = new Dictionary<AlgorithmType, string>()
        {
            { AlgorithmType.INVALID , "INVALID" },
            { AlgorithmType.NONE , "NONE" },
            { AlgorithmType.Scrypt_UNUSED , "scrypt" }, // NOT used
            { AlgorithmType.SHA256_UNUSED , "sha256" }, // NOT used
            { AlgorithmType.ScryptNf_UNUSED , "scryptnf" }, // NOT used
            { AlgorithmType.X11_UNUSED , "x11" }, // NOT used
            { AlgorithmType.X13 , "x13" },
            { AlgorithmType.Keccak , "keccak" },
            { AlgorithmType.X15 , "x15" },
            { AlgorithmType.Nist5 , "nist5" },
            { AlgorithmType.NeoScrypt , "neoscrypt" },
            { AlgorithmType.Lyra2RE , "lyra2re" },
            { AlgorithmType.WhirlpoolX , "whirlpoolx" },
            { AlgorithmType.Qubit , "qubit" },
            { AlgorithmType.Quark , "quark" },
            { AlgorithmType.Axiom_UNUSED , "axiom" }, // NOT used
            { AlgorithmType.Lyra2REv2 , "lyra2rev2" },
            { AlgorithmType.ScryptJaneNf16_UNUSED , "scryptjanenf16" }, // NOT used
            { AlgorithmType.Blake256r8 , "blake256r8" },
            { AlgorithmType.Blake256r14 , "blake256r14" },
            { AlgorithmType.Blake256r8vnl , "blake256r8vnl" },
            { AlgorithmType.Hodl , "hodl" },
            { AlgorithmType.DaggerHashimoto , "daggerhashimoto" },
            { AlgorithmType.Decred , "decred" },
            { AlgorithmType.CryptoNight , "cryptonight" },
            { AlgorithmType.Lbry , "lbry" },
        };

        static private List<AlgorithmType> _keys = null;

        public static string GetName(AlgorithmType type) { return _names[type]; }

        /// <summary>
        /// GetKey is used for Algorithm initialization, from config files.
        /// If algorithm name is invalid it will return INVALID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AlgorithmType GetKey(string name) {
            foreach (var pair in _names) {
                if (pair.Value == name) return pair.Key;
            }

            return AlgorithmType.INVALID;
        }

        public static List<AlgorithmType> GetAllAvaliableTypes() {
            // lazy load
            if (_keys == null) {
                _keys = new List<AlgorithmType>(_names.Keys);
            }
            return _keys;
        }
    }
}
