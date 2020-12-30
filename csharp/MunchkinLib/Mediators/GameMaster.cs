using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MunchkinLib.Helpers;
using MunchkinLib.Models;
using MunchkinLib.Models.Source;

namespace MunchkinLib.Mediators
{
    public class GameMaster
    {
        private static object locker = new object(); 
        private static GameMaster instance;
        public static GameMaster Instance
        {
            get
            {
                lock(locker)
                {
                    if(null == instance)
                    {
                        instance = new GameMaster();
                    }
                    return instance;
                }
            }
        }

        private Stack<Card> doorCards;
        private Stack<Card> treasureCards;

        private List<ICard> rejectedDoorCards;
        private List<ICard> rejectedTreasureCards;

        public bool DoorCardStackIsNotEmpty
        {
            get { return doorCards.Any(); }
        }

        public bool TreasureCardStackIsNotEmpty
        {
            get { return treasureCards.Any(); }
        }

        public bool RejectedDoorCardStackIsNotEmpty
        {
            get { return rejectedDoorCards.Any(); }
        }

        public bool RejectedTreasureCardStackIsNotEmpty
        {
            get { return rejectedTreasureCards.Any(); }
        }

        private GameMaster()
        {
            doorCards = new Stack<Card>();
            treasureCards = new Stack<Card>();

            rejectedDoorCards = new List<ICard>();
            rejectedTreasureCards = new List<ICard>();

            fillStacks();
        }

        private void fillStacks()
        {
            fillDoorCardStack();
            fillTreasureCardStack();
        }

        private void fillDoorCardStack()
        {
            var tempCardDoor = new List<ICard>();

            for (var i = 0; i < 3; i++)
            {
                tempCardDoor.Add(new Pathfinder());
                tempCardDoor.Add(new EagleKnight());
                tempCardDoor.Add(new HellKnight());

                tempCardDoor.Add(new Halfling());
                tempCardDoor.Add(new Elf());
                tempCardDoor.Add(new Dwarf());

                tempCardDoor.Add(new Warrior());
                tempCardDoor.Add(new Wizard());
                tempCardDoor.Add(new Priest());
                tempCardDoor.Add(new Thief());
            }

            Randomizer.Shuffling(tempCardDoor);

            foreach (Card card in tempCardDoor)
                doorCards.Push(card);
        }

        private void fillTreasureCardStack()
        {
            var tempCard = new List<ICard>();


            Randomizer.Shuffling(tempCard);

            foreach (Card card in tempCard)
                treasureCards.Push(card);
        }

        public void Add2RejectDoorCard(ICard card)
        {
            rejectedDoorCards.Add(card);
        }

        public void Add2RejectTreasureCard(ICard card)
        {
            rejectedDoorCards.Add(card);
        }

        public void GiveCardsToNewPlayer(ObservableCollection<ICard> cards)
        {
            for(var i = 0; i < GameProperties.DoorCardNumber; i++)
            {
                ICard card = GiveOneDoorCard();

                if (null != card)
                {
                    cards.Add(card);
                }
            }

            for (var i = 0; i < GameProperties.TreasureCardNumber; i++)
            {
                ICard card = GiveOneTreasureCard();

                if(null != card)
                {
                    cards.Add(card);
                }
            }
        }

        public Card GiveOneDoorCard()
        {
            Card card = null;
            try
            {
                card = doorCards.Pop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (null == card && rejectedDoorCards.Any())
            {
                Randomizer.Shuffling(rejectedDoorCards);
                foreach (ICard c in rejectedDoorCards)
                {
                    doorCards.Push((Card)c);
                }

                rejectedDoorCards = new List<ICard>();
            }

            if (doorCards.Any())
            {
                card = doorCards.Pop();
            }

            return card;
        }

        public Card GiveOneTreasureCard()
        {
            Card card = null;
            try
            {
                card = treasureCards.Pop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (null == card && rejectedTreasureCards.Any())
            {
                Randomizer.Shuffling(rejectedTreasureCards);
                foreach (ICard c in rejectedTreasureCards)
                {
                    treasureCards.Push((Card)c);
                }

                rejectedDoorCards = new List<ICard>();
            }

            if(treasureCards.Any())
            {
                card = treasureCards.Pop();
            }

            return card;
        }

        public Card ShowOneRejectedDoorCard(int backIndex = 0)
        {
            int index = 1 + backIndex;
            Card card = null;
            if (rejectedDoorCards.Any())
            {
                card = (Card)rejectedDoorCards[rejectedDoorCards.Count - index];
            }

            return card;
        }

        public Card ShowOneRejectedTreasureCard(int backIndex = 0)
        {
            int index = 1 + backIndex;
            Card card = null;
            if (rejectedTreasureCards.Any())
            {
                card = (Card)rejectedTreasureCards[rejectedTreasureCards.Count - index];
            }

            return card;
        }
    }
}
