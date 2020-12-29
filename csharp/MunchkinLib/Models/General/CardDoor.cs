using MunchkinLib.Helpers;
using MunchkinLib.Mediators;
using System;

namespace MunchkinLib.Models
{
    public class CardDoor : BaseCard
    {
        public bool IsDoorCard { get; }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private bool isAdditional = false;
        public bool IsAdditional
        {
            get { return isAdditional; }
            set { isAdditional = value; }
        }

        private int level = 0;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private int bonus = 0;
        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }

        private int reword = 0;
        public int Reword
        {
            get { return reword; }
            set { reword = value; }
        }

        private int additionalLevel = 0;
        public int AdditionalLevel
        {
            get { return additionalLevel; }
            set { additionalLevel = value; }
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

        private DoorType type = DoorType.Unknown;
        public DoorType Type
        {
            get { return type; }
            set { type = value; }
        }

        public CardDoor()
        {
            IsDoorCard = true;
        }

        public override string ToString()
        {
            return $"Drzwi, {CustomConverter.ConvertTypeToString(this)}, {Name}";
        }
    }
}
