using MunchkinLib.Helpers;
using MunchkinLib.Models.Source;
using MunchkinLib.Overrides;
using System;

namespace MunchkinLib.Models
{
    public class Card : ICard
    {
        private string name;
        public string Name
        {
            get { return name; }
            protected set { name = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            protected set { description = value; }
        }

        private bool isAdditional = false;
        public bool IsAdditional
        {
            get { return isAdditional; }
            protected set { isAdditional = value; }
        }

        private CardTypeFlags cardType;
        public CardTypeFlags CardType
        {
            get { return cardType; }
            protected set { cardType = value; }
        }

        private CardFlags cardFlags;
        public CardFlags CardFlags
        {
            get { return cardFlags; }
            protected set { cardFlags = value; }
        }

        private MunchkinDictionary<CardAttributes, int> integerAttributes;
        public MunchkinDictionary<CardAttributes, int> IntegerAttributes
        {
            get { return integerAttributes; }
        }

        private MunchkinDictionary<CardAttributes, string> stringAttributes;
        public MunchkinDictionary<CardAttributes, string> StringAttributes
        {
            get { return stringAttributes; }
        }

        public Card()
        {
            cardType = CardTypeFlags.Door;
            integerAttributes = new MunchkinDictionary<CardAttributes, int>();
            stringAttributes = new MunchkinDictionary<CardAttributes, string>();
        }

        public override string ToString()
        {
            return $"{GetTypeCartString()}" +
                $", {CustomConverter.ConvertTypeToString(this)}" +
                $", {Name}";
        }

        public virtual bool CheckSpecialEfectHandler()
        {
            if (false)
                throw new NotImplementedException("Special effect not implemented");
            return true;
        }

        public virtual bool SpecialEfectHandler()
        {
            if (false)
                throw new NotImplementedException("Special effect not implemented");
            return true;
        }

        private string GetTypeCartString()
        {
            string type;
            if (CardType.HasFlag(CardTypeFlags.Door))
            {
                type = MunchkinGlobals.Instance.CardTypes[CardTypeFlags.Door];
            }
            else
            {
                type = MunchkinGlobals.Instance.CardTypes[CardTypeFlags.Treasure];
            }
            return type;
        }
    }
}
