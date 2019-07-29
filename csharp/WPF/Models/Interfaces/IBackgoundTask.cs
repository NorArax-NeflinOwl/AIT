using System.ComponentModel;

namespace WPF.Models.Interfaces
{
    public interface IBackgroundTask
    {
        bool Completed { get; set; }
        bool MustBeCollected { get; }
        BackgroundWorker BackgroundWorker { get; }

        void Run();
        void Collect();
    }
}
