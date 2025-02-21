using System.IO;

namespace AppSearch.MVC.Models
{
    [Serializable]
    public class ConfigurationModel
    {
        /// <summary>
        /// How often refresh timer should be fired up? Default should be 60 seconds
        /// </summary>
        public int RefreshTimerInterval { get; set; }
        public string AppsDir { get; set; }
        public bool EnableLogging { get; set; }

        public ConfigurationModel()
        {
            RefreshTimerInterval = 60;
            EnableLogging = false;
        }

        public ConfigurationModel(string appsDir) : this()
        {
            AppsDir = appsDir;
        }

        public ConfigurationModel(string appsDir, int refreshTimerInterval, bool enableLoggin = false)
        {
            AppsDir = appsDir;
            RefreshTimerInterval = refreshTimerInterval;
            EnableLogging = enableLoggin;
        }

        public string GetAppsPath()
        {
            return Path.Combine(AppsDir, Properties.Resources.AppsDirName);
        }
    }
}
