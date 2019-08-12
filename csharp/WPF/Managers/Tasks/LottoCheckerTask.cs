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

        public readonly string Title = "Congratulation You hit the six in a Lotto!!!";
        public readonly string Message = "Congratulation Your lucky six \"{0}\" numbers win a award. {1}/6 hits!";

        public BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public bool MustBeCollected => false;

        public bool Completed { get; set; }

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
            Completed = false;
            var find = false;
            try
            {
                using (WebClient client = new WebClient())
                {
                    var content = client.DownloadString(link);

                    if (!string.IsNullOrEmpty(content))
                    {
                        List<LottoInfoModel> lottoResults = LottoInfoModel.Convert(content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList().OrderByDescending(q => q).ToList());

                        DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1);
                        using(var context = PDBContext.Instance.Context)
                        {
                            var manager = context.Stsgenids.Where(q => q.ID.Equals(ConfigurationManager.AppSettings["TasksManager"].ToString())).FirstOrDefault();
                            if (manager != null && startDate > manager.Create)
                                startDate = manager.Create;
                        }
                        var smallLottoResult = lottoResults.Where(q => q.DateTime >= startDate).ToList();

                        var firstLoopTurn = true;
                        foreach (var lottoModel in smallLottoResult)
                        {
                            var hits = 0;
                            if (lottoModel != null && GlobalValidators.CheckNumbersInLotto(lottoModel.LuckyNumbers, out hits))
                            {
                                find = true;
                                using (var context = PDBContext.Instance.Context)
                                {
                                    var account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                    if (account != null)
                                    {
                                        MailSender.SendTo(account.Email, Title, string.Format(Message, lottoModel.LuckyNumbers, hits));
                                    }
                                    else
                                    {
                                        var taskToSave = new AitFilesModel(context)
                                        {
                                            ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                                            //Creator = ConfigurationManager.AppSettings["TasksManager"].ToString(),
                                            Name = nameof(LottoCheckerTask),
                                            Type = FileTypesEnum.TASK,
                                            Content = CryptoJsonManager.Instance.Serialize(new LogInfoModel
                                            {
                                                Type = FileTypesEnum.TASK,
                                                Message = new SimpleMessageInfoModel(Title + Environment.NewLine + string.Format(Message, lottoModel.LuckyNumbers, hits))
                                            })
                                        };

                                        if(firstLoopTurn && startDate == new DateTime(DateTime.Now.Year, 1, 1))
                                        {
                                            WaitForManager();
                                            firstLoopTurn = false;
                                        }

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
                LogManager.Instance.LogExceptionToFile(ex);
            }

            Completed = true;
            return find;
        }

        private void WaitForManager()
        {
            var id = ConfigurationManager.AppSettings["TasksManager"].ToString();
            var cnt = PDBContext.Instance.Context;
            while (cnt.Stsgenids.Where(q => q.ID.Equals(id)).FirstOrDefault() == null)
            {
                cnt.Dispose();
                Thread.Sleep(10);
                cnt = PDBContext.Instance.Context;
            }
            cnt.Dispose();
        }

        public void Collect()
        {
        }
    }
}
