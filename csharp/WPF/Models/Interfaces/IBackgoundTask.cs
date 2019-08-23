using System.ComponentModel;
using WPF.Databases.Contexts;

namespace WPF.Models.Interfaces
{
    public interface IBackgroundTask
    {
        bool Completed { get; set; }
        bool MustBeCollected { get; }
        BackgroundWorker BackgroundWorker { get; }
        DBContext Context { get; }

        void Run();
        void Collect();
    }
}
