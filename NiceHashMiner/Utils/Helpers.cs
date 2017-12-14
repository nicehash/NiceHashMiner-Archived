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

            if (ConfigManager.GeneralConfig.LogToFile && Logger.IsInit)
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

        public static string FormatSpeedOutput(double speed, string separator=" ") {
            string ret = "";

            if (speed < 1000)
                ret = (speed).ToString("F3", CultureInfo.InvariantCulture) + separator;
            else if (speed < 100000)
                ret = (speed * 0.001).ToString("F3", CultureInfo.InvariantCulture) + separator + "k";
            else if (speed < 100000000)
                ret = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + separator + "M";
            else
                ret = (speed * 0.000000001).ToString("F3", CultureInfo.InvariantCulture) + separator + "G";

            return ret;
        }

        public static string FormatDualSpeedOutput(double primarySpeed, double secondarySpeed=0) {
            string ret;
            if (secondarySpeed > 0) {
                ret = FormatSpeedOutput(primarySpeed, "") + "/" + FormatSpeedOutput(secondarySpeed, "") + " ";
            }
            else {
                ret = FormatSpeedOutput(primarySpeed);
            }
            return ret + "H/s ";
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

        // parsing helpers
        public static int ParseInt(string text) {
            int tmpVal = 0;
            if (Int32.TryParse(text, out tmpVal)) {
                return tmpVal;
            }
            return 0;
        }
        public static long ParseLong(string text) {
            long tmpVal = 0;
            if (Int64.TryParse(text, out tmpVal)) {
                return tmpVal;
            }
            return 0;
        }
        public static double ParseDouble(string text) {
            try {
                string parseText = text.Replace(',', '.');
                return Double.Parse(parseText, CultureInfo.InvariantCulture);
            } catch {
                return 0;
            }
        }

        // IsWMI enabled
        public static bool IsWMIEnabled() {
            try {
                ManagementObjectCollection moc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem").Get();
                ConsolePrint("NICEHASH", "WMI service seems to be running, ManagementObjectSearcher returned success.");
                return true;
            }
            catch {
                ConsolePrint("NICEHASH", "ManagementObjectSearcher not working need WMI service to be running");
            }
            return false;
        }

        public static void InstallVcRedist() {
            Process CudaDevicesDetection = new Process();
            CudaDevicesDetection.StartInfo.FileName = @"bin\vc_redist.x64.exe";
            CudaDevicesDetection.StartInfo.Arguments = "/q /norestart";
            CudaDevicesDetection.StartInfo.UseShellExecute = false;
            CudaDevicesDetection.StartInfo.RedirectStandardError = false;
            CudaDevicesDetection.StartInfo.RedirectStandardOutput = false;
            CudaDevicesDetection.StartInfo.CreateNoWindow = false;

            //const int waitTime = 45 * 1000; // 45seconds
            //CudaDevicesDetection.WaitForExit(waitTime);
            CudaDevicesDetection.Start();
        }

        public static void SetDefaultEnvironmentVariables()
        {
            Helpers.ConsolePrint("NICEHASH", "Setting environment variables");

            Dictionary<string, string> envNameValues = new Dictionary<string, string>() {
                { "GPU_MAX_ALLOC_PERCENT",      "100" },
                { "GPU_USE_SYNC_OBJECTS",       "1" },
                { "GPU_SINGLE_ALLOC_PERCENT",   "100" },
                { "GPU_MAX_HEAP_SIZE",          "100" },
                { "GPU_FORCE_64BIT_PTR",        "1" }
            };

            foreach (var kvp in envNameValues) {
                string envName = kvp.Key;
                string envValue = kvp.Value;
                // Check if all the variables is set
                if (Environment.GetEnvironmentVariable(envName) == null)
                {
                    try { Environment.SetEnvironmentVariable(envName, envValue); }
                    catch (Exception e) { Helpers.ConsolePrint("NICEHASH", e.ToString()); }
                }

                // Check to make sure all the values are set correctly
                if (!Environment.GetEnvironmentVariable(envName).Equals(envValue))
                {
                    try { Environment.SetEnvironmentVariable(envName, envValue); }
                    catch (Exception e) { Helpers.ConsolePrint("NICEHASH", e.ToString()); }
                }
            }
        }

    }
}
