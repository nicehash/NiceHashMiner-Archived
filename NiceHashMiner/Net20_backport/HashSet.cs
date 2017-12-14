using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Net20_backport {
    public class HashSet<T> : List<T> {

        public new bool Add(T item) {
            if (this.Contains(item) == false) {
                base.Add(item);
                return true;
            }
            return false;
        }

        public T First() {
            if(this.Count >= 1) {
                return this[0];
            }
            throw new IndexOutOfRangeException();
        }
    }
}
