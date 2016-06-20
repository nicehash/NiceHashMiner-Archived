using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace nvidiasetp0state
{
    class Program
    {
        private static int SetP0State()
        {
            string stdOut, stdErr, args, smiPath;
            stdOut = stdErr = args = String.Empty;
            smiPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\NVIDIA Corporation\\NVSMI\\nvidia-smi.exe";
            if (smiPath.Contains(" (x86)")) smiPath = smiPath.Replace(" (x86)", "");

            try
            {
                Process P = new Process();
                P.StartInfo.FileName = smiPath;
                P.StartInfo.Arguments = "--list-gpus";
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.CreateNoWindow = true;
                P.Start();
                P.WaitForExit();

                stdOut = P.StandardOutput.ReadToEnd();
                stdErr = P.StandardError.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ChangeP0State] Exception: " + ex.Message);
                return 1;
            }

            if (stdOut.Length < 10)
            {
                Console.WriteLine("[ChangeP0State] NVSMI: Error! Output too short. (" + stdOut + ")");
                return 2;
            }
            else
            {
                string[] strGPUs = stdOut.Split('\n');
                int numGPUs = strGPUs.Length - 1;
                Console.WriteLine("[ChangeP0State] Num GPUs: " + numGPUs);

                for (int i = 0; i < numGPUs; i++)
                {
                    string mem, clk;
                    mem = clk = String.Empty;

                    try
                    {
                        args = "-i " + i + " -q -d SUPPORTED_CLOCKS";
                        Console.WriteLine("[ChangeP0State] GetClocks Start Process: " + args);
                        Process GetClocks = new Process();
                        GetClocks.StartInfo.FileName = smiPath;
                        GetClocks.StartInfo.Arguments = args;
                        GetClocks.StartInfo.UseShellExecute = false;
                        GetClocks.StartInfo.RedirectStandardOutput = true;
                        GetClocks.Start();

                        string outdata;
                        do
                        {
                            outdata = GetClocks.StandardOutput.ReadLine();
                            if (outdata != null)
                            {
                                if (outdata.Contains("Memory"))
                                {
                                    mem = outdata.Split(':')[1].Trim();
                                    mem = mem.Substring(0, mem.Length - 4);
                                }
                                else if (outdata.Contains("Graphics"))
                                {
                                    clk = outdata.Split(':')[1].Trim();
                                    clk = clk.Substring(0, clk.Length - 4);
                                    break;
                                }
                            }
                        } while (outdata != null);

                        GetClocks.Kill();
                        GetClocks.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[ChangeP0State] Exception: " + ex.Message);
                    }

                    if (mem.Length > 1 && clk.Length > 1)
                    {
                        try
                        {
                            args = "-i " + i + " -ac " + mem + "," + clk;
                            Console.WriteLine("[ChangeP0State] SetClock Start Process: " + args);
                            Process SetClock = new Process();
                            SetClock.StartInfo.FileName = smiPath;
                            SetClock.StartInfo.Arguments = args;
                            SetClock.StartInfo.UseShellExecute = false;
                            SetClock.StartInfo.RedirectStandardOutput = true;
                            SetClock.Start();

                            string outdata = SetClock.StandardOutput.ReadToEnd();
                            Console.WriteLine("[ChangeP0State] SetClock: " + outdata);
                            if (outdata.Contains("Applications clocks set to"))
                                Console.WriteLine("[ChangeP0State] SetClock: Successfully set.");
                            else if (outdata.Contains("is not supported"))
                                Console.WriteLine("[ChangeP0State] SetClock: Setting applications clocks is not supported.");
                            else if (outdata.Contains("does not have permission"))
                            {
                                Console.WriteLine("[ChangeP0State] SetClock: The current user does not have permission to change clocks.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[ChangeP0State] Exception: " + ex.Message);
                        }
                    }
                }
            }

            return 0;
        }

        static void Main(string[] args)
        {
            Environment.ExitCode = SetP0State();
        }
    }
}
