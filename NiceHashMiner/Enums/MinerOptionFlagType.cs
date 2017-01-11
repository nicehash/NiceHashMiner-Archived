using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums {
    // indicates if uni flag (no parameter), single param or multi param
    public enum MinerOptionFlagType {
        Uni,
        SingleParam,
        MultiParam,
        DuplicateMultiParam // the flag is multiplied
    }
}
