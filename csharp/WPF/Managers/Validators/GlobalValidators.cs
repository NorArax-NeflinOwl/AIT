using System.Linq;
using WPF.Databases.Contexts;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.Managers.Validators
{
    public class GlobalValidators
    {
        public static bool CheckNumbersInLotto(string winString, out int hits)
        {
            hits = 0;

            using (var context = PDBContext.Instance.Context)
            {
                var files = context.Files.ToList();
                foreach(var file in files)
                {
                    if(!string.IsNullOrEmpty(file.Content) && FileTypesEnum.TASK.Equals(file.Type))
                    {
                        var logInfo = CryptoJsonManager.Instance.Deserialize<LogInfoModel>(file.Content);
                        if (logInfo != null && logInfo.Message is IMessageInfo info && info.Message.Contains(winString))
                            return false;
                    }
                }
            }

            var winNumbers = winString.Split(',');
            string[] searchNumbers = { "4", "6", "17", "21", "24", "34" };

            for (var i = 0; i < 6; i++)
                for(var j = 0; j < 6; j++)
                {
                    if (searchNumbers[i].Equals(winNumbers[j]))
                        hits++;
                }

            return hits >= 3;
        }
    }
}
