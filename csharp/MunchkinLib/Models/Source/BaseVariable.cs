namespace MunchkinLib.Models
{
    public class GameProperties
    {
        public static int DoorCardNumber = 4;
        public static int TreasureCardNumber = 4;
        public static int MaxLevel = 10;
        public static int MinimunCardsCountInHand = 5;
        public static int MaxDiceScore = 6;
        public static int MinDiceScore = 1;
        public static int MinToWinThrowDice = 3;
        public static int MinimalPriceByCard = 100;
    }

    public class LevelProperties
    {
        public static int MIN = 0;
        public static int START = 1;
        public static int MAX = GameProperties.MaxLevel;
    }

    public enum DoorType
    {
        Unknown,
        Class,
        Race,
        Fraction,
        Special,
        Monster,
        Curse
    }
}
