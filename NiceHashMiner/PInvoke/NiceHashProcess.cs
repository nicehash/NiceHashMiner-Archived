using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace NiceHashMiner
{
    public class NiceHashProcess
    {
        private const uint CREATE_NEW_CONSOLE = 0x00000010;
        private const uint NORMAL_PRIORITY_CLASS = 0x0020;
        private const uint CREATE_NO_WINDOW = 0x08000000;
        private const int STARTF_USESHOWWINDOW = 0x00000001;
        private const short SW_SHOWMINNOACTIVE = 7;
        private const uint INFINITE = 0xFFFFFFFF;
        private const uint STILL_ACTIVE = 259;

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 ProcessId;
            public Int32 ThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool CreatePipe(out IntPtr hReadPipe, out IntPtr hWritePipe,
           ref SECURITY_ATTRIBUTES lpPipeAttributes, uint nSize);

        public delegate void ExitEventDelegate();

        public ProcessStartInfo StartInfo;
        public ExitEventDelegate ExitEvent;
        public uint ExitCode;
        public int Id;

        private Thread tHandle;
        private bool bRunning;
        private IntPtr pHandle;

        public NiceHashProcess()
        {
            StartInfo = new ProcessStartInfo();
        }

        public bool Start()
        {
            PROCESS_INFORMATION pInfo = new PROCESS_INFORMATION();
            STARTUPINFO sInfo = new STARTUPINFO();
            SECURITY_ATTRIBUTES pSec = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES tSec = new SECURITY_ATTRIBUTES();
            pSec.nLength = Marshal.SizeOf(pSec);
            tSec.nLength = Marshal.SizeOf(tSec);

            uint sflags = 0;
            if (StartInfo.CreateNoWindow) {
                sflags = CREATE_NO_WINDOW;
            }
            else if (StartInfo.WindowStyle == ProcessWindowStyle.Minimized) {
                sInfo.dwFlags = STARTF_USESHOWWINDOW;
                sInfo.wShowWindow = SW_SHOWMINNOACTIVE;
                sflags = CREATE_NEW_CONSOLE;
            }
            else {
                sflags = CREATE_NEW_CONSOLE;
            }

            string workDir = null;
            if (StartInfo.WorkingDirectory != null && StartInfo.WorkingDirectory.Length > 0)
                workDir = StartInfo.WorkingDirectory;

            bool res = CreateProcess(StartInfo.FileName,
                " " + StartInfo.Arguments,
                ref pSec,
                ref tSec,
                false,
                sflags | NORMAL_PRIORITY_CLASS,
                IntPtr.Zero,
                workDir,
                ref sInfo, 
                out pInfo);

            if (!res)
            {
                int err = Marshal.GetLastWin32Error();
                throw new Exception("Failed to start process, err=" + err.ToString());
            }

            CloseHandle(sInfo.hStdError);
            CloseHandle(sInfo.hStdInput);
            CloseHandle(sInfo.hStdOutput);

            pHandle = pInfo.hProcess;
            CloseHandle(pInfo.hThread);

            Id = pInfo.ProcessId;

            if (ExitEvent != null)
            {
                bRunning = true;
                tHandle = new Thread(cThread);
                tHandle.Start();
            }

            return true;
        }

        public void Kill()
        {
            if (pHandle == IntPtr.Zero) return;

            if (tHandle != null)
            {
                bRunning = false;
                tHandle.Join();
            }

            TerminateProcess(pHandle, 0);
            CloseHandle(pHandle);
            pHandle = IntPtr.Zero;
        }

        public void Close()
        {
            if (pHandle == IntPtr.Zero) return;

            if (tHandle != null)
            {
                bRunning = false;
                tHandle.Join();
            }

            CloseHandle(pHandle);
            pHandle = IntPtr.Zero;
        }

        private void cThread()
        {
            while (bRunning)
            {
                if (WaitForSingleObject(pHandle, 10) == 0)
                {
                    GetExitCodeProcess(pHandle, out ExitCode);
                    if (ExitCode != STILL_ACTIVE)
                    {
                        CloseHandle(pHandle);
                        pHandle = IntPtr.Zero;
                        ExitEvent();
                        return;
                    }
                }
            }
        }
    }
}
