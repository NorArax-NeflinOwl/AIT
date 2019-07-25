using Newtonsoft.Json;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Models;

namespace WPF.Managers.Validators
{
    public class GlobalValidators
    {
        private static readonly string[] searchNumbers = { "4", "6", "17", "21", "24", "34" };

        private static readonly JsonSerializerSettings m_Setting = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static bool CheckNumbersInLotto(string winString, out int hits)
        {
            hits = 0;

            using (var context = PDBContext.Instance.Context)
            {
                var file = context.Files.Where(q => JsonConvert.DeserializeObject<LogInfoModel>(q.Content, m_Setting).Message.Contains(winString));
                if(file != null)
                    return false;
            }

            var winNumbers = winString.Split(',');

            for(var i = 0; i < 6; i++)
                for(var j = 0; j < 6; j++)
                {
                    if (searchNumbers[i].Equals(winNumbers[j]))
                        hits++;
                }

            return hits >= 3;
        }
    }
}
