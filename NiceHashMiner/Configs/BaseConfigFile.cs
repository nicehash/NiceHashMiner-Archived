using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Configs {
    [Serializable]
    public abstract class BaseConfigFile<T> where T : class {
        #region Members
        [field: NonSerialized]
        readonly public static string CONF_FOLDER = @"configs\";

        private string _filePath = "";
        private string _filePathOld = "";
        protected string FilePath {
            get { return CONF_FOLDER + _filePath; }
            set { _filePath = value; }
        }
        protected string FilePathOld {
            get { return CONF_FOLDER + _filePathOld; }
            set { _filePathOld = value; }
        }

        [JsonIgnore]
        public T FileLoaded { get { return _self; } }

        [field: NonSerialized]
        protected T _self;

        #endregion //Members

        public void InitializeConfig() {
            InitializePaths();
            ReadFile();
            if (_self != null) {
                InitializeObject();
            }
        }

        abstract protected void InitializePaths();
        abstract protected void InitializeObject();

        private static void CheckAndCreateConfigsFolder () {
            try {
                if (Directory.Exists(CONF_FOLDER) == false) {
                    Directory.CreateDirectory(CONF_FOLDER);
                }
            } catch { }
        }

        protected void ReadFile() {
            CheckAndCreateConfigsFolder();
            try {
                if (new FileInfo(FilePath).Exists) {
                    _self = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));
                } else {
                    Commit();
                }
            } catch(Exception ex) {
                Helpers.ConsolePrint("BaseConfigFile", "ReadFile exception " + ex.ToString() );
            }
        }

        public void Commit() {
            try { File.WriteAllText(FilePath, JsonConvert.SerializeObject(this, Formatting.Indented)); }
            catch { }
        }

        public bool ConfigFileExist() {
            return File.Exists(FilePath);
        }
    }
}
