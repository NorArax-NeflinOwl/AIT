using System.Collections.Generic;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Managers.Helpers;
using WPF.Managers.Validators;
using WPF.Models.Enums;
using WPF.Models.Extensions.Exceptions;

namespace WPF.Managers.Builders
{
    public class EntryIdentificatorBuilder
    {

        private static readonly char separator = '-';

        private static readonly List<string> UsedByNotSavedIDs = new List<string>();

        public static string RecordIDGenerator(TableInerfixEnum tablePrefix)
        {
            return RecordIDGenerator(SystemPrefixEnum.AIT, tablePrefix);
        }

        public static string RecordIDGenerator(SystemPrefixEnum systemPrefix, TableInerfixEnum tablePrefix)
        {
            var id = "0000000";

            if (UsedByNotSavedIDs.Any())
            {
                var oldid = int.Parse(UsedByNotSavedIDs.Last().Split(separator)[2]);
                id = Converters.Digit2StringCreate(oldid + 1, 7);
            }
            else
            {
                using (var context = PDBContext.Instance.Context)
                {
                    var sysid = context.Stsgenids.OrderByDescending(q => q.ID.Split(separator)[2]).ToList().FirstOrDefault();
                    if (sysid != null)
                    {
                        var oldid = int.Parse(sysid.ID.Split(separator)[2]);
                        id = Converters.Digit2StringCreate(oldid + 1, 7);
                    }
                    else
                    {
                        LogManager.Instance.LogExceptionToFileAndDB(new SqliteExceptions.EntityNotFound("Stsgenids table is empty"));
                    }
                }
            }

            var newid = systemPrefix.ToString() + separator + tablePrefix.ToString() + separator + id;
            if (BasePropertiesValidator.ValidateID(newid))
            {
                UsedByNotSavedIDs.Add(newid);
                return newid;
            }
            else
                throw new BaseExceptions.IDException("Incorect id generate");
        }

        public static void ClearLocalIDs()
        {
            UsedByNotSavedIDs.Clear();
        }
    }
}
