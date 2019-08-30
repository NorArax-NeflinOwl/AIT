using System;
using System.Collections.Generic;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models;
using WPF.Models.Enums;

namespace WPF.Managers.Validators
{
    public class GlobalValidators
    {
        public static bool CheckNumbersInLotto(string winString, out int hits, out List<string> winingNumbers)
        {
            hits = 0;
            winingNumbers = new List<string>();

            var lottoUserLuckyNumbers = new List<AitFileModel>();
            using (var context = PDBContext.Instance.Context)
            {
                var files = context.Files.ToList();
                foreach (var file in files)
                {
                    if (!string.IsNullOrEmpty(file.Content) && FileTypesEnum.LOTTO_NOTE.Equals(file.Type))
                    {
                        var logInfo = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(file.Content, false);
                        if (logInfo != null && logInfo.MessageInfo is MessageInfoModel info
                            && string.IsNullOrEmpty(info.Message) && info.Message.Contains(winString))
                            return false;
                    }
                }

                if (!string.IsNullOrEmpty(PDBContext.Instance.AccountID))
                {
                    lottoUserLuckyNumbers = context.Files.Where(q => !string.IsNullOrEmpty(q.Content) && FileTypesEnum.LOTTO_NOTE.Equals(q.Type)
                                                                     && q.AssignedTo.Equals(PDBContext.Instance.AccountID)).ToList();
                }
            }

            List<string[]> searchTab = new List<string[]>
            {
                new string[] { "4", "6", "17", "21", "24", "34" },
            };

            if(lottoUserLuckyNumbers.Any())
            {
                foreach(var file in lottoUserLuckyNumbers)
                {
                    if(file.Content.StartsWith("[{"))
                    {
                        var content = CryptoJsonManager.Instance.Deserialize<List<LogInfoModel>>(file.Content);
                        foreach (var item in content)
                        {
                            foreach (var element in item.MessageInfo.Array)
                            {
                                searchTab.Add(element.Replace(" ", string.Empty).Split(','));
                            }
                        }
                    }
                    else
                    {
                        var content = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(file.Content);
                        foreach(var item in content.MessageInfo.Array)
                        {
                            searchTab.Add(item.Replace(" ", string.Empty).Split(','));
                        }
                    }
                }
            }

            var winNumbers = winString.Split(',');

            for(var i = 0; i < searchTab.Count; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    for (var k = 0; k < 6; k++)
                    {
                        if (searchTab[i][j].Equals(winNumbers[k]))
                            hits++;
                    }
                }

                if(hits >= 3)
                {
                    winingNumbers.Add(ConvertTab2String(searchTab[i]));
                }
            }

            return winingNumbers.Any();
        }

        private static string ConvertTab2String(string[] tab)
        {
            var result = string.Empty;
            var index = 0;
            foreach (var value in tab)
            {
                result += index > 0 ? ", " + value : "" + value;
                index++;
            }

            return result;
        }
    }
}
