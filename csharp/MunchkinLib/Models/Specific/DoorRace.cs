namespace MunchkinLib.Models
{
    public class RaceBase : CardDoor
    {
        public RaceBase() : base()
        {
            Type = DoorType.Race;
        }

        public RaceBase(string name, string description) : base()
        {
            Name = name;
            Description = description;
            Type = DoorType.Race;
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
            base.Name = nameof(Halfling);
            base.Description = BaseCardsDescriptions.HalflingDescription;
            base.HasSpecialEfect = true;
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
            base.Name = nameof(Elf);
            base.Description = BaseCardsDescriptions.ElfDescription;
            base.HasSpecialEfect = true;
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
            base.Name = nameof(Dwarf);
            base.Description = BaseCardsDescriptions.DwarfDescription;
            base.HasSpecialEfect = true;
        }

        public override bool SpecialEfectHandler()
        {
            return base.SpecialEfectHandler();
        }
    }
}
