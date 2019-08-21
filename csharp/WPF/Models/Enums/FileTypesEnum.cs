using System.Collections.Generic;

namespace WPF.Models.Enums
{
    public enum FileTypesEnum
    {
        EXCEPTION,
        INFORMATION,
        NOTE,
        TRACE,
        QUERY,
        TASK,
        KEYLOGGER,
        ACTIVATION_CODE
    }

    public class FileTypesManager
    {
        public static IList<string> Content = new List<string>
        {
            Properties.Resources.EXCEPTION,
            Properties.Resources.INFORMATION,
            Properties.Resources.NOTE,
            Properties.Resources.TRACE,
            Properties.Resources.QUERY,
            Properties.Resources.TASK,
            Properties.Resources.KEYLOGGER,
            Properties.Resources.ACTIVATION_CODE
        };

        public static FileTypesEnum? SetType(int index)
        {
            switch(index)
            {
                case 0:
                    return FileTypesEnum.EXCEPTION;
                case 1:
                    return FileTypesEnum.INFORMATION;
                case 2:
                    return FileTypesEnum.NOTE;
                case 3:
                    return FileTypesEnum.TRACE;
                case 4:
                    return FileTypesEnum.QUERY;
                case 5:
                    return FileTypesEnum.TASK;
                case 6:
                    return FileTypesEnum.KEYLOGGER;
                case 7:
                    return FileTypesEnum.ACTIVATION_CODE;

            }
            return null;
        }
    }
}
