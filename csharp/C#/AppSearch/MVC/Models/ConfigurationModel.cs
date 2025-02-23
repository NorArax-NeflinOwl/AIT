using System.IO;

namespace AppSearch.MVC.Models
{
    [Serializable]
    public class ConfigurationModel
    {
        public class EnviromentPrefix
        {
            public required string WindowsPrefix { get; set; }
            public required string LinuxPrefix { get; set; }
        }

        public class EnviromentUrl
        {
            public required string EnvName { get; set; }
            public required string Url { get; set; }
        }

        public int RefreshTimerInterval { get; set; }
        public string AppsDir { get; set; }
        public LogginLevel EnableLogging { get; set; }
        public int Timeout { get; set; }
        public int DefaulPort { get; set; }
        public EnviromentPrefix EnvPrefix { get; set; }
        public string WebServicesUrl { get; set; }
        public List<EnviromentUrl> EnvList { get; set; }

        public ConfigurationModel()
        {
            RefreshTimerInterval = 60;
            Timeout = 500;
            DefaulPort = 7700;
            EnvPrefix = new()
            {
                WindowsPrefix = Properties.Resources.WindowsUrlPrefix,
                LinuxPrefix = Properties.Resources.LinuxUrlPrefix
            };
            WebServicesUrl = Properties.Resources.DefaultWebServicesUrl;
            EnvList = [];
        }

        public ConfigurationModel(string appsDir) : this()
        {
            AppsDir = appsDir;
        }

        public ConfigurationModel(string appsDir, int refreshTimerInterval)
        {
            AppsDir = appsDir;
            RefreshTimerInterval = refreshTimerInterval;
        }

        public string GetAppsPath()
        {
            return Path.Combine(AppsDir, Properties.Resources.AppsDirName);
        }
    }

    public enum LogginLevel
    {
        NONE,
        ERROR,
        WARNING,
        INFO
    }
}
