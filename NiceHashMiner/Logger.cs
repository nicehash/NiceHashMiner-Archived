using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using NiceHashMiner.Configs;
using System.IO;

namespace NiceHashMiner
{
    public class Logger
    {
        public static bool IsInit = false;
        public static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        public const string _logPath = @"logs\";

        public static void ConfigureWithFile()
        {
            try {
                if (!Directory.Exists("logs")) {
                    Directory.CreateDirectory("logs");
                }
            } catch { }

            IsInit = true;
            try {
                Hierarchy h = (Hierarchy)LogManager.GetRepository();

                if (ConfigManager.GeneralConfig.LogToFile)
                    h.Root.Level = Level.Info;
                //else if (ConfigManager.Instance.GeneralConfig.LogLevel == 2)
                //    h.Root.Level = Level.Warn;
                //else if (ConfigManager.Instance.GeneralConfig.LogLevel == 3)
                //    h.Root.Level = Level.Error;

                h.Root.AddAppender(CreateFileAppender());
                h.Configured = true;
            } catch (Exception e) {
                IsInit = false;
            }
        }

        public static IAppender CreateFileAppender()
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = "RollingFileAppender";
            appender.File = _logPath + "log.txt";
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.MaxSizeRollBackups = 1;
            appender.MaxFileSize = ConfigManager.GeneralConfig.LogMaxFileSize;
            appender.PreserveLogFileNameExtension = true;
            appender.Encoding = System.Text.Encoding.Unicode;

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "[%date{yyyy-MM-dd HH:mm:ss}] [%level] %message%newline";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }
    }
}
