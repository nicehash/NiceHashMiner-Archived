using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Enums {
    public enum NHMConectionType {
        NONE,
        STRATUM_TCP,
        STRATUM_SSL,
        LOCKED // inhouse miners that are locked on NH (our eqm)
    }
}
