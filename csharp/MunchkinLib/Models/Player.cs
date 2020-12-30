using MunchkinLib.Mediators;
using MunchkinLib.Models.Source;
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

        private ObservableCollection<ICard> cardsInHand;
        public ObservableCollection<ICard> CardsInHand
        {
            get { return cardsInHand; }
        }

        private ObservableCollection<ICard> cardsInGame;
        public ObservableCollection<ICard> CardsInGame
        {
            get { return cardsInGame; }
        }

        public int CardsCount
        {
            get { return cardsInHand.Count; }
        }

        public List<ICard> PlayerRaces
        {
            get { return cardsInGame.Where(card => card is Card doorCard && CardTypeFlags.Race.Equals(doorCard.CardType)).ToList(); }
        }

        public List<ICard> PlayerClasses
        {
            get { return cardsInGame.Where(card => card is Card doorCard && CardTypeFlags.Class.Equals(doorCard.CardType)).ToList(); }
        }

        public List<ICard> PlayerFractions
        {
            get { return cardsInGame.Where(card => card is Card doorCard && CardTypeFlags.Fraction.Equals(doorCard.CardType)).ToList(); }
        }

        public int BonusCount
        {
            get 
            {
                int couter = 0;
                foreach(var card in cardsInGame)
                {
                    couter += card.IntegerAttributes[CardAttributes.Bonus];
                }

                return couter;
            }
        }

        public Player(bool isFemale)
        {
            this.isFemale = isFemale;

            money = GameProperties.MoneyStart;
            level = LevelProperties.START;

            cardsInHand = new ObservableCollection<ICard>();
            cardsInGame = new ObservableCollection<ICard>();

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

            return atributes;
        }

        private string PlayerMembershipsAsString(List<ICard> list)
        {
            string result = string.Empty;

            for (var i = 0; i < list.Count - 1; i++)
            {
                if (list[i] is Card door)
                    result += door.Name + ", ";
            }

            if(list.Count >= 1)
            {
                var card = list[list.Count - 1] as Card;
                if (null != card)
                    result += card.Name;
            }

            return result;
        }
    }
}
