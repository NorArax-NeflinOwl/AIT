namespace MunchkinLib.Models
{
    public class FractionBase : CardDoor
    {
        public FractionBase() : base()
        {
            Type = DoorType.Fraction;
            IsAdditional = true;
        }

        public FractionBase(string name, string description) : base()
        {
            Name = name;
            Description = description;
            Type = DoorType.Fraction;
            IsAdditional = true;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Pathfinder : FractionBase
    {
        public Pathfinder() : base()
        {
            base.Name = nameof(Pathfinder);
            base.Description = BaseCardsDescriptions.PathfinderDescription;
            base.HasSpecialEfect = true;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class EagleKnight : FractionBase
    {
        public EagleKnight() : base()
        {
            base.Name = nameof(EagleKnight);
            base.Description = BaseCardsDescriptions.EagleKnightDescription;
            base.HiddenBonus = 2;
            base.HiddenEscapeBonus = -1;
            base.HasSpecialEfect = true;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class HellKnight : FractionBase
    {
        public HellKnight() : base()
        {
            base.Name = nameof(HellKnight);
            base.Description = BaseCardsDescriptions.HellKnightDescription;
            base.Bonus = 5;
            base.HasSpecialEfect = true;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class DeadlyRedMantis : FractionBase
    {
        public DeadlyRedMantis() : base()
        {
            base.Name = nameof(DeadlyRedMantis);
            base.Description = BaseCardsDescriptions.DeadlyRedMantisDescription;
            base.EscapeBonus = 1;
            base.HiddenBonus = 2;
            base.HasSpecialEfect = true;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }
}
