using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;
using WPF.Managers.Helpers;
using WPF.Models;
using WPF.Managers.Validators;

namespace WPF.Managers.Tasks
{
    public class LottoCheckerTask : IBackgroundTask, IDisposable
    {
        private readonly string link = "http://www.mbnet.com.pl/dl.txt";
        private readonly int intervalDay = 3;
        private readonly JsonSerializerSettings m_Setting = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public readonly string Title = "Congratulation You hit the six in a Lotto!!!";
        public readonly string Message = "Congratulation Your lucky six \"{0}\" numbers win a award. {1}/6 hits!";

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
            while(true)
            {
                CheckLotto();
                Thread.Sleep(intervalDay * 24 * 3600000); //Sleep for [interval] days
            }
        }

        public bool CheckLotto()
        {
            var find = false;
            try
            {
                using (WebClient client = new WebClient())
                {
                    var content = client.DownloadString(link);

                    if (!string.IsNullOrEmpty(content))
                    {
                        List<string> lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList().OrderByDescending(q => q).ToList();

                        foreach (var line in lines)
                        {
                            var hits = 0;
                            var parts = line.Split(' ').ToList();
                            if (parts != null && parts.Count == 3 && GlobalValidators.CheckNumbersInLotto(parts[2], out hits))
                            {
                                find = true;
                                using (var context = PDBContext.Instance.Context)
                                {
                                    var account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                    if (account != null)
                                    {
                                        MailSender.SendTo(account.Email, Title, string.Format(Message, parts[2], hits));
                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var taskToSave = new AitFilesModel(context)
                                        {
                                            ID = Generators.IDGenerator(IDInerfixEnum.FLS),
                                            Creator = ConfigurationManager.AppSettings["TasksManager"].ToString(),
                                            Name = FileTypesEnum.TASK.ToString() + (random.Next() % 1000).ToString(),
                                            Type = FileTypesEnum.TASK.ToString(),
                                            Content = JsonConvert.SerializeObject(new LogInfoModel
                                            {
                                                Type = FileTypesEnum.TASK,
                                                Message = Title + Environment.NewLine + string.Format(Message, parts[2], hits)
                                            })
                                        };

                                        taskToSave.Insert();
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogExceptionToFile(ex);
            }

            return find;
        }
    }
}
