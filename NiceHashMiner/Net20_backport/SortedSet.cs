using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Net20_backport {
    public class SortedSet<T> : List<T> {
        public new void Add(T item) {
            if (this.Contains(item) == false) {
                base.Add(item);
                base.Sort();
            }
        }
    }
}
