using MunchkinLib.Models.Source;

namespace MunchkinLib.Models
{
    public class FractionBase : Card
    {
        public FractionBase() : base()
        {
            CardType |= CardTypeFlags.Fraction;
            IsAdditional = true;
        }

        public FractionBase(string name, string description) : base()
        {
            Name = name;
            Description = description;
            CardType |= CardTypeFlags.Fraction;
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
            base.Name = Resources.PathfinderName;
            base.Description = Resources.PathfinderDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
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
            base.Name = Resources.EagleKnightName;
            base.Description = Resources.EagleKnightDescription;
            StringAttributes.Add(CardAttributes.HiddenBonus, "2");
            StringAttributes.Add(CardAttributes.HiddenEscapeBonus, "-1");
            CardFlags |= CardFlags.HasSpecialEfects;
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
            base.Name = Resources.HellKnightName;
            base.Description = Resources.HellKnightDescription;
            StringAttributes.Add(CardAttributes.Bonus, "5");
            CardFlags |= CardFlags.HasSpecialEfects;
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
            base.Name = Resources.DeadlyRedMantisName;
            base.Description = Resources.DeadlyRedMantisDescription;
            StringAttributes.Add(CardAttributes.HiddenBonus, "2");
            StringAttributes.Add(CardAttributes.EscapeBonus, "1");
            CardFlags |= CardFlags.HasSpecialEfects;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }
}
