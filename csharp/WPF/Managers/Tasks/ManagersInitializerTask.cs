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
    public class ManagersInitializerTask : IBackgroundTask, IDisposable
    {
        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public void Dispose()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
            GC.Collect();
        }

        public void Run()
        {
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            FileManager.Initialize();

            try
            {
                CreateManager();
            }
            catch(SqliteException)
            {
                using (var context = PDBContext.Instance.Context)
                {
                    context.ReCreate();
                }
                CreateManager();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
        }

        private void CreateManager()
        {
            // Create task menagera
            using (var context = PDBContext.Instance.Context)
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
                    context.SaveChanges();
                }
            }
        }
    }
}
