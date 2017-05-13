using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices {
    public static class CUDA_Unsupported {
        private static List<string> SM_1_0 = new List<string>() {
            "GeForce 8800 Ultra",
            "GeForce 8800 GTX",
            "GeForce 8800 GTS",
            "Quadro FX 5600",
            "Quadro FX 4600",
            "Quadro Plex 2100 S4",
            "Tesla C870",
            "Tesla D870",
            "Tesla S870",
        };
        private static List<string> SM_1_1 = new List<string>() {
            "GeForce GTS 250",
            "GeForce 9800 GX2",
            "GeForce 9800 GTX",
            "GeForce 9800 GT",
            "GeForce 8800 GTS",
            "GeForce 8800 GT",
            "GeForce 9600 GT",
            "GeForce 9500 GT",
            "GeForce 9400 GT",
            "GeForce 8600 GTS",
            "GeForce 8600 GT",
            "GeForce 8500 GT",
            "GeForce G110M",
            "GeForce 9300M GS",
            "GeForce 9200M GS",
            "GeForce 9100M G",
            "GeForce 8400M GT","GeForce G105M",
            "Quadro FX 4700 X2",
            "Quadro FX 3700",
            "Quadro FX 1800",
            "Quadro FX 1700",
            "Quadro FX 580",
            "Quadro FX 570",
            "Quadro FX 470",
            "Quadro FX 380",
            "Quadro FX 370",
            "Quadro FX 370 Low Profile",
            "Quadro NVS 450",
            "Quadro NVS 420",
            "Quadro NVS 290",
            "Quadro NVS 295",
            "Quadro Plex 2100 D4",
            "Quadro FX 3800M",
            "Quadro FX 3700M",
            "Quadro FX 3600M",
            "Quadro FX 2800M",
            "Quadro FX 2700M",
            "Quadro FX 1700M",
            "Quadro FX 1600M",
            "Quadro FX 770M",
            "Quadro FX 570M",
            "Quadro FX 370M",
            "Quadro FX 360M",
            "Quadro NVS 320M",
            "Quadro NVS 160M",
            "Quadro NVS 150M",
            "Quadro NVS 140M",
            "Quadro NVS 135M",
            "Quadro NVS 130M",
            "Quadro NVS 450",
            "Quadro NVS 420",
            "Quadro NVS 295",
        };
        private static List<string> SM_1_2 = new List<string>() {
            "GeForce GT 340",
            "GeForce GT 330",
            "GeForce GT 320",
            "GeForce 315",
            "GeForce 310",
            "GeForce GT 240",
            "GeForce GT 220",
            "GeForce 210",
            "GeForce GTS 360M",
            "GeForce GTS 350M",
            "GeForce GT 335M",
            "GeForce GT 330M",
            "GeForce GT 325M",
            "GeForce GT 240M",
            "GeForce G210M",
            "GeForce 310M",
            "GeForce 305M",
            "Quadro FX 380 Low Profile",
            "Nvidia NVS 300",
            "Quadro FX 1800M",
            "Quadro FX 880M",
            "Quadro FX 380M",
            "Nvidia NVS 300",
            "NVS 5100M",
            "NVS 3100M",
            "NVS 2100M",
            "ION",
        };
        private static List<string> SM_1_3 = new List<string>() {
            "GeForce GTX 295",
            "GTX 285",
            "GTX 280",
            "GeForce GTX 275",
            "GeForce GTX 260",
            "Quadro FX 5800",
            "Quadro FX 4800",
            "Quadro FX 3800",
            "Quadro CX",
            "Quadro Plex 2200 D2",
            "Tesla C1060",
            "Tesla S1070",
            "Tesla M1060",
        };
        private static List<string> SM_2_0 = new List<string>() {
            "GeForce GTX 590",
            "GeForce GTX 580",
            "GeForce GTX 570",
            "GeForce GTX 480",
            "GeForce GTX 470",
            "GeForce GTX 465",
            "GeForce GTX 480M",
            "Quadro 6000",
            "Quadro 5000",
            "Quadro 4000",
            "Quadro Plex 7000",
            "Quadro 5010M",
            "Quadro 5000M",
            "Tesla C2075",
            "Tesla C2050",
            "Tesla C2070",
            "Tesla M2050",
            "Tesla M2070",
            "Tesla M2075",
            "Tesla M2090",
        };

        private static bool ContainsSM(List<string> list, string text) {
            foreach(var el in list) {
                if(text.Contains(el)) {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSupported(string text) {
            if (ContainsSM(SM_1_0, text)) {
                return false;
            }
            if (ContainsSM(SM_1_1, text)) {
                return false;
            }
            if (ContainsSM(SM_1_2, text)) {
                return false;
            }
            if (ContainsSM(SM_1_3, text)) {
                return false;
            }
            if (ContainsSM(SM_2_0, text)) {
                return false;
            }

            return true;
        }
    }
}
