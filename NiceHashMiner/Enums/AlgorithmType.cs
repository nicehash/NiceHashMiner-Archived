using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums
{
    /// <summary>
    /// AlgorithmType enum should/must mirror the values from https://www.nicehash.com/?p=api
    /// Some algorithms are not used anymore on the client, rename them with _UNUSED postfix so we can catch compile time errors if they are used.
    /// </summary>
    public enum AlgorithmType : int
    {
        INVALID = -2,
        NONE = -1,
        #region NiceHashAPI
        Scrypt_UNUSED = 0,
        SHA256_UNUSED = 1,
        ScryptNf_UNUSED = 2,
        X11_UNUSED = 3,
        X13 = 4,
        Keccak = 5,
        X15 = 6,
        Nist5 = 7,
        NeoScrypt = 8,
        Lyra2RE = 9,
        WhirlpoolX = 10,
        Qubit = 11,
        Quark = 12,
        Axiom_UNUSED = 13,
        Lyra2REv2 = 14,
        ScryptJaneNf16_UNUSED = 15,
        Blake256r8 = 16,
        Blake256r14 = 17,
        Blake256r8vnl = 18,
        Hodl = 19,
        DaggerHashimoto = 20,
        Decred = 21,
        CryptoNight = 22,
        Lbry = 23
        #endregion // NiceHashAPI
    }
}
