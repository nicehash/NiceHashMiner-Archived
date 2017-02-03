using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceHashMiner.Configs.ConfigJsonFile {
    public abstract class ConfigFile<T> where T : class {
        // statics/consts
        const string TAG_FORMAT = "ConfigFile<{0}>";
        private readonly string CONF_FOLDER; // = @"configs\";
        private readonly string TAG;

        private void CheckAndCreateConfigsFolder() {
            try {
                if (Directory.Exists(CONF_FOLDER) == false) {
                    Directory.CreateDirectory(CONF_FOLDER);
                }
            } catch { }
        }

        // member stuff
        protected string _filePath = "";
        protected string _filePathOld = "";

        public ConfigFile(string iCONF_FOLDER, string fileName, string fileNameOld) {
            CONF_FOLDER = iCONF_FOLDER;
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
            TAG = String.Format(TAG_FORMAT, typeof(T).Name);
        }

        public bool IsFileExists() {
            return File.Exists(_filePath);
        }

        public T ReadFile() {
            CheckAndCreateConfigsFolder();
            T file = null;
            try {
                if (File.Exists(_filePath)) {
                    file = JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePath), Globals.JsonSettings);
                }
            } catch (Exception ex) {
                Helpers.ConsolePrint(TAG, String.Format("ReadFile {0}: exception {1}", _filePath, ex.ToString()));
                file = null;
            }
            return file;
        }

        public void Commit(T file) {
            CheckAndCreateConfigsFolder();
            if (file == null) {
                Helpers.ConsolePrint(TAG, String.Format("Commit for FILE {0} IGNORED. Passed null object", _filePath));
                return;
            }
            try {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(file, Formatting.Indented));
            } catch (Exception ex) {
                Helpers.ConsolePrint(TAG, String.Format("Commit {0}: exception {1}", _filePath, ex.ToString()));
            }
        }

        public void CreateBackup() {
            Helpers.ConsolePrint(TAG, String.Format("Backing up {0} to {1}..", _filePath, _filePathOld));
            try {
                if (File.Exists(_filePathOld))
                    File.Delete(_filePathOld);
                File.Copy(_filePath, _filePathOld, true);
            } catch { }
        }
    }
}
