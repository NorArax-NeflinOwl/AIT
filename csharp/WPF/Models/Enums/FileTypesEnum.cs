using System.Collections.Generic;

namespace WPF.Models.Enums
{
    public enum FileTypesEnum
    {
        UNDEFINED,
        EXCEPTION,
        INFORMATION,
        NOTE,
        TRACE,
        QUERY,
        TASK,
        KEYLOGGER,
        ACTIVATION_CODE,
        LOTTO_NOTE
    }

    public class FileTypesManager
    {
        public static IList<string> Content = new List<string>
        {
            Properties.Resources.UNDEFINED,
            Properties.Resources.EXCEPTION,
            Properties.Resources.INFORMATION,
            Properties.Resources.NOTE,
            Properties.Resources.TRACE,
            Properties.Resources.QUERY,
            Properties.Resources.TASK,
            Properties.Resources.KEYLOGGER,
            Properties.Resources.ACTIVATION_CODE,
            Properties.Resources.LOTTO_NOTE
        };

        public static FileTypesEnum? SetType(int index)
        {
            switch(index)
            {
                case 0:
                    return FileTypesEnum.UNDEFINED;
                case 1:
                    return FileTypesEnum.EXCEPTION;
                case 2:
                    return FileTypesEnum.INFORMATION;
                case 3:
                    return FileTypesEnum.NOTE;
                case 4:
                    return FileTypesEnum.TRACE;
                case 5:
                    return FileTypesEnum.QUERY;
                case 6:
                    return FileTypesEnum.TASK;
                case 7:
                    return FileTypesEnum.KEYLOGGER;
                case 8:
                    return FileTypesEnum.ACTIVATION_CODE;
                case 9:
                    return FileTypesEnum.LOTTO_NOTE;

            }
            return null;
        }

        public static bool AllowToEmptyContent(FileTypesEnum? type)
        {
            if(type != null)
            {
                switch (type)
                {
                    case FileTypesEnum.UNDEFINED:
                        return true;
                    case FileTypesEnum.EXCEPTION:
                        return false;
                    case FileTypesEnum.INFORMATION:
                        return false;
                    case FileTypesEnum.NOTE:
                        return true;
                    case FileTypesEnum.TRACE:
                        return false;
                    case FileTypesEnum.QUERY:
                        return false;
                    case FileTypesEnum.TASK:
                        return true;
                    case FileTypesEnum.KEYLOGGER:
                        return false;
                    case FileTypesEnum.ACTIVATION_CODE:
                        return false;
                    case FileTypesEnum.LOTTO_NOTE:
                        return false;
                }
            }
            return false;
        }
    }
}
