namespace MunchkinLib.Models.Source
{
    public enum Boolean
    {
        True = 1,
        False = 0,
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

    public enum Actions
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

    public enum GeneralActions
    {
        HideCard,           // Zminimalizuj karte do ręki, na stół, na stos
        SellCardForLevel,   // Sprzedaj kartę za poziom
    }

    public enum CardView
    {
        BackDoor,
        BackTreasure,
        FrontDoor,
        FrontTreasure
    }
}
