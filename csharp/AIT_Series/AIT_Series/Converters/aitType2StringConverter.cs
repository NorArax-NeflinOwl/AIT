using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIT.Constant;

namespace AIT.Converters
{
    public class aitType2StringConverter
    {
        public static string NoteType2StringConverter(aitNotesTypesModel type)
        {
            switch (type)
            {
                case aitNotesTypesModel.SIMPLE_NOTE_MODEL:
                    return aitStrings.SIMPLE_NOTE;
                case aitNotesTypesModel.TREE_VIEW_MODEL:
                    return aitStrings.TREE_VIEW;
                case aitNotesTypesModel.TRAINING_MODEL:
                    return aitStrings.TRAINING_NOTE;
            }
            return string.Empty;
        }

        public static string DateType2StringConverter(aitDateTimeType type)
        {
            switch (type)
            {
                case aitDateTimeType.DATE:
                    return aitStrings.DATE;
                case aitDateTimeType.DATE_TIME:
                    return aitStrings.DATE_TIME;
                case aitDateTimeType.TIME_NO_DATE:
                    return aitStrings.TIME_NO_DATE;
                case aitDateTimeType.NO_DATE:
                    return aitStrings.NO_DATE;
            }
            return string.Empty;
        }

        public static string TrainingType2StringConverter(aitTrainingTypes type)
        {
            switch (type)
            {
                case aitTrainingTypes.PUMP:
                    return aitStrings.PUMP;
                case aitTrainingTypes.SQUATS:
                    return aitStrings.SQUATS;
                case aitTrainingTypes.DUMBBELL:
                    return aitStrings.DUMBBELL;
                case aitTrainingTypes.BENCH:
                    return aitStrings.BENCH;
                case aitTrainingTypes.BIKE:
                    return aitStrings.BIKE;
                case aitTrainingTypes.RUN:
                    return aitStrings.RUN;
                case aitTrainingTypes.WALK:
                    return aitStrings.WALK;
            }

            return string.Empty;
        }

        public static string TimeOfDay2StringConverter(aitTimeOfDay type)
        {
            switch (type)
            {
                case aitTimeOfDay.MWU:
                    return aitStrings.MWU;
                case aitTimeOfDay.MMT:
                    return aitStrings.MMT;
                case aitTimeOfDay.MBB:
                    return aitStrings.MBB;
                case aitTimeOfDay.MAB:
                    return aitStrings.MAB;
                case aitTimeOfDay.MBL:
                    return aitStrings.MBL;
                case aitTimeOfDay.MAL:
                    return aitStrings.MAL;
                case aitTimeOfDay.ABD:
                    return aitStrings.ABD;
                case aitTimeOfDay.AAD:
                    return aitStrings.AAD;
                case aitTimeOfDay.NBB:
                    return aitStrings.NBB;
                case aitTimeOfDay.NAB:
                    return aitStrings.NAB;
                case aitTimeOfDay.NBS:
                    return aitStrings.NBS;
            }

            return string.Empty;
        }
    }
}
