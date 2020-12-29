using System.Collections.Generic;

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
        public static int MinimalPriceForNewLevel = 1000;
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

    public enum QueuePhase
    {
        OutOfRound,
        OpenDoor,
        FightPhase,
        FindProblem,
        FightPhaseAfterFindProblem,
        PickUpTreasure,
        GiveCardsForFree,
        ShowStack,
    }

    public enum PlayerActions
    {
        StartRound,      // Czy chcesz rozpocząć runde?
        OpenDoor,        // Otwórz Drzwi,
        Fight,           // Walcz z potworem (po otwarciu pokoju lub szukaj guza z potworem z ręki) / dostań kolątwą / dostań kartę,
        Escape,          // Rzuć kością aby uciec z walki
        FindTreasure,    // Przeszukaj pokój ze skarbem,
        UseCardsFromHand,// Połuż karty przed siebie na stół, użyj kart specjalnych, sprzedaj za poziom, oddaj karty innemu graczowi
        PickUpCard,      // Weź kartę do ręki
        NextPhase,       // Przejście do następnego etapu
        EndRound         // Koniec tury uzytkownika
    }

    public enum GeneralPlayerActions
    {
        HideCard,           // Zminimalizuj karte do ręki, na stół, na stos
        SellCardForLevel,   // Sprzedaj kartę za poziom
    }

    public class MunchkinGlobals
    {
        public static Dictionary<PlayerActions, string> PlayerActions = new Dictionary<PlayerActions, string>
        {
            { Models.PlayerActions.StartRound, "Zacznij rundę" },
            { Models.PlayerActions.OpenDoor, "Otwórz Drzwi" },
            { Models.PlayerActions.Fight, "Walcz" },
            { Models.PlayerActions.Escape, "Uciekaj" },
            { Models.PlayerActions.FindTreasure, "Przeszukaj pokój" },
            { Models.PlayerActions.UseCardsFromHand, "Użyj" },
            { Models.PlayerActions.PickUpCard, "Weź kartę do ręki" },
            { Models.PlayerActions.NextPhase, "Dalej" },
            { Models.PlayerActions.EndRound, "Koniec rundy" },
        };

        public static Dictionary<GeneralPlayerActions, string> GeneralPlayerActions = new Dictionary<GeneralPlayerActions, string>
        {
            { Models.GeneralPlayerActions.HideCard, "Schowaj kartę" },
            { Models.GeneralPlayerActions.SellCardForLevel, "Sprzedaj Kartę/y" },
        };

        public static Dictionary<QueuePhase, string> PlayerState = new Dictionary<QueuePhase, string>
        {
            { QueuePhase.OutOfRound, "Poza swoją rundą" },
            { QueuePhase.OpenDoor, "Otwórz Drzwi!" },
            { QueuePhase.FightPhase, "Walka!" },
            { QueuePhase.FindProblem, "Szukaj guza" },
            { QueuePhase.PickUpTreasure, "Przeszukaj pokój" },
            { QueuePhase.GiveCardsForFree, "Oddaj karty" },
            { QueuePhase.ShowStack, "Podgląd stosów" },
        };
    }
}
