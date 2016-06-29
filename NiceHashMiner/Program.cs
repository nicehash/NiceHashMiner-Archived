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

            Config.InitializeConfig();

            if (Config.ConfigData.LogToFile)
                Logger.ConfigureWithFile();

            if (Config.ConfigData.DebugConsole)
                Helpers.AllocConsole();

            Helpers.ConsolePrint("NICEHASH", "Starting up NiceHashMiner v" + Application.ProductVersion);

            // Init languages
            International.Initialize(Config.ConfigData.Language);

            if (argv.Length > 0)
            {
                string val;

                if (ParseCommandLine(argv, "-lang", out val))
                {
                    int lang;
                    if (Int32.TryParse(val, out lang))
                    {
                        International.Initialize(lang);
                    }
                }
                if (ParseCommandLine(argv, "-config", out val))
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
