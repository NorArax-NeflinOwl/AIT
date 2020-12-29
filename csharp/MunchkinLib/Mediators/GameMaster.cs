using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MunchkinLib.Helpers;
using MunchkinLib.Models;

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

        private Stack<CardDoor> doorCards;
        private Stack<CardTreasure> treasureCards;

        private List<BaseCard> rejectedDoorCards;
        private List<BaseCard> rejectedTreasureCards;

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
            doorCards = new Stack<CardDoor>();
            treasureCards = new Stack<CardTreasure>();

            rejectedDoorCards = new List<BaseCard>();
            rejectedTreasureCards = new List<BaseCard>();

            fillStacks();
        }

        private void fillStacks()
        {
            fillDoorCardStack();
            fillTreasureCardStack();
        }

        private void fillDoorCardStack()
        {
            var tempCardDoor = new List<BaseCard>();

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

            foreach (CardDoor card in tempCardDoor)
                doorCards.Push(card);
        }

        private void fillTreasureCardStack()
        {
            var tempCard = new List<BaseCard>();


            Randomizer.Shuffling(tempCard);

            foreach (CardTreasure card in tempCard)
                treasureCards.Push(card);
        }

        public void Add2RejectDoorCard(BaseCard card)
        {
            rejectedDoorCards.Add(card);
        }

        public void Add2RejectTreasureCard(BaseCard card)
        {
            rejectedDoorCards.Add(card);
        }

        public void GiveCardsToNewPlayer(ObservableCollection<BaseCard> cards)
        {
            for(var i = 0; i < GameProperties.DoorCardNumber; i++)
            {
                BaseCard card = GiveOneDoorCard();

                if (null != card)
                {
                    cards.Add(card);
                }
            }

            for (var i = 0; i < GameProperties.TreasureCardNumber; i++)
            {
                BaseCard card = GiveOneTreasureCard();

                if(null != card)
                {
                    cards.Add(card);
                }
            }
        }

        public CardDoor GiveOneDoorCard()
        {
            CardDoor card = null;
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
                foreach (BaseCard c in rejectedDoorCards)
                {
                    doorCards.Push((CardDoor)c);
                }

                rejectedDoorCards = new List<BaseCard>();
            }

            if (doorCards.Any())
            {
                card = doorCards.Pop();
            }

            return card;
        }

        public CardTreasure GiveOneTreasureCard()
        {
            CardTreasure card = null;
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
                foreach (BaseCard c in rejectedTreasureCards)
                {
                    treasureCards.Push((CardTreasure)c);
                }

                rejectedDoorCards = new List<BaseCard>();
            }

            if(treasureCards.Any())
            {
                card = treasureCards.Pop();
            }

            return card;
        }

        public CardDoor ShowOneRejectedDoorCard(int backIndex = 0)
        {
            int index = 1 + backIndex;
            CardDoor card = null;
            if (rejectedDoorCards.Any())
            {
                card = (CardDoor)rejectedDoorCards[rejectedDoorCards.Count - index];
            }

            return card;
        }

        public CardTreasure ShowOneRejectedTreasureCard(int backIndex = 0)
        {
            int index = 1 + backIndex;
            CardTreasure card = null;
            if (rejectedTreasureCards.Any())
            {
                card = (CardTreasure)rejectedTreasureCards[rejectedTreasureCards.Count - index];
            }

            return card;
        }
    }
}
