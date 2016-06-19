using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    class International
    {
        private class Language
        {
#pragma warning disable 649
            public string Name;
            public int ID;
            public Dictionary<string, string> Entries;
#pragma warning restore 649
        }

        private static Language SelectedLanguage;

        private static List<Language> GetLanguages()
        {
            List<Language> langs = new List<Language>();
            DirectoryInfo di = new DirectoryInfo("langs");
            FileInfo[] files = di.GetFiles("*.lang");

            foreach (FileInfo fi in files)
            {
                try
                {
                    Language l = JsonConvert.DeserializeObject<Language>(File.ReadAllText(fi.FullName));
                    langs.Add(l);
                }
                catch (Exception ex)
                {
                    Helpers.ConsolePrint("NICEHASH", "Lang error: " + ex.Message);
                }
            }

            return langs;
        }

        public static void Initialize(int lid)
        {
            List<Language> langs = GetLanguages();

            foreach (Language lang in langs)
            {
                if (lang.ID == lid)
                {
                    Helpers.ConsolePrint("NICEHASH", "Selected language: " + lang.Name);
                    SelectedLanguage = lang;
                    return;
                }
            }
            
            Helpers.ConsolePrint("NICEHASH", "Critical error: missing language");
        }

        /// <summary>
        /// Call this method to obtain available languages. Used by Settings GUI.
        /// </summary>
        /// <returns>Each dictionary entry contains id of the language (int) and name of the language (string).</returns>
        public static Dictionary<int, string> GetAvailableLanguages()
        {
            List<Language> langs = GetLanguages();
            Dictionary<int, string> retdict = new Dictionary<int,string>();

            foreach (Language lang in langs)
            {
                Helpers.ConsolePrint("NICEHASH", "Found language: " + lang.Name);
                retdict.Add(lang.ID, lang.Name);
            }

            return retdict;
        }

        public static string GetText(string token)
        {
            if (SelectedLanguage.Entries.ContainsKey(token))
                return SelectedLanguage.Entries[token];
            else
                return "";
        }
    }
}
