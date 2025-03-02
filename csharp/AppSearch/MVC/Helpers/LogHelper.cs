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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        public static void Initialize(string mainDirPath)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
#pragma warning restore CS8604 // Possible null reference argument.
            GlobalContext.Properties["LogFileName"] = Path.Combine(mainDirPath, Properties.Resources.LoggerFileName);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static bool WriteErrorLine(object error)
        {
            try
            {
                logger.Error(error);
                return true;
            }
            catch(Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                return false;
            }
        }

        public static bool WriteLine(object message, ConfigurationModel config, LogginLevel logginLevel)
        {
            if(config.DefaultConfig.EnableLogging >= logginLevel)
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
            switch(config.DefaultConfig.EnableLogging)
            {
                case LogginLevel.SHOW:
                    if(message is Exception exception)
                        logger.Error(exception);
                    else
                        logger.Info(message);

                    MessageBox.Show(message.ToString(), Properties.Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Hand);
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
