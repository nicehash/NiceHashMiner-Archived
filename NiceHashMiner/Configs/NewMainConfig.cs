using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceHashMiner.Configs
{
    public sealed class NewMainConfig
    {
        #region SINGLETON Stuff
        private static NewMainConfig _instance = new NewMainConfig();

        public static NewMainConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NewMainConfig();
                }
                return _instance;
            }
        }
        #endregion

    }
}
