using System.ComponentModel;

namespace WPF.Models.Interfaces
{
    public interface IBackgroundTask
    {
        BackgroundWorker BackgroundWorker { get; }

        void Run();
    }
}
