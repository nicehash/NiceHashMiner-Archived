using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NiceHashMiner.Utils
{
    class SettingsUtil
    {
        private static Dictionary<string, string> registry;

        private static string fileName;

        public static void Init()
        {
            registry = new Dictionary<string, string>();

            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NiceHash\\DeviceNames.dat");
        }

        public static void LoadSettings()
        {
            var dir = Path.GetDirectoryName(fileName);

            if (dir == null || !Directory.Exists(dir) || File.Exists(dir))
                return;

            var bf = new BinaryFormatter();

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (var fs = File.OpenRead(fileName))
                    {
                        registry = (Dictionary<string, string>)bf.Deserialize(fs);
                    }

                    return;
                }
                catch
                {
                }
            }
        }

        public static void SaveSettings()
        {
            var dir = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var bf = new BinaryFormatter();

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (var fs = File.OpenWrite(fileName))
                    {
                        bf.Serialize(fs, registry);
                    }

                    return;
                }
                catch
                {
                }
            }
        }

        public static void SetNameForDevice(string name, string deviceUUID)
        {
            if (registry.ContainsKey(deviceUUID))
                registry.Remove(deviceUUID);

            registry.Add(deviceUUID, name);
        }

        public static string Translate(string deviceUUID, string defaultName)
        {
            if (!registry.TryGetValue(deviceUUID, out var value))
                return defaultName;

            return value;
        }
    }
}
