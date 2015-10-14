using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    class Algorithm
    {
        //private static Algorithm[] AlgorithmNames = { new Algorithm(0, "scrypt", "scrypt"),
        //                                              new Algorithm(13, "axiom", "axiom")};

        public int NiceHashID;
        public string NiceHashName;
        public string MinerName;


        public Algorithm(int id, string nhname, string mname)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            MinerName = mname;
        }

        //public static Algorithm GetFromMiner(string aname)
        //{
        //    for (int i = 0; i < AlgorithmNames.Length; i++)
        //    {
        //        if (AlgorithmNames[i].MinerName == aname)
        //            return AlgorithmNames[i];
        //    }

        //    return AlgorithmNames[0];
        //}
    }
}
