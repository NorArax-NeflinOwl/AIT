using AIT.Enums;

namespace AIT.Helpers
{
    public static class aitTypesResources
    {
        public const string DATE = "Date";
        public const string DATE_TIME = "Date and time";
        public const string TIME_NO_DATE = "Only time";
        public const string NO_DATE = "No date";

        public const string SINGLE_NOTE_MODEL = "Single note";
        public const string TREE_VIEW_MODEL = "Tree note";
        public const string TRAINING_MODEL = "Training note";

        public const string PUMP = "Pump";
        public const string SQUATS = "Squats";
        public const string DUMBBELL = "Dumbbell";
        public const string BENCH = "Bench";
        public const string BIKE_TIME = "Bike (time)";
        public const string BIKE_KM = "Bike (km)";
        public const string STEPS = "Steps";

        public const string MWU = "Morning first action after wake up";
        public const string MMT = "Morning first action after morning toilet";
        public const string MBB = "Morning before breakfast";
        public const string MAB = "Morning after breakfast";
        public const string MBL = "Midday before lunch";
        public const string MAL = "Midday after lunch";
        public const string ABD = "Afternoom before dinner";
        public const string AAD = "Afternoom after dinner";
        public const string NBB = "Night before bath";
        public const string NAB = "Night after bath";
        public const string NBS = "Night before sleep";

    }

    public static class aitTypesConverterHelper
    {
        public static aitDateTimeTypesModel DateTimeTypesConverter(string type)
        {
            switch(type)
            {
                case aitTypesResources.DATE:
                    return aitDateTimeTypesModel.DATE;
                case aitTypesResources.DATE_TIME:
                    return aitDateTimeTypesModel.DATE_TIME;
                case aitTypesResources.TIME_NO_DATE:
                    return aitDateTimeTypesModel.TIME_NO_DATE;
                case aitTypesResources.NO_DATE:
                    return aitDateTimeTypesModel.NO_DATE_TIME;
            }
            return aitDateTimeTypesModel.DATE;
        }

        public static aitNotesTypesModel NotesTypesConverter(string type)
        {
            switch (type)
            {
                case aitTypesResources.SINGLE_NOTE_MODEL:
                    return aitNotesTypesModel.SINGLE_NOTE_MODEL;
                case aitTypesResources.TREE_VIEW_MODEL:
                    return aitNotesTypesModel.TREE_VIEW_MODEL;
                case aitTypesResources.TRAINING_MODEL:
                    return aitNotesTypesModel.TRAINING_MODEL;
            }
            return aitNotesTypesModel.SINGLE_NOTE_MODEL;
        }

        public static aitTrainingTypes TrainigTypesConverter(string type)
        {
            switch (type)
            {
                case aitTypesResources.PUMP:
                    return aitTrainingTypes.PUMP;
                case aitTypesResources.SQUATS:
                    return aitTrainingTypes.SQUATS;
                case aitTypesResources.DUMBBELL:
                    return aitTrainingTypes.DUMBBELL;
                case aitTypesResources.BENCH:
                    return aitTrainingTypes.BENCH;
                case aitTypesResources.BIKE_TIME:
                    return aitTrainingTypes.BIKE_TIME;
                case aitTypesResources.BIKE_KM:
                    return aitTrainingTypes.BIKE_KM;
                case aitTypesResources.STEPS:
                    return aitTrainingTypes.STEPS;
            }
            return aitTrainingTypes.UNSPECIFIED;
        }

        public static aitTimeOfDay TimeOfDayConverter(string type)
        {
            switch (type)
            {
                case aitTypesResources.MWU:
                    return aitTimeOfDay.MWU;
                case aitTypesResources.MMT:
                    return aitTimeOfDay.MMT;
                case aitTypesResources.MBB:
                    return aitTimeOfDay.MBB;
                case aitTypesResources.MAB:
                    return aitTimeOfDay.MAB;
                case aitTypesResources.MBL:
                    return aitTimeOfDay.MBL;
                case aitTypesResources.MAL:
                    return aitTimeOfDay.MAL;
                case aitTypesResources.ABD:
                    return aitTimeOfDay.ABD;
                case aitTypesResources.AAD:
                    return aitTimeOfDay.AAD;
                case aitTypesResources.NBB:
                    return aitTimeOfDay.NBB;
                case aitTypesResources.NAB:
                    return aitTimeOfDay.NAB;
                case aitTypesResources.NBS:
                    return aitTimeOfDay.NBS;
            }
            return aitTimeOfDay.UNF;
        }
    }
}
