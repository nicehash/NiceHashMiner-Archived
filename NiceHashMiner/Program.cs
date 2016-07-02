using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NiceHashMiner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool ConfigExist = Config.ConfigFileExist();
            
            Config.InitializeConfig();

            if (Config.ConfigData.LogToFile)
                Logger.ConfigureWithFile();

            if (Config.ConfigData.DebugConsole)
                Helpers.AllocConsole();

            Helpers.ConsolePrint("NICEHASH", "Starting up NiceHashMiner v" + Application.ProductVersion);

            string tmp;
            if (!ConfigExist && !ParseCommandLine(argv, "-lang", out tmp))
            {
                Helpers.ConsolePrint("NICEHASH", "No config file found. Running NiceHash Miner for the first time. Choosing a default language.");
                Application.Run(new Form_ChooseLanguage());
            }

            // Init languages
            International.Initialize(Config.ConfigData.Language);

            if (argv.Length > 0)
            {
                if (ParseCommandLine(argv, "-lang", out tmp))
                {
                    int lang;
                    if (Int32.TryParse(tmp, out lang))
                    {
                        Helpers.ConsolePrint("NICEHASH", "Language is overwritten by command line parameter (-lang).");
                        International.Initialize(lang);
                        Config.ConfigData.Language = lang;
                    }
                }

                if (ParseCommandLine(argv, "-config", out tmp))
                    Application.Run(new Form1(true));
            }
            
            Application.Run(new Form1(false));
        }

        private static bool ParseCommandLine(string[] argv, string find, out string value)
        {
            value = "";

            for (int i = 0; i < argv.Length; i++)
            {
                if (argv[i].Equals(find))
                {
                    if ((i + 1) < argv.Length && argv[i + 1].Trim()[0] != '-')
                    {
                        value = argv[i + 1];
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
