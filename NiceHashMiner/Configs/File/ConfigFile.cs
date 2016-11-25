using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Configs {
    public abstract class ConfigFile<T> where T : class {
        #region statics/consts
        // statics/consts
        const string TAG = "ConfigFile<T>";
        const string CONF_FOLDER = @"configs\";

        private static void CheckAndCreateConfigsFolder() {
            try {
                if (Directory.Exists(CONF_FOLDER) == false) {
                    Directory.CreateDirectory(CONF_FOLDER);
                }
            } catch { }
        }
        #endregion statics/consts

        protected string _filePath = "";
        protected string _filePathOld = "";

        protected T _file = null;

        public ConfigFile(string fileName, string fileNameOld) {
            if(fileName.Contains(CONF_FOLDER)) {
                this._filePath = fileName;
            } else {
                this._filePath = CONF_FOLDER + fileName;
            }
            if (fileNameOld.Contains(CONF_FOLDER)) {
                this._filePathOld = fileNameOld;
            } else {
                this._filePathOld = CONF_FOLDER + fileNameOld;
            }
        }

        protected void ReadFile() {
            CheckAndCreateConfigsFolder();
            try {
                if (new FileInfo(_filePath).Exists) {
                    _file = JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePath), Globals.JsonSettings);
                } else {
                    Commit();
                }
            } catch (Exception ex) {
                Helpers.ConsolePrint(TAG, String.Format("ReadFile {0}: exception {1}", _filePath, ex.ToString()));
            }
        }

        public void Commit() {
            try {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(_file, Formatting.Indented));
            } catch (Exception ex) {
                Helpers.ConsolePrint(TAG, String.Format("Commit {0}: exception {1}", _filePath, ex.ToString()));
            }
        }

        public void CreateBackup() {
            Helpers.ConsolePrint(TAG, String.Format("Backing up {0} to {1}..", _filePath, _filePathOld));
            try {
                if (File.Exists(_filePathOld))
                    File.Delete(_filePathOld);
                File.Move(_filePath, _filePathOld);
            } catch { }
        }
    }
}
