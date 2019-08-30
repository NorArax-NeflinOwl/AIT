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
using System.Threading.Tasks;
using WPF.Models.Extensions;

namespace WPF.Managers.Tasks
{
    public class LottoCheckerTask : IBackgroundTask, IDisposableExtended
    {
        private readonly string link = "http://www.mbnet.com.pl/dl.txt";
        private readonly int intervalDay = 3;

        public readonly string Title = "Congratulation You hit the six in a Lotto!!!";
        public readonly string Message = "Congratulation Your lucky six \"{0}\" numbers win a award in \"{1}\" lottery - \"{2}\". {3}/6 hits!";

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
                lock (dbLocker)
                {
                    if (context == null || context.IsDisposed)
                        context = new DBContext();

                    return context;
                }
            }
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
            DispatcherExtension.Invoke(async () =>
            {
                while (true)
                {
                    CheckLotto();
                    await Task.Delay(intervalDay * 24 * 3600000); //Sleep for [interval] days
                }
            });
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
                        using(var context = Context)
                        {
                            var manager = context.Stsgenids.Where(q => q.ID.Equals(ConfigurationManager.AppSettings["TasksManager"].ToString())).FirstOrDefault();
                            if (manager != null && startDate > manager.Create)
                                startDate = manager.Create;
                        }
                        var smallLottoResult = lottoResults.Where(q => q.Date >= startDate).ToList();

                        var firstLoopTurn = true;
                        foreach (var lottoModel in smallLottoResult)
                        {
                            var hits = 0;
                            if (lottoModel != null && GlobalValidators.CheckNumbersInLotto(lottoModel.LuckyNumbers, out hits))
                            {
                                find = true;
                                using (var context = Context)
                                {
                                    var account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                    if (account != null)
                                    {
                                        MailSender.SendTo(account.Email, Title, string.Format(Message, lottoModel.LuckyNumbers, hits));
                                    }
                                    else
                                    {
                                        var lottoFile = context.Files.Where(q => q.Create > DateTime.Now.Date).FirstOrDefault();
                                        if(lottoFile == null)
                                        {
                                            var msg = Title + Environment.NewLine + string.Format(Message,
                                                    lottoModel.LuckyNumbers,
                                                    lottoModel.ID.Replace(".", string.Empty),
                                                    lottoModel.Date.ToString("dd/MM/yyyy"),
                                                    hits);

                                            var taskToSave = new AitFileModel(context)
                                            {
                                                ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                                                FileCreator = context.Accounts.Where(q => q.ID.Equals(ConfigurationManager.AppSettings["TasksManager"].ToString())).FirstOrDefault(),
                                                Name = nameof(LottoCheckerTask),
                                                Type = FileTypesEnum.LOTTO_NOTE,
                                                Content = CryptoJsonManager.Instance.Serialize(new LogInfoModel
                                                {
                                                    MessageInfo = new MessageInfoModel(msg)
                                                })
                                            };

                                            if (firstLoopTurn && startDate == new DateTime(DateTime.Now.Year, 1, 1))
                                            {
                                                WaitForManager();
                                                firstLoopTurn = false;
                                            }

                                            taskToSave.Insert();
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFileAndDB(ex);
            }

            Completed = true;
            return find;
        }

        private void WaitForManager()
        {
            var id = ConfigurationManager.AppSettings["TasksManager"].ToString();
            var cnt = Context;
            while (cnt.Stsgenids.Where(q => q.ID.Equals(id)).FirstOrDefault() == null)
            {
                cnt.Dispose();
                Thread.Sleep(10);
                cnt = Context;
            }
            cnt.Dispose();
        }

        public void Collect()
        {
        }
    }
}
