using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums
{
    /// <summary>
    /// AlgorithmType enum should/must mirror the values from https://www.nicehash.com/?p=api
    /// </summary>
    public enum AlgorithmType : int
    {
        NONE = -1,
        #region NiceHashAPI
        Scrypt = 0,
        SHA256 = 1,
        ScryptNf = 2,
        X11 = 3,
        X13 = 4,
        Keccak = 5,
        X15 = 6,
        Nist5 = 7,
        NeoScrypt = 8,
        Lyra2RE = 9,
        WhirlpoolX = 10,
        Qubit = 11,
        Quark = 12,
        Axiom = 13,
        Lyra2REv2 = 14,
        ScryptJaneNf16 = 15,
        Blake256r8 = 16,
        Blake256r14 = 17,
        Blake256r8vnl = 18,
        Hodl = 19,
        DaggerHashimoto = 20,
        Decred = 21,
        #endregion // NiceHashAPI
        // ethereum not documented but it is returned from API value is 999
        Ethereum = 999
    }
}
