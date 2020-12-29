using MunchkinLib.Models;

namespace MunchkinLib.Helpers
{
    public class CustomConverter
    {
        public static string ConvertTypeToString(BaseCard card)
        {
            if(card is CardDoor door)
            {
                return DoorTypeToString(door.Type);
            }

            return string.Empty;
        }

        private static string DoorTypeToString(DoorType type)
        {
            switch (type)
            {
                case DoorType.Class:
                    return "Klasa";
                case DoorType.Race:
                    return "Rasa";
                case DoorType.Fraction:
                    return "Frakcja";
                case DoorType.Curse:
                    return "Klątwa!";
                case DoorType.Monster:
                    return "Potwór!";
                case DoorType.Special:
                    return "Specjalna";
                default:
                    return string.Empty;
            }
        }

        public static string MembershipCardsToString(DoorType type)
        {
            switch (type)
            {
                case DoorType.Class:
                    return "Klasa";
                case DoorType.Race:
                    return "Rasa";
                case DoorType.Fraction:
                    return "Frakcja";
                default:
                    return string.Empty;
            }
        }
    }
}
