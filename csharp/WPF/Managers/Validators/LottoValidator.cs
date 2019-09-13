using System.Collections.Generic;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models;
using WPF.Models.Enums;

namespace WPF.Managers.Validators
{
    public class LottoValidator
    {
        public static bool CheckNumbersInLotto(string winString, out int hits, out List<string> userLuckyNumber)
        {
            hits = 0;
            userLuckyNumber = new List<string>();

            var lottoUserLuckyNumbers = new List<AitFileModel>();
            using (var context = PDBContext.Instance.Context)
            {
                var files = context.Files.ToList();
                foreach (var file in files)
                {
                    if (!string.IsNullOrEmpty(file.Content) && FileTypesEnum.LOTTO_NOTE.Equals(file.Type))
                    {
                        var logInfo = CryptoJsonManager.Instance.Deserialize<MessageInfoModel>(file.Content, false);
                        if (logInfo != null && string.IsNullOrEmpty(logInfo.Message) && logInfo.Message.Contains(winString))
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
                new string[] { "3", "7", "13", "21", "34", "47" },
            };

            if(lottoUserLuckyNumbers.Any())
            {
                foreach(var file in lottoUserLuckyNumbers)
                {
                    if(file.Content.StartsWith("[{"))
                    {
                        var content = CryptoJsonManager.Instance.Deserialize<List<MessageInfoModel>>(file.Content);
                        foreach (var item in content)
                        {
                            foreach (var element in item.Array)
                            {
                                searchTab.Add(element.Replace(" ", string.Empty).Split(','));
                            }
                        }
                    }
                    else
                    {
                        var content = CryptoJsonManager.Instance.Deserialize<MessageInfoModel>(file.Content);
                        foreach(var item in content.Array)
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
                        // FIX ME - some error ;/
                        if (searchTab[i][j].Equals(winNumbers[k]))
                            hits++;
                    }
                }

                if(hits >= 4)
                {
                    userLuckyNumber.Add(ConvertTab2String(searchTab[i]));
                }
            }

            return userLuckyNumber.Any();
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
