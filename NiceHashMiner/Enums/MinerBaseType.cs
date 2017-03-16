using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums {

    /// <summary>
    /// Do not delete obsolete enums! Always add new ones before the END enum.
    /// </summary>
    public enum MinerBaseType {
        NONE = 0,
        cpuminer,
        ccminer,
        sgminer,
        nheqminer,
        eqm,
        ethminer,
        ClaymoreAMD,
        OptiminerAMD,
        excavator,
        XmrStackCPU,
        ccminer_alexis,
        experimental,
        END
    }
}
