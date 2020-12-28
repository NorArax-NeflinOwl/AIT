namespace MunchkinLib.Models
{
    public class DoorClass : CardDoor
    {
        public DoorClass() : base()
        {
            Type = DoorType.Class;
        }

        public DoorClass(string name, string description) : base()
        {
            Name = name;
            Description = description;
            Type = DoorType.Class;
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
            base.Name = "Wojownik";
            base.Description = BaseCardsDescriptions.WarriorDescription;
            base.HasSpecialEfect = true;
            base.HiddenBonus = 1;
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
            base.Name = "Czarodziej";
            base.Description = BaseCardsDescriptions.WizardDescription;
            base.HasSpecialEfect = true;
            base.HiddenEscapeBonus = 1;
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
            base.Name = "Kapłan";
            base.Description = BaseCardsDescriptions.PriestDescription;
            base.HasSpecialEfect = true;
            base.HiddenBonus = 3;
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
            base.Name = "Złodziej";
            base.Description = BaseCardsDescriptions.ThiefDescription;
            base.HasSpecialEfect = true;
            base.HiddenBonus = -2;
            base.HiddenEscapeBonus = 1;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }
}
