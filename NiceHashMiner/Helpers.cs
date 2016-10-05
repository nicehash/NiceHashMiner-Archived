using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using NiceHashMiner.Configs;
using System.Globalization;
using NiceHashMiner.PInvoke;
using System.Management;

namespace NiceHashMiner
{
    class Helpers : PInvokeHelpers
    {
        

        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }

        public static void ConsolePrint(string grp, string text)
        {
            Console.WriteLine("[" +DateTime.Now.ToLongTimeString() + "] [" + grp + "] " + text);

            if (ConfigManager.Instance.GeneralConfig.LogToFile)
                Logger.log.Info("[" + grp + "] " + text);
        }

        public static void ConsolePrint(string grp, string text, params object[] arg)
        {
            ConsolePrint(grp, String.Format(text, arg));
        }

        public static void ConsolePrint(string grp, string text, object arg0)
        {
            ConsolePrint(grp, String.Format(text, arg0));
        }

        public static void ConsolePrint(string grp, string text, object arg0, object arg1)
        {
            ConsolePrint(grp, String.Format(text, arg0, arg1));
        }

        public static void ConsolePrint(string grp, string text, object arg0, object arg1, object arg2)
        {
            ConsolePrint(grp, String.Format(text, arg0, arg1, arg2));
        }    

        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }

        public static void DisableWindowsErrorReporting(bool en)
        {
            //bool failed = false;

            Helpers.ConsolePrint("NICEHASH", "Trying to enable/disable Windows error reporting");

            // CurrentUser
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\Windows Error Reporting"))
                {
                    if (rk != null)
                    {
                        Object o = rk.GetValue("DontShowUI");
                        if (o != null)
                        {
                            int val = (int)o;
                            Helpers.ConsolePrint("NICEHASH", "Current DontShowUI value: " + val);

                            if (val == 0 && en)
                            {
                                Helpers.ConsolePrint("NICEHASH", "Setting register value to 1.");
                                rk.SetValue("DontShowUI", 1);
                            }
                            else if (val == 1 && !en)
                            {
                                Helpers.ConsolePrint("NICEHASH", "Setting register value to 0.");
                                rk.SetValue("DontShowUI", 0);
                            }
                        }
                        else
                        {
                            Helpers.ConsolePrint("NICEHASH", "Registry key not found .. creating one..");
                            rk.CreateSubKey("DontShowUI", RegistryKeyPermissionCheck.Default);
                            Helpers.ConsolePrint("NICEHASH", "Setting register value to 1..");
                            rk.SetValue("DontShowUI", en ? 1 : 0);
                        }
                    }
                    else
                        Helpers.ConsolePrint("NICEHASH", "Unable to open SubKey.");
                }
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint("NICEHASH", "Unable to access registry. Error: " + ex.Message);
            }
        }

        public static string FormatSpeedOutput(double speed) {
            string ret = "";

            if (speed < 1000)
                ret = (speed).ToString("F3", CultureInfo.InvariantCulture) + " H/s ";
            else if (speed < 100000)
                ret = (speed * 0.001).ToString("F3", CultureInfo.InvariantCulture) + " kH/s ";
            else if (speed < 100000000)
                ret = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s ";
            else
                ret = (speed * 0.000000001).ToString("F3", CultureInfo.InvariantCulture) + " GH/s ";

            return ret;
        }

        public static string GetMotherboardID() {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = mos.Get();
            string serial = "";
            foreach (ManagementObject mo in moc) {
                serial = (string)mo["SerialNumber"];
            }

            return serial;
        }

        // TODO could have multiple cpus
        public static string GetCpuID() {
            string id = "N/A";
            try {
                ManagementObjectCollection mbsList = null;
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                mbsList = mbs.Get();
                foreach (ManagementObject mo in mbsList) {
                    id = mo["ProcessorID"].ToString();
                }
            } catch { }
            return id;
        }

        public static bool WebRequestTestGoogle() {
            string url = "http://www.google.com";
            try {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                myRequest.Timeout = Globals.FirstNetworkCheckTimeoutTimeMS;
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            } catch (System.Net.WebException) {
                return false;
            }
            return true;
        }

        // Checking the version using >= will enable forward compatibility, 
        // however you should always compile your code on newer versions of
        // the framework to ensure your app works the same.
        private static bool Is45DotVersion(int releaseKey) {
            if (releaseKey >= 393295) {
                //return "4.6 or later";
                return true;
            }
            if ((releaseKey >= 379893)) {
                //return "4.5.2 or later";
                return true;
            }
            if ((releaseKey >= 378675)) {
                //return "4.5.1 or later";
                return true;
            }
            if ((releaseKey >= 378389)) {
                //return "4.5 or later";
                return true;
            }
            // This line should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            //return "No 4.5 or later version detected";
            return false;
        }

        public static bool Is45NetOrHigher() {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\")) {
                if (ndpKey != null && ndpKey.GetValue("Release") != null) {
                    return Is45DotVersion((int)ndpKey.GetValue("Release"));
                } else {
                    return false;
                }
            }
        }

        public static bool IsConnectedToInternet() {
            bool returnValue = false;
            try {
                int Desc;
                returnValue = InternetGetConnectedState(out Desc, 0);
            } catch {
                returnValue = false;
            }
            return returnValue;
        }

    }
}
