using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceHashMiner.Interfaces;

namespace NiceHashMiner.Configs {
    [Serializable]
    public abstract class BaseConfigFile<T> : IPathsProperties where T : class, IPathsProperties {
        #region Members
        [field: NonSerialized]
        readonly public static string CONF_FOLDER = @"configs\";

        private string _filePath = "";
        private string _filePathOld = "";
        public string FilePath {
            get { return _filePath.Contains(CONF_FOLDER) ? _filePath : CONF_FOLDER + _filePath; }
            set { _filePath = value; }
        }
        public string FilePathOld {
            get { return _filePathOld.Contains(CONF_FOLDER) ? _filePathOld : CONF_FOLDER + _filePathOld; }
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
                _self.FilePath = this.FilePath;
                _self.FilePathOld = this.FilePathOld;
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
                Helpers.ConsolePrint("BaseConfigFile", String.Format("ReadFile {0}: exception {1}", FilePath, ex.ToString()));
            }
        }

        public void Commit() {
            try {
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception ex) {
                Helpers.ConsolePrint("BaseConfigFile", String.Format("Commit {0}: exception {1}", FilePath, ex.ToString()));
            }
        }

        public bool ConfigFileExist() {
            return File.Exists(FilePath);
        }


    }
}
