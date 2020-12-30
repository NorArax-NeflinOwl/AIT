using System.Collections.Generic;

namespace MunchkinLib.Models.Source
{
    public class MunchkinGlobals
    {
        public Dictionary<Actions, string> PlayerActions;
        public Dictionary<GeneralActions, string> GeneralPlayerActions;
        public Dictionary<QueuePhase, string> PlayerState;
        public Dictionary<CardTypeFlags, string> CardTypes;
        public Dictionary<CardFlags, string> CardsInfoParts;

        private static object locker = new object();
        private static MunchkinGlobals instance;

        public static MunchkinGlobals Instance
        {
            get
            {
                lock(locker)
                {
                    if (null == instance)
                    {
                        instance = new MunchkinGlobals();
                    }
                    return instance;
                }
            }
        }

        private  MunchkinGlobals()
        {
            PlayerActions = new Dictionary<Actions, string>
            {
                { Actions.StartRound, "Zacznij rundę" },
                { Actions.OpenDoor, "Otwórz Drzwi" },
                { Actions.Fight, "Walcz" },
                { Actions.Escape, "Uciekaj" },
                { Actions.FindTreasure, "Przeszukaj pokój" },
                { Actions.UseCardsFromHand, "Użyj" },
                { Actions.PickUpCard, "Weź kartę do ręki" },
                { Actions.NextPhase, "Dalej" },
                { Actions.EndRound, "Koniec rundy" },
            };

            GeneralPlayerActions = new Dictionary<GeneralActions, string>
            {
                { GeneralActions.HideCard, "Schowaj kartę" },
                { GeneralActions.SellCardForLevel, "Sprzedaj Kartę/y" },
            };

            PlayerState = new Dictionary<QueuePhase, string>
            {
                { QueuePhase.OutOfRound, "Poza swoją rundą" },
                { QueuePhase.OpenDoor, "Otwórz Drzwi!" },
                { QueuePhase.FightPhase, "Walka!" },
                { QueuePhase.FindProblem, "Szukaj guza" },
                { QueuePhase.PickUpTreasure, "Przeszukaj pokój" },
                { QueuePhase.GiveCardsForFree, "Oddaj karty" },
                { QueuePhase.ShowStack, "Podgląd stosów" },
            };

            CardTypes = new Dictionary<CardTypeFlags, string>
            {
                { CardTypeFlags.Door, "Drzwi" },
                { CardTypeFlags.Treasure, "Skarb" },
                { CardTypeFlags.Class, "Klasa" },
                { CardTypeFlags.Race, "Rasa" },
                { CardTypeFlags.Fraction, "Frakcja" },
                { CardTypeFlags.Special, "Specjalna" },
                { CardTypeFlags.Monster, "Potwór" },
                { CardTypeFlags.Curse, "Kłątwa" },
            };

            CardsInfoParts = new Dictionary<CardFlags, string>
            {
                { CardFlags.IsHead, "Hełm" },
                { CardFlags.IsArmor, "Zbroja" },
                { CardFlags.IsOneHandWeaponLeft, "Broń 1-Ręczna" },
                { CardFlags.IsOneHandWeaponRight, "Broń 1-Ręczna" },
                { CardFlags.IsTwoHandWeapon, "Bróń 2-Ręczna" },
                { CardFlags.IsShoes, "Buty" },
                { CardFlags.IsBig, "Duża" },
                { CardFlags.HasPrice, " Sztuk Złota" },
                { CardFlags.HasBonus, "Bonus " },
                { CardFlags.HasEscapeBonus, " do Ucieczki" },
            };
        }
    }
}
