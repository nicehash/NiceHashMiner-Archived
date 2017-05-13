using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Net20_backport {
    public class Tuple3<T1, T2, T3> {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }

        public Tuple3(T1 i1, T2 i2, T3 i3) {
            Item1 = i1;
            Item2 = i2;
            Item3 = i3;
        }
    }
}
