using MunchkinLib.Models;
using MunchkinLib.Models.Source;

namespace MunchkinLib.Helpers
{
    public class CustomConverter
    {
        public static string ConvertTypeToString(ICard card)
        {
            return DoorTypeToString(card.CardType);
        }

        private static string DoorTypeToString(CardTypeFlags type)
        {
            if (type.HasFlag(CardTypeFlags.Class))
            {
                return "Klasa";
            }
            if (type.HasFlag(CardTypeFlags.Race))
            {
                return "Rasa";
            }
            if (type.HasFlag(CardTypeFlags.Fraction))
            {
                return "Frakcja";
            }
            if (type.HasFlag(CardTypeFlags.Curse))
            {
                return "Klątwa!";
            }
            if (type.HasFlag(CardTypeFlags.Monster))
            {
                return "Potwór!";
            }
            if (type.HasFlag(CardTypeFlags.Special))
            {
                return "Specjalna";
            }
            return string.Empty;
        }

        public static string MembershipCardsToString(CardTypeFlags type)
        {
            if (type.HasFlag(CardTypeFlags.Class))
            {
                return "Klasa";
            }
            if (type.HasFlag(CardTypeFlags.Race))
            {
                return "Rasa";
            }
            if (type.HasFlag(CardTypeFlags.Fraction))
            {
                return "Frakcja";
            }
            return string.Empty;
        }

        public static string GetCardStringWeaponType(CardFlags flag)
        {
            if (flag.HasFlag(CardFlags.IsHead))
            {
                return MunchkinGlobals.Instance.CardsInfoParts[CardFlags.IsHead];
            }
            if (flag.HasFlag(CardFlags.IsArmor))
            {
                return MunchkinGlobals.Instance.CardsInfoParts[CardFlags.IsArmor];
            }
            if (flag.HasFlag(CardFlags.IsOneHandWeaponLeft))
            {
                return MunchkinGlobals.Instance.CardsInfoParts[CardFlags.IsOneHandWeaponLeft];
            }
            if (flag.HasFlag(CardFlags.IsOneHandWeaponRight))
            {
                return MunchkinGlobals.Instance.CardsInfoParts[CardFlags.IsOneHandWeaponRight];
            }
            return string.Empty;
        }
    }
}
