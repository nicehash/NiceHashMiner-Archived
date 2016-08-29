using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Enums {
    
    /// <summary>
    /// This is used for ethminers DAG generation mode
    /// </summary>
    public enum DagGenerationType : int {
        SingleKeep = 0,
        Single,
        Sequential,
        Parallel,
        END
    }
}
