using System.ComponentModel;

namespace WPF.Interfaces
{
    public interface IBackgroundTask
    {
        BackgroundWorker BackgroundWorker { get; }

        void Run();
    }
}
