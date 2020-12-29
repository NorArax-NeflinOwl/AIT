using MunchkinLib.Helpers;
using MunchkinLib.Mediators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MunchkinLib.Models
{
    public class Player
    {
        private QueuePhase actualPhace = QueuePhase.OutOfRound;
        public QueuePhase ActualPhace
        {
            get { return actualPhace; }
            set { actualPhace = value; }
        }

        private int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private int money;
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        private bool isFemale = false;
        public bool IsFemale
        {
            get { return isFemale; }
            set { isFemale = value; }
        }

        private bool helper = false;
        public bool Helper
        {
            get { return helper; }
            set { helper = value; }
        }

        private ObservableCollection<BaseCard> cardsInHand;
        public ObservableCollection<BaseCard> CardsInHand
        {
            get { return cardsInHand; }
        }

        private int minimunCardsCountInHand;
        public int MinimunCardsCountInHand
        {
            get { return minimunCardsCountInHand; }
        }

        public int CardsCount
        {
            get { return cardsInHand.Count; }
        }

        public bool PlayerCanEndTour
        {
            get { return minimunCardsCountInHand >= CardsCount; }
        }

        private ObservableCollection<BaseCard> cardsInGame;
        public ObservableCollection<BaseCard> CardsInGame
        {
            get { return cardsInGame; }
        }

        public List<BaseCard> PlayerRaces
        {
            get { return cardsInGame.Where(card => card is CardDoor doorCard && DoorType.Race.Equals(doorCard.Type)).ToList(); }
        }

        public List<BaseCard> PlayerClasses
        {
            get { return cardsInGame.Where(card => card is CardDoor doorCard && DoorType.Class.Equals(doorCard.Type)).ToList(); }
        }

        public List<BaseCard> PlayerFractions
        {
            get { return cardsInGame.Where(card => card is CardDoor doorCard && DoorType.Fraction.Equals(doorCard.Type)).ToList(); }
        }

        public int BonusCount
        {
            get 
            {
                int couter = 0;
                foreach(var card in cardsInGame)
                {
                    couter += card.Bonus;
                }

                return couter;
            }
        }

        public int ArmorBonusCount
        {
            get
            {
                throw new NotImplementedException("Armor not implemented");
            }
        }

        public int EscapeBonus
        {
            get
            {
                int couter = 0;
                foreach (var card in cardsInGame)
                {
                    couter += card.EscapeBonus;
                }

                return couter;
            }
        }

        public Player(bool isFemale)
        {
            this.isFemale = isFemale;

            money = 0;
            level = LevelProperties.START;
            minimunCardsCountInHand = GameProperties.MinimunCardsCountInHand;

            cardsInHand = new ObservableCollection<BaseCard>();
            cardsInGame = new ObservableCollection<BaseCard>();

            GameMaster.Instance.GiveCardsToNewPlayer(cardsInHand);
        }

        public Dictionary<string, string> GetAtributeList()
        {
            Dictionary<string, string> atributes = new Dictionary<string, string>();
            atributes.Add("Poziom", level.ToString());
            atributes.Add("Pieniądze", money.ToString() + " Sztuk Złota");
            atributes.Add("Płęć", IsFemale ? "Kobieta" : "Męszczyzna");
            atributes.Add("Rasa/y", PlayerMembershipsAsString(PlayerRaces));
            atributes.Add("Klasa/y", PlayerMembershipsAsString(PlayerClasses));
            atributes.Add("Frakcja/ie", PlayerMembershipsAsString(PlayerFractions));
            atributes.Add("Moc przedmiotów", BonusCount.ToString());

            return atributes;
        }

        private string PlayerMembershipsAsString(List<BaseCard> list)
        {
            string result = string.Empty;

            for (var i = 0; i < list.Count - 1; i++)
            {
                if (list[i] is CardDoor door)
                    result += door.Name + ", ";
            }

            if(list.Count >= 1)
            {
                var card = list[list.Count - 1] as CardDoor;
                if (null != card)
                    result += card.Name;
            }

            return result;
        }
    }
}
