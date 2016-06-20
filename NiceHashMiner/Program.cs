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

            if (argv.Length > 0 && argv[0] == "-config")
                Application.Run(new Form1(true));
            else
                Application.Run(new Form1(false));
        }
    }
}
