using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel;
using System.Configuration;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.Managers.Tasks
{
    public class ManagersInitializerTask : IBackgroundTask, IDisposableExtended
    {
        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public bool MustBeCollected => false;

        public bool Completed { get; set; }
        public bool IsDisposed { get; set; }

        private object dbLocker => new object();

        private DBContext context;
        public DBContext Context
        {
            get
            {
                lock(dbLocker)
                {
                    if (context == null || context.IsDisposed)
                        context = new DBContext();

                    return context;
                }
            }
        }

        public void Collect()
        {
        }

        public void Dispose()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
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
            Completed = false;
            FileManager.Initialize();

            try
            {
                CreateManager();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
            Completed = true;
        }

        private void CreateManager()
        {
            // Create task menagera
            using (var context = Context)
            {
                var managerID = ConfigurationManager.AppSettings["TasksManager"].ToString();
                var manager = context.Accounts.Find(managerID);
                if (manager == null)
                {
                    manager = new AitAccountModel(context)
                    {
                        ID = managerID,
                        Login = "taskmanager",
                        Email = ConfigurationManager.AppSettings["AppEmail"].ToString(),
                        Permition = PermitionAccountEnum.MANAGER,
                        IsActive = false
                    };
                    manager.Insert();
                    //context.SaveChanges();
                }
            }
        }
    }
}
