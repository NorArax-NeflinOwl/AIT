namespace MunchkinLib.Models.Source
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
        public static int MinimalPriceCard = 100;
        public static int MinimalPriceForNewLevel = 1000;
        public static int MoneyStart = 0;
    }

    public class LevelProperties
    {
        public static int MIN = 1;
        public static int START = 1;
        public static int MAX = GameProperties.MaxLevel;
    }
}
