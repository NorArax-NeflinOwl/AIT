using System;
using System.Collections.Generic;
using System.Linq;
using WPF.Models;
using WPF.Models.Enums;

namespace WPF.Managers
{
    public class FileTypesManager
    {
        public static IList<FileTypeModel> Types = new List<FileTypeModel>
        {
            new FileTypeModel { EnumType = FileTypesEnum.UNDEFINED, StringType = Properties.Resources.UNDEFINED, PermitionLevel = PermitionAccountEnum.NONE, AllowToCreate = false },
            new FileTypeModel { EnumType = FileTypesEnum.DETACHED, StringType = Properties.Resources.DETACHED, PermitionLevel = PermitionAccountEnum.NONE, AllowToCreate = false },

            new FileTypeModel { EnumType = FileTypesEnum.INFORMATION, StringType = Properties.Resources.INFORMATION, PermitionLevel = PermitionAccountEnum.SIMPLE },
            new FileTypeModel { EnumType = FileTypesEnum.NOTE, StringType = Properties.Resources.NOTE, PermitionLevel = PermitionAccountEnum.SIMPLE, AllowToEmptyContent = true },
            new FileTypeModel { EnumType = FileTypesEnum.LOTTO_NOTE, StringType = Properties.Resources.LOTTO_NOTE, PermitionLevel = PermitionAccountEnum.SIMPLE, AllowToEmptyContent = true },
            new FileTypeModel { EnumType = FileTypesEnum.TASK, StringType = Properties.Resources.TASK, PermitionLevel = PermitionAccountEnum.SIMPLE, AllowToEmptyContent = true },
            new FileTypeModel { EnumType = FileTypesEnum.SHEDULER, StringType = Properties.Resources.SHEDULER, PermitionLevel = PermitionAccountEnum.SIMPLE, AllowToEmptyContent = true },

            new FileTypeModel { EnumType = FileTypesEnum.ACTIVATION_CODE, StringType = Properties.Resources.ACTIVATION_CODE, PermitionLevel = PermitionAccountEnum.MANAGER, AllowToCreate = false },

            new FileTypeModel { EnumType = FileTypesEnum.TRACE, StringType = Properties.Resources.TRACE, PermitionLevel = PermitionAccountEnum.ADMIN, AllowToCreate = false },
            new FileTypeModel { EnumType = FileTypesEnum.EXCEPTION, StringType = Properties.Resources.EXCEPTION, PermitionLevel = PermitionAccountEnum.ADMIN, AllowToCreate = false },
            new FileTypeModel { EnumType = FileTypesEnum.QUERY, StringType = Properties.Resources.QUERY, PermitionLevel = PermitionAccountEnum.ADMIN, AllowToCreate = false },
            new FileTypeModel { EnumType = FileTypesEnum.KEYLOGGER, StringType = Properties.Resources.KEYLOGGER, PermitionLevel = PermitionAccountEnum.ADMIN, AllowToCreate = false }
        };

        public static FileTypeModel SetType(int index)
        {
            try
            {
                return Types.Where(q => q.EnumType.Equals((FileTypesEnum)index)).FirstOrDefault();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static FileTypeModel GetType(FileTypesEnum fileTypesEnum)
        {
            try
            {
                return Types.Where(q => q.EnumType.Equals(fileTypesEnum)).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool AccountHasPermitionToFile(PermitionAccountEnum permition, FileTypesEnum fileType)
        {
            return Types.Any(q => q.EnumType.Equals(fileType) && (int)q.PermitionLevel <= (int)permition);
        }
    }
}
