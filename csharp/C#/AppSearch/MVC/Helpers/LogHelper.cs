using AppSearch.MVC.Models;
using log4net;
using log4net.Config;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AppSearch.MVC.Helpers
{
    public class LogHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Initialize(string mainDirPath)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            GlobalContext.Properties["LogFileName"] = Path.Combine(mainDirPath, Properties.Resources.LoggerFileName);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static bool WriteLine(object message, ConfigurationModel config, LogginLevel logginLevel)
        {
            if(config.EnableLogging >= logginLevel)
            {
                return WriteLine(message, config);
            }
            return false;
        }

        private static bool WriteLine(object message, ConfigurationModel config)
        {
#if DEBUG
            Debug.WriteLine(message);
#endif
            switch(config.EnableLogging)
            {
                case LogginLevel.SHOW:
                    if(message is Exception exception)
                        logger.Error(exception);
                    else
                        logger.Info(message);

                    MessageBox.Show(message.ToString(), Properties.Resources.Warning);
                    break;
                case LogginLevel.INFO:
                    logger.Info(message);
                    break;
                case LogginLevel.WARNING:
                    logger.Warn(message);
                    break;
                case LogginLevel.ERROR:
                    logger.Error(message);
                    break;
                
            }
            return true;
        }
    }
}
