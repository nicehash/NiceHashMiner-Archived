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

            if (argv.Length > 0 && argv[0] == "-config")
                Application.Run(new Form1(true));
            else
                Application.Run(new Form1(false));
        }
    }
}
