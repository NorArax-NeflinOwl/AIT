using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT_Lib.Constant;
using System.Diagnostics;
using AIT_Lib.Helpers;

namespace AIT_Lib.Converters
{
    public static class aitNumber2TypeCoverter
    {
        public static aitTrainingTypes Number2TrainingTypeConverter(int number)
        {
            try
            {
                return (aitTrainingTypes)number;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                aitExceptionsLogger.Instance.LogToFile(new aitLog { Title = aitLogTitle.EXCEPTION, Message = ex.ToString() });
                return aitTrainingTypes.UNSPECIFIED;
            }
        }

        public static aitTimeOfDay Number2TimeOfDayConverter(int number)
        {
            try
            {
                return (aitTimeOfDay)number;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                aitExceptionsLogger.Instance.LogToFile(new aitLog { Title = aitLogTitle.EXCEPTION, Message = ex.ToString() });
                return aitTimeOfDay.UNF;
            }
        }
    }
}
