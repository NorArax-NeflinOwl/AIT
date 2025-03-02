using AppSearch.MVC.Helpers;

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

        public class ExchangeName
        {
            public string From { get; set; }
            public string To { get; set; }

            public ExchangeName()
            {

            }

            public ExchangeName(string from, string to)
            {
                From = from;
                To = to;
            }
        }

        public class WebServiceFixes
        {
            static readonly string _DefFullVerBegin = "Full Version: <b>";
            static readonly string _DefFullVerEnd = "-UNSIGNED</b>";
            static readonly string _DefAppFullVerEnd = " <!-- HF_nr --></b>";
            static readonly string _DefCurVerBegin = "Version: <b>";
            static readonly string _DefCurVerEnd = "</b>";
            static readonly string _DefHFstamp = " -HF";
            static readonly string _DefRevBegin = "SVN Revision: <b>";
            static readonly string _DefRevEnd = "</b> ";

            public string FullVerBegin { get; set; } = _DefFullVerBegin;
            public string FullVerEnd { get; set; } = _DefFullVerEnd;
            public string AppFullVerEnd { get; set; } = _DefAppFullVerEnd;
            public string CurVerBegin { get; set; } = _DefCurVerBegin;
            public string CurVerEnd { get; set; } = _DefCurVerEnd;
            public string HFstamp { get; set; } = _DefHFstamp;
            public string RevBegin { get; set; } = _DefRevBegin;
            public string RevEnd { get; set; } = _DefRevEnd;
        }

        public class UserConfigProperties
        {
            public string AppsDir { get; set; }
            public string RafiTermPath { get; set; }
            public string SccUpdaterPath { get; set; }
            public List<EnviromentUrl> EnvList { get; set; }
            public List<ExchangeName> UnwantedPartsAppNames { get; set; }
            public UserConfigProperties()
            {
                EnvList = [];
                UnwantedPartsAppNames = [];
            }
        }

        public class ConfigProperties
        {
            public int Timeout { get; set; }
            public int DefaulPort { get; set; }
            public int RefreshTimerInterval { get; set; }
            public string AppsDir { get; set; }
            public LogginLevel EnableLogging { get; set; }
            public EnviromentPrefix EnvPrefix { get; set; }
            public WebServiceFixes WebServiceInfo { get; set; }

            public ConfigProperties()
            {
                Timeout = 500;
                DefaulPort = 7700;
                RefreshTimerInterval = 60;

                EnableLogging = LogginLevel.ERROR;
                EnvPrefix = new()
                {
                    WindowsPrefix = Properties.Resources.WindowsUrlPrefix,
                    LinuxPrefix = Properties.Resources.LinuxUrlPrefix
                };
                WebServiceInfo = new WebServiceFixes();
            }
        }

        public UserConfigProperties UserConfig { get; set; }
        public ConfigProperties DefaultConfig { get; set; }

        public ConfigurationModel()
        {
            UserConfig = new UserConfigProperties();
            DefaultConfig = new ConfigProperties();
        }

        public ConfigurationModel(string appsDir) : this()
        {
            UserConfig = new UserConfigProperties();
            DefaultConfig = new ConfigProperties();
            DefaultConfig.AppsDir = appsDir;
        }

        public string GetAppDir()
        {
            if (FileHelper.DirExists(UserConfig.AppsDir))
                return UserConfig.AppsDir;
            else
                return DefaultConfig.AppsDir;
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
