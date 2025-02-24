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

        public class WebServiceFixes
        {
            static string _DefFullVerBegin = "Full Version: <b>";
            static string _DefFullVerEnd = "-UNSIGNED</b>";
            static string _DefCurVerBegin = "Version: <b>";
            static string _DefCurVerEnd = "</b>";
            static string _DefHFstamp = " -HF";
            static string _DefRevBegin = "SVN Revision: <b>";
            static string _DefRevEnd = "</b> ";

            public string FullVerBegin { get; set; } = _DefFullVerBegin;
            public string FullVerEnd { get; set; } = _DefFullVerEnd;
            public string CurVerBegin { get; set; } = _DefCurVerBegin;
            public string CurVerEnd { get; set; } = _DefCurVerEnd;
            public string HFstamp { get; set; } = _DefHFstamp;
            public string RevBegin { get; set; } = _DefRevBegin;
            public string RevEnd { get; set; } = _DefRevEnd;
        }

        public int RefreshTimerInterval { get; set; }
        public string AppsDir { get; set; }
        public LogginLevel EnableLogging { get; set; }
        public int Timeout { get; set; }
        public int DefaulPort { get; set; }
        public EnviromentPrefix EnvPrefix { get; set; }
        public string WebServicesUrl { get; set; }
        public List<EnviromentUrl> EnvList { get; set; }
        public WebServiceFixes WebServiceInfo { get; set; }

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
            WebServiceInfo = new WebServiceFixes();
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
        NONE = 0,
        ERROR = 1,
        WARNING = 2,
        INFO = 3,
        SHOW = 4
    }
}
