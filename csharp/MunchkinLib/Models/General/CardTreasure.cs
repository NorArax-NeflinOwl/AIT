using MunchkinLib.Mediators;
using System;

namespace MunchkinLib.Models
{
    public class CardTreasure : BaseCard
    {
        public uint ID { get; }

        public bool IsDoorCard { get; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string requirement;
        public string Requirement
        {
            get { return requirement; }
            set { requirement = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        private bool isAdditional = false;
        public bool IsAdditional
        {
            get { return isAdditional; }
            set { isAdditional = value; }
        }

        private int bonus = 0;
        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }

        private int hiddenBonus = 0;
        public int HiddenBonus
        {
            get { return hiddenBonus; }
            set { hiddenBonus = value; }
        }

        private int escapeBonus = 0;
        public int EscapeBonus
        {
            get { return escapeBonus; }
            set { escapeBonus = value; }
        }

        private int hiddenEscapeBonus = 0;
        public int HiddenEscapeBonus
        {
            get { return hiddenEscapeBonus; }
            set { hiddenEscapeBonus = value; }
        }

        private string itemType;
        public string ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        private bool isBig;
        public bool IsBig
        {
            get { return isBig; }
            set { isBig = value; }
        }

        private bool haveSpecialEfect = false;
        public bool HasSpecialEfect
        {
            get { return haveSpecialEfect; }
            set { haveSpecialEfect = value; }
        }

        public virtual bool CheckSpecialEfectHandler()
        {
            if (haveSpecialEfect)
                throw new NotImplementedException("Special effect not implemented");
            return true;
        }

        public virtual bool SpecialEfectHandler()
        {
            if (haveSpecialEfect)
                throw new NotImplementedException("Special effect not implemented");
            return true;
        }

        public CardTreasure()
        {
            IsDoorCard = false;
        }

        public override string ToString()
        {
            return "Karta Skarbu, " + Name;
        }
    }
}
