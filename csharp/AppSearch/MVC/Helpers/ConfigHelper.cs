using AppSearch.MVC.Models;
using System.IO;
using System.Xml.Serialization;

namespace AppSearch.MVC.Helpers
{
    public class ConfigHelper
    {
        public static void SaveConfig(ConfigurationModel config)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Properties.Resources.ApplicationName,
                Properties.Resources.ConfigFileName + ".xml");

            SaveConfig(config, path);
        }

        public static void SaveConfig(ConfigurationModel config, string filePath)
        {
            XmlSerializer serializer = new(typeof(ConfigurationModel));
            using StreamWriter writer = new(filePath);
            serializer.Serialize(writer, config);
        }

        public static ConfigurationModel? LoadConfig(string dirPath, string filePath = "")
        {
            string appsDir = Path.Combine(dirPath, Properties.Resources.AppsDirName);
            try
            {
                if (dirPath.EndsWith(filePath) == false || string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = Path.Combine(dirPath, Properties.Resources.ConfigFileName + ".xml");
                }
                else
                {
                    filePath = dirPath;
                }
                if (FileHelper.Exists(filePath))
                {
                    XmlSerializer serializer = new(typeof(ConfigurationModel));
                    using StreamReader reader = new(filePath);
                    var config = serializer.Deserialize(reader) as ConfigurationModel;
                    if(config != null)
                    {
                        config.DefaultConfig.AppsDir = appsDir;
                    }
                    return config;
                }
                else
                {
                    var defaultConfig = new ConfigurationModel(appsDir);
                    SaveConfig(defaultConfig, filePath);
                    return defaultConfig;
                }
            }
            catch (Exception ex)
            {
                var config = new ConfigurationModel(appsDir);
                LogHelper.WriteLine(ex, config, LogginLevel.ERROR);
                return config;
            }
        }
    }
}
