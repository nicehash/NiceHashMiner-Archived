using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiceHashMiner {
    public abstract class BaseLazySingleton<T> where T : class {
        private static readonly Lazy<T> LazyInstance =
            new Lazy<T>(CreateInstanceOfT, LazyThreadSafetyMode.ExecutionAndPublication);

        #region Properties
        public static T Instance {
            get { return LazyInstance.Value; }
        }
        #endregion

        #region Methods
        private static T CreateInstanceOfT() {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        protected BaseLazySingleton() { }

        #endregion
    }
}
