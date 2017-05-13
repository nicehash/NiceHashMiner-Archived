using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Net20_backport {
    public class Tuple<T1, T2> {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Tuple(T1 i1, T2 i2) {
            Item1 = i1;
            Item2 = i2;
        }
    }
}
