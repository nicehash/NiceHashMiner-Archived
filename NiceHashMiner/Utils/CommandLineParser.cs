using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Utils
{
    class CommandLineParser
    {
        //// single parameter flags don't need aditional parameters
        //private static readonly string[] SingleParameterFlags = { "-config" };
        //private static readonly string[] DoubleParameterFlags = { "-lang" };
        //// TODO temp, for now we only have two
        //Dictionary<string, bool> IsParameterUsedDict = new Dictionary<string, bool>();
        //Dictionary<string, string> DoubleParameterUsedDict = new Dictionary<string, string>();

        // keep it simple only two parameters for now
        bool isConfig = false;
        bool isLang = false;
        int langValue = 0;

        // properties
        public bool IsConfig { get { return isConfig; } }
        public bool IsLang { get { return isLang; } }
        public int LangValue { get { return langValue; } }

        public CommandLineParser(string[] argv)
        {
            string tmpString;
            if(ParseCommandLine(argv, "-config", out tmpString)) {
                isConfig = true;
            }
            if (ParseCommandLine(argv, "-lang", out tmpString)) {
                isLang = true;
                // if parsing fails set to default
                if (!Int32.TryParse(tmpString, out langValue)) {
                    langValue = 0;
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
