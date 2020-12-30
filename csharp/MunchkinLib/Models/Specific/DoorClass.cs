using MunchkinLib.Models.Source;

namespace MunchkinLib.Models
{
    public class DoorClass : Card
    {
        public DoorClass() : base()
        {
            CardType |= CardTypeFlags.Class;
        }

        public DoorClass(string name, string description) : base()
        {
            Name = name;
            Description = description;
            CardType |= CardTypeFlags.Class;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Warrior : RaceBase
    {
        public Warrior() : base()
        {
            Name = Resources.WarriorName;
            Description = Resources.WarriorDescription;
            StringAttributes.Add(CardAttributes.Secial, "Remis Win");
            StringAttributes.Add(CardAttributes.HiddenBonusAfterGiveCardFromHand, "+1, 3");
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Wizard : RaceBase
    {
        public Wizard() : base()
        {
            Name = Resources.WizardName;
            Description = Resources.WizardDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
            StringAttributes.Add(CardAttributes.HiddenExcapeBonusAfterGiveCardFromHand, "+1, 3");
            StringAttributes.Add(CardAttributes.HiddenPassFightWithOneMosterAndPickTreasure, "3");
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Priest : RaceBase
    {
        public Priest() : base()
        {
            Name = Resources.PriestName;
            Description = Resources.PriestDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
            StringAttributes.Add(CardAttributes.HiddenBonusAfterGiveCardFromHand, "+3, 3");
            StringAttributes.Add(CardAttributes.HiddenExchangeCardsWithRejectedStack, "+3, 3");
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Thief : RaceBase
    {
        public Thief() : base()
        {
            base.Name = Resources.ThiefName;
            base.Description = Resources.ThiefDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
            StringAttributes.Add(CardAttributes.HiddenBonusByCardFromCard, "-2");
            StringAttributes.Add(CardAttributes.HiddenThiefBonus, "4, -1lvl");
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }
}
