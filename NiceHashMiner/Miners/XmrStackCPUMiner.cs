using Newtonsoft.Json.Linq;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace NiceHashMiner.Miners {
    
    public class XmrStackCPUMinerConfig {
        public XmrStackCPUMinerConfig(int numberOfthreads, string poolAddr, string wallet, int port) {
            this.cpu_thread_num = numberOfthreads;
            this.pool_address = poolAddr;
            this.wallet_address = wallet;
            this.httpd_port = port;
        }

        public void Inti_cpu_threads_conf(bool low_power_mode, bool no_prefetch, bool affine_to_cpu, bool isHyperThreading) {
            cpu_threads_conf = new List<JObject>();
            if (isHyperThreading) {
                for (int i_cpu = 0; i_cpu < cpu_thread_num; ++i_cpu) {
                    cpu_threads_conf.Add(JObject.FromObject(new { low_power_mode = low_power_mode, no_prefetch = no_prefetch, affine_to_cpu = i_cpu * 2 }));
                }
            } else {
                for (int i_cpu = 0; i_cpu < cpu_thread_num; ++i_cpu) {
                    if (affine_to_cpu) {
                        cpu_threads_conf.Add(JObject.FromObject(new { low_power_mode = low_power_mode, no_prefetch = no_prefetch, affine_to_cpu = i_cpu }));
                    } else {
                        cpu_threads_conf.Add(JObject.FromObject(new { low_power_mode = low_power_mode, no_prefetch = no_prefetch, affine_to_cpu = false }));
                    }
                }
            }
        }

        /* 
         * Number of threads. You can configure them below. Cryptonight uses 2MB of memory, so the optimal setting 
         * here is the size of your L3 cache divided by 2. Intel mid-to-high end desktop processors have 2MB of L3
         * cache per physical core. Low end cpus can have 1.5 or 1 MB while Xeons can have 2, 2.5 or 3MB per core.
         */
        public readonly int cpu_thread_num;

        /*
         * Thread configuration for each thread. Make sure it matches the number above.
         * low_power_mode - This mode will double the cache usage, and double the single thread performance. It will 
         *                  consume much less power (as less cores are working), but will max out at around 80-85% of 
         *                  the maximum performance.
         *
         * no_prefetch -    This mode meant for large pages only. It will generate an error if running on slow memory
         *                  Some sytems can gain up to extra 5% here, but sometimes it will have no difference or make
         *                  things slower.
         *
         * affine_to_cpu -  This can be either false (no affinity), or the CPU core number. Note that on hyperthreading 
         *                  systems it is better to assign threads to physical cores. On Windows this usually means selecting 
         *                  even or odd numbered cpu numbers. For Linux it will be usually the lower CPU numbers, so for a 4 
         *                  physical core CPU you should select cpu numbers 0-3.
         *
         */
        public List<JObject> cpu_threads_conf;
        //"cpu_threads_conf" : [ 
        //    { "low_power_mode" : false, "no_prefetch" : false, "affine_to_cpu" : 0 },
        //    { "low_power_mode" : false, "no_prefetch" : false, "affine_to_cpu" : 1 },
        //],

        /*
         * LARGE PAGE SUPPORT
         * Lare pages need a properly set up OS. It can be difficult if you are not used to systems administation,
         * but the performace results are worth the trouble - you will get around 20% boost. Slow memory mode is
         * meant as a backup, you won't get stellar results there. If you are running into trouble, especially
         * on Windows, please read the common issues in the README.
         *
         * By default we will try to allocate large pages. This means you need to "Run As Administrator" on Windows.
         * You need to edit your system's group policies to enable locking large pages. Here are the steps from MSDN
         *
         * 1. On the Start menu, click Run. In the Open box, type gpedit.msc.
         * 2. On the Local Group Policy Editor console, expand Computer Configuration, and then expand Windows Settings.
         * 3. Expand Security Settings, and then expand Local Policies.
         * 4. Select the User Rights Assignment folder.
         * 5. The policies will be displayed in the details pane.
         * 6. In the pane, double-click Lock pages in memory.
         * 7. In the Local Security Setting – Lock pages in memory dialog box, click Add User or Group.
         * 8. In the Select Users, Service Accounts, or Groups dialog box, add an account that you will run the miner on
         * 9. Reboot for change to take effect.
         *
         * Windows also tends to fragment memory a lot. If you are running on a system with 4-8GB of RAM you might need
         * to switch off all the auto-start applications and reboot to have a large enough chunk of contiguous memory.
         *
         * On Linux you will need to configure large page support "sudo sysctl -w vm.nr_hugepages=128" and increase your
         * ulimit -l. To do do this you need to add following lines to /etc/security/limits.conf - "* soft memlock 262144"
         * and "* hard memlock 262144". You can also do it Windows-style and simply run-as-root, but this is NOT
         * recommended for security reasons.
         *
         * Memory locking means that the kernel can't swap out the page to disk - something that is unlikey to happen on a 
         * command line system that isn't starved of memory. I haven't observed any difference on a CLI Linux system between 
         * locked and unlocked memory. If that is your setup see option "no_mlck". 
         */

        /*
         * use_slow_memory defines our behaviour with regards to large pages. There are three possible options here:
         * always  - Don't even try to use large pages. Always use slow memory.
         * warn    - We will try to use large pages, but fall back to slow memory if that fails.
         * no_mlck - This option is only relevant on Linux, where we can use large pages without locking memory.
         *           It will never use slow memory, but it won't attempt to mlock
         * never   - If we fail to allocate large pages we will print an error and exit.
         */
        public string use_slow_memory = "warn";

        /*
         * NiceHash mode
         * nicehash_nonce - Limit the noce to 3 bytes as required by nicehash. This cuts all the safety margins, and
         *                  if a block isn't found within 30 minutes then you might run into nonce collisions. Number
         *                  of threads in this mode is hard-limited to 32.
         */
        public readonly bool nicehash_nonce = true; // 

        /*
         * pool_address	  - Pool address should be in the form "pool.supportxmr.com:3333". Only stratum pools are supported.
         * wallet_address - Your wallet, or pool login.
         * pool_password  - Can be empty in most cases or "x".
         */

        public readonly string pool_address; // : "pool.supportxmr.com:3333",
        public readonly string wallet_address;
        public readonly string pool_password = "x";

        /*
         * Network timeouts.
         * Because of the way this client is written it doesn't need to constantly talk (keep-alive) to the server to make 
         * sure it is there. We detect a buggy / overloaded server by the call timeout. The default values will be ok for 
         * nearly all cases. If they aren't the pool has most likely overload issues. Low call timeout values are preferable -
         * long timeouts mean that we waste hashes on potentially stale jobs. Connection report will tell you how long the
         * server usually takes to process our calls.
         *
         * call_timeout - How long should we wait for a response from the server before we assume it is dead and drop the connection.
         * retry_time	- How long should we wait before another connection attempt.
         *                Both values are in seconds.
         */
        public int call_timeout = 10;
        public int retry_time = 10;

        /*
         * Output control.
         * Since most people are used to miners printing all the time, that's what we do by default too. This is suboptimal
         * really, since you cannot see errors under pages and pages of text and performance stats. Given that we have internal
         * performance monitors, there is very little reason to spew out pages of text instead of concise reports.
         * Press 'h' (hashrate), 'r' (results) or 'c' (connection) to print reports.
         *
         * verbose_level - 0 - Don't print anything. 
         *                 1 - Print intro, connection event, disconnect event
         *                 2 - All of level 1, and new job (block) event if the difficulty is different from the last job
         *                 3 - All of level 1, and new job (block) event in all cases, result submission event.
         *                 4 - All of level 3, and automatic hashrate report printing 
         */
        public int verbose_level = 3;

        /*
         * Automatic hashrate report
         *
         * h_print_time - How often, in seconds, should we print a hashrate report if verbose_level is set to 4.
         *                This option has no effect if verbose_level is not 4.
         */
        public int h_print_time = 60;

        /*
         * Built-in web server
         * I like checking my hashrate on my phone. Don't you?
         * Keep in mind that you will need to set up port forwarding on your router if you want to access it from
         * outside of your home network. Ports lower than 1024 on Linux systems will require root.
         *
         * httpd_port - Port we should listen on. Default, 0, will switch off the server.
         */
        public readonly int httpd_port;

        /*
         * prefer_ipv4 - IPv6 preference. If the host is available on both IPv4 and IPv6 net, which one should be choose?
         *               This setting will only be needed in 2020's. No need to worry about it now.
         */
        public bool prefer_ipv4 = true;

    }

    public class XmrStackCPUMiner : Miner {
        public XmrStackCPUMiner()
            : base("XmrStackCPUMiner") {
            this.ConectionType = NHMConectionType.NONE;
            IsNeverHideMiningWindow = true;
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 3600000; // 1hour
        }


        private string GetConfigFileName() {
            return String.Format("config_{0}.txt", this.MiningSetup.MiningPairs[0].Device.ID);
        }

        private void prepareConfigFile(string pool, string wallet) {
            if (this.MiningSetup.MiningPairs.Count > 0) {
                try {
                    bool IsHyperThreadingEnabled = this.MiningSetup.MiningPairs[0].CurrentExtraLaunchParameters.Contains("enable_ht=true");
                    int numTr = ExtraLaunchParametersParser.GetThreadsNumber(this.MiningSetup.MiningPairs[0]);
                    if (IsHyperThreadingEnabled) {
                        numTr /= 2;
                    }
                    var config = new XmrStackCPUMinerConfig(numTr, pool, wallet, this.APIPort);

                    //config.Inti_cpu_threads_conf(false, false, true, ComputeDeviceManager.Avaliable.IsHyperThreadingEnabled);
                    config.Inti_cpu_threads_conf(false, false, false, IsHyperThreadingEnabled);
                    var confJson = JObject.FromObject(config);
                    string writeStr = confJson.ToString();
                    int start = writeStr.IndexOf("{");
                    int end = writeStr.LastIndexOf("}");
                    writeStr = writeStr.Substring(start + 1, end - 1);
                    System.IO.File.WriteAllText(WorkingDirectory + GetConfigFileName(), writeStr);
                } catch {
                }
            }
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        public override APIData GetSummary() {
            string resp;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            string DataToSend = GetHttpRequestNHMAgentStrin("h");

            resp = GetAPIData(APIPort, DataToSend);
            if (resp == null) {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " summary is null");
                _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                return null;
            }
            const string Totals = "Totals:";
            const string Highest = "Highest:";
            int start_i = resp.IndexOf(Totals);
            int end_i = resp.IndexOf(Highest);
            if (start_i > -1 && end_i > -1) {
                string sub_resp = resp.Substring(start_i, end_i);
                sub_resp = sub_resp.Replace(Totals, "");
                sub_resp = sub_resp.Replace(Highest, "");
                var strings = sub_resp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in strings) {
                    if (s != "(na)") {
                        _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
                        ad.Speed = Helpers.ParseDouble(s);
                        break;
                    }
                }
            }
            // check if speed zero
            if (ad.Speed == 0) _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;

            return ad;
        }

        public override void Start(string url, string btcAdress, string worker) {
            if (!IsInit) {
                Helpers.ConsolePrint(MinerTAG(), "MiningSetup is not initialized exiting Start()");
                return;
            }
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = GetConfigFileName();

            prepareConfigFile(url, username);

            ProcessHandle = _Start();
        }

        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();

            var AffinityMask = MiningSetup.MiningPairs[0].Device.AffinityMask;
            if (AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, AffinityMask);

            return P;
        }

        // doesn't work stubs
        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            string url = Globals.GetLocationURL(algorithm.NiceHashID, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
            prepareConfigFile(url, Globals.DemoUser);
            return "benchmark_mode " + GetConfigFileName();
        }

        protected override Process BenchmarkStartProcess(string CommandLine) {
            Process BenchmarkHandle = base.BenchmarkStartProcess(CommandLine);

            var AffinityMask = MiningSetup.MiningPairs[0].Device.AffinityMask;
            if (AffinityMask != 0 && BenchmarkHandle != null)
                CPUID.AdjustAffinity(BenchmarkHandle.Id, AffinityMask);

            return BenchmarkHandle;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            if (outdata.Contains("Total:")) {
                string toParse = outdata.Substring(outdata.IndexOf("Total:")).Replace("Total:", "").Trim();
                var strings = toParse.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in strings) {
                    double lastSpeed = 0;
                    if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out lastSpeed)) {
                        Helpers.ConsolePrint("BENCHMARK " + MinerTAG(), "double.TryParse true. Last speed is" + lastSpeed.ToString());
                        BenchmarkAlgorithm.BenchmarkSpeed = Helpers.ParseDouble(s);
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }
    }
}
