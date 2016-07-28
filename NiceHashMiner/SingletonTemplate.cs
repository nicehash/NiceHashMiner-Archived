using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner {
    /// <summary>
    /// SingletonTemplate template is a generic for a thread safe singleton pattern.
    /// Should be thread safe (link=https://msdn.microsoft.com/en-us/library/ff650316.aspx)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class SingletonTemplate<T> where T : class, new() {
        protected static volatile T instance;
        protected static object syncRoot = new Object();

        protected SingletonTemplate() { }

        public static T Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null)
                            instance = new T();
                    }
                }
                return instance;
            }
        }

    }
}
