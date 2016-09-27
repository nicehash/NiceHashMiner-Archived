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

        [JsonIgnore]
        public string FilePath {
            get { return _filePath.Contains(CONF_FOLDER) ? _filePath : CONF_FOLDER + _filePath; }
            set { _filePath = value; }
        }
        [JsonIgnore]
        public string FilePathOld {
            get { return _filePathOld.Contains(CONF_FOLDER) ? _filePathOld : CONF_FOLDER + _filePathOld; }
            set { _filePathOld = value; }
        }

        [JsonIgnore]
        protected bool FileLoaded { get { return _file != null; } }

        [field: NonSerialized]
        protected T _file;

        #endregion //Members

        public void InitializeConfig() {
            InitializePaths();
            ReadFile();
            if (FileLoaded) {
                _file.FilePath = this.FilePath;
                _file.FilePathOld = this.FilePathOld;
                InitializeObject();
            }
        }

        /// <summary>
        /// InitializePaths should be overrided in the subclass to specify filepath(old) paths.
        /// </summary>
        abstract protected void InitializePaths();
        /// <summary>
        /// InitializeObject must be overrided in the subclass to reinitialize values and references from the configuration files.
        /// Use the _self member and reinitialize all non null references (use DeepCopy or plain reference it is up to the implementor).
        /// IMPORTANT!!! Take extra care with arrays, lists and dictionaries, initialize them manually and not by DeepCopy or reference, the 
        /// reason for this is to be future proof if new keys/values are added so reinitialize them one by one if they exist.
        /// </summary>
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
                    _file = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath), Globals.JsonSettings);
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
    }
}
