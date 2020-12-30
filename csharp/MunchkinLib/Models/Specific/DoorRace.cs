using MunchkinLib.Models.Source;

namespace MunchkinLib.Models
{
    public class RaceBase : Card
    {
        public RaceBase() : base()
        {
            CardType |= CardTypeFlags.Race;
        }

        public RaceBase(string name, string description) : base()
        {
            Name = name;
            Description = description;
            CardType |= CardTypeFlags.Race;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Halfling : RaceBase
    {
        public Halfling() : base()
        {
            base.Name = Resources.HalflingName;
            base.Description = Resources.HalflingDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Elf : RaceBase
    {
        public Elf() : base()
        {
            base.Name = Resources.ElfName;
            base.Description = Resources.ElfDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }

    public class Dwarf : RaceBase
    {
        public Dwarf() : base()
        {
            base.Name = Resources.DwarfName;
            base.Description = Resources.DwarfDescription;
            CardFlags |= CardFlags.HasSpecialEfects;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }
}
