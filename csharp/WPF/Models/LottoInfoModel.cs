using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace WPF.Models
{
    public class LottoInfoModel
    {
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public string WiningNumbers { get; set; }

        public static List<LottoInfoModel> Convert(List<string> list)
        {
            List<LottoInfoModel> lottoInfoModels = new List<LottoInfoModel>();
            foreach (var line in list)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split(' ').ToList();
                    if (parts.Count != 3)
                        throw new IndexOutOfRangeException();

                    int idLength = parts[0].Trim().Length;
                    var model = new LottoInfoModel
                    {
                        ID = parts[0].Trim().Remove(idLength - 1, 1),
                        Date = DateTime.ParseExact(parts[1], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        WiningNumbers = parts[2]
                    };
                    lottoInfoModels.Add(model);
                }
            }
            return lottoInfoModels;
        }
    }
}
