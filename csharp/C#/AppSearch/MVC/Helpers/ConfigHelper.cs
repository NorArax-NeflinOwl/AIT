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
            try
            {
                if(dirPath.EndsWith(filePath) == false || string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = Path.Combine(dirPath, Properties.Resources.ConfigFileName + ".xml");
                }
                else
                {
                    filePath = dirPath;
                }

                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new(typeof(ConfigurationModel));
                    using StreamReader reader = new(filePath);
                    return serializer.Deserialize(reader) as ConfigurationModel;
                }
                else
                {
                    var defaultConfig = new ConfigurationModel(dirPath);
                    SaveConfig(defaultConfig, filePath);
                    return defaultConfig;
                }
            }
            catch (Exception ex)
            {
                var config = new ConfigurationModel(dirPath);
                LogHelper.WriteLine(ex, config, LogginLevel.ERROR);
                return config;
            }
        }
    }
}
