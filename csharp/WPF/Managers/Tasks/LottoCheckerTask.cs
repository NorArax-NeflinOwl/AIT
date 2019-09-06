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
        public readonly string Message = "Congratulation Your lucky six \"{0}\" numbers win a award in \"{1}\" lottery - \"{2}\". {3}/6 hits! \nWining numners is: {4}";
        public readonly string FileMessage = "Winning number";
        public readonly string LottoLuckyNumberListItemMessage = "Lucky six \"{0}\" numbers win a award in \"{1}\" lottery - \"{2}\". {3}/6 hits! Wining numners is: {4}";

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
                    await Task.Delay(TimeSpan.FromDays(intervalDay)); //Sleep for [interval] days
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

                        foreach (var lottoModel in smallLottoResult)
                        {
                            var hits = 0;
                            var userLuckyNumbers = new List<string>();
                            if (lottoModel != null && GlobalValidators.CheckNumbersInLotto(lottoModel.WiningNumbers, out hits, out userLuckyNumbers))
                            {
                                find = true;
                                using (var context = Context)
                                {
                                    var account = context.Accounts.Where(q => q.ID.Equals(PDBContext.Instance.AccountID)).FirstOrDefault();
                                    if (account != null)
                                    {
                                        MailSender.SendTo(account.Email, Title, string.Format(Message,
                                                    lottoModel.WiningNumbers,
                                                    lottoModel.ID.Replace(".", string.Empty),
                                                    lottoModel.Date.ToString("dd/MM/yyyy"),
                                                    hits, ConvertList2String(userLuckyNumbers)));
                                    }
                                    else
                                    {
                                        AitAccountModel fileCreator = null;
                                        var creator = ConfigurationManager.AppSettings["TasksManager"].ToString();
                                        if (string.IsNullOrEmpty(creator))
                                        {
                                            fileCreator = context.Accounts.Where(q => q.ID.Equals(creator)).FirstOrDefault();
                                        }

                                        var lottoFiles = context.Files.Where(q => q.Create > DateTime.Now.Date && nameof(LottoCheckerTask).Equals(q.Name)).ToList();
                                        if(lottoFiles == null || !lottoFiles.Any())
                                        {
                                            var taskToSave = new AitFileModel(context)
                                            {
                                                ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS),
                                                FileCreator = fileCreator ?? null,
                                                Name = nameof(LottoCheckerTask),
                                                Type = FileTypesEnum.LOTTO_NOTE,
                                                Content = CryptoJsonManager.Instance.Serialize(new MessageInfoModel(FileMessage)
                                                {
                                                    Array = ConvertNewItemListToMessage(lottoModel, hits, userLuckyNumbers)
                                                })
                                            };

                                            taskToSave.Insert();
                                        }
                                        else
                                        {
                                            foreach(var lottoFile in lottoFiles.GroupBy(q => q.AssignedTo).Select(group => group.First()).ToList())
                                            {
                                                var obj = CryptoJsonManager.Instance.Deserialize<MessageInfoModel>(lottoFile.Content);
                                                if(obj != null)
                                                {
                                                    obj.Array = ConvertExistsItemListToMessage(lottoModel, hits, userLuckyNumbers, obj.Array.ToList());
                                                    lottoFile.Content = CryptoJsonManager.Instance.Serialize(obj);
                                                    lottoFile.Update();
                                                }
                                            }
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

        private string[] ConvertNewItemListToMessage(LottoInfoModel lottoInfo, int hits, List<string> list)
        {
            return ConvertExistsItemListToMessage(lottoInfo, hits, list, new List<string>());
        }

        private string[] ConvertExistsItemListToMessage(LottoInfoModel lottoInfo, int hits, List<string> list, List<string> newList)
        {
            foreach(var item in list)
            {
                newList.Add(string.Format(LottoLuckyNumberListItemMessage,
                                          lottoInfo.WiningNumbers,
                                          lottoInfo.ID.Replace(".", string.Empty),
                                          lottoInfo.Date.ToString("dd/MM/yyyy"),
                                          hits, item));
            }
            return newList.Distinct().ToArray();
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

        private static string ConvertList2String(List<string> tab)
        {
            var result = "[";
            var index = 0;
            foreach (var value in tab)
            {
                result += index > 0 ? "]; [" + value : "" + value;
                index++;
            }
            result += "]";

            return result;
        }

        public void Collect()
        {
        }
    }
}
