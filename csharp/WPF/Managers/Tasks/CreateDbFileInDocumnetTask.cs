using System;
using System.ComponentModel;
using WPF.Models.Interfaces;

namespace WPF.Managers.Tasks
{
    public class CreateDbFileInDocumnetTask : IBackgroundTask, IDisposableExtended
    {
        public bool Completed { get; set; }

        public bool MustBeCollected { get => true; }

        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();
        public bool IsDisposed { get; set; }

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
