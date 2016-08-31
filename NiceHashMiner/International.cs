using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using NiceHashMiner.Enums;

namespace NiceHashMiner
{
    class International
    {
        private class Language
        {
#pragma warning disable 649
            public string Name;
            public LanguageType ID;
            public Dictionary<string, string> Entries;
#pragma warning restore 649
        }

        private static Language SelectedLanguage;

        private static List<Language> GetLanguages()
        {
            List<Language> langs = new List<Language>();

            try
            {
                DirectoryInfo di = new DirectoryInfo("langs");
                FileInfo[] files = di.GetFiles("*.lang");

                foreach (FileInfo fi in files)
                {
                    try
                    {
                        Language l = JsonConvert.DeserializeObject<Language>(File.ReadAllText(fi.FullName)); // TODO , Globals.JsonSettings not sure since data must be localized
                        langs.Add(l);
                    }
                    catch (Exception ex)
                    {
                        Helpers.ConsolePrint("NICEHASH", "Lang error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint("NICEHASH", "Lang error: " + ex.Message);
            }

            return langs;
        }

        public static void Initialize(LanguageType lid)
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
        public static Dictionary<LanguageType, string> GetAvailableLanguages()
        {
            List<Language> langs = GetLanguages();
            Dictionary<LanguageType, string> retdict = new Dictionary<LanguageType, string>();

            foreach (Language lang in langs)
            {
                Helpers.ConsolePrint("NICEHASH", "Found language: " + lang.Name);
                retdict.Add(lang.ID, lang.Name);
            }

            return retdict;
        }

        public static string GetText(string token)
        {
            if (SelectedLanguage == null) return "";

            if (SelectedLanguage.Entries.ContainsKey(token))
                return SelectedLanguage.Entries[token];
            else
                return "";
        }
    }
}
