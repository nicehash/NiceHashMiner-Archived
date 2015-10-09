using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    class Algorithm
    {
        private static Algorithm[] AlgorithmNames = { new Algorithm(0, "scrypt", "scrypt", "scrypt"),
                                                      new Algorithm(13, "axiom", "axiom", "axiom")};

        public int NiceHashID;
        public string NiceHashName;
        public string ccminerName;
        public string sgminerName;


        public Algorithm(int id, string nhname, string ccname, string sgname)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            ccminerName = ccname;
            sgminerName = sgname;
        }


        public static Algorithm GetFromccminer(string aname)
        {
            for (int i = 0; i < AlgorithmNames.Length; i++)
            {
                if (AlgorithmNames[i].ccminerName == aname)
                    return AlgorithmNames[i];
            }

            return AlgorithmNames[0];
        }
    }
}
