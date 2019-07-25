using System.Collections.Generic;
using WPF.Interfaces;
using WPF.Managers.Tasks;

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

        public void Initialize()
        {
            tasks.Add(new LottoCheckerTask());
            tasks.Add(new ManagersInitializerTask());

            Start();
        }

        private void Start()
        {
            foreach(var task in tasks)
            {
                task.Run();
            }
        }
    }
}
