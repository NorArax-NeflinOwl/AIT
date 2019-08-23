using System;
using System.ComponentModel;
using WPF.Databases.Contexts;
using WPF.Models.Interfaces;

namespace WPF.Managers.Tasks
{
    public class CreateDbFileInDocumnetTask : IBackgroundTask, IDisposableExtended
    {
        public bool Completed { get; set; }

        public bool MustBeCollected { get => true; }

        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();
        public bool IsDisposed { get; set; }

        private object dbLocker => new object();

        private DBContext context;
        public DBContext Context
        {
            get
            {
                lock (dbLocker)
                {
                    if (context == null || context.IsDisposed)
                        context = new DBContext();

                    return context;
                }
            }
        }

        public void Collect()
        {
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Completed = false;
            // TODO 
            Completed = true;
        }

        public void Dispose()
        {
            IsDisposed = true;
            GC.Collect();
        }

        public void Run()
        {
            Completed = true;
        }
    }
}
