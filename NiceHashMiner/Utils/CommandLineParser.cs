using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Utils
{
    class CommandLineParser
    {
        // keep it simple only two parameters for now
        readonly public bool IsLang = false;
        readonly public int LangValue = 0;

        public CommandLineParser(string[] argv)
        {
            string tmpString;
            if(ParseCommandLine(argv, "-config", out tmpString)) {
                Helpers.ConsolePrint("CommandLineParser", "-config parameter has been depreciated, run setting from GUI");
            }
            if (ParseCommandLine(argv, "-lang", out tmpString)) {
                IsLang = true;
                // if parsing fails set to default
                if (!Int32.TryParse(tmpString, out LangValue)) {
                    LangValue = 0;
                }
            }
        }

        private bool ParseCommandLine(string[] argv, string find, out string value)
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
