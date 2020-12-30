using System;

namespace MunchkinLib.Models.Source
{
    [Flags]
    public enum CardTypeFlags
    {
        Unknown = 0,
        Door = 1,
        Treasure = 2,
        Class = 4,
        Race = 8,
        Fraction = 16,
        Special = 32,
        Monster = 64,
        Curse = 128,
    }

    [Flags]
    public enum CardAttributes
    {
        Level = 1,
        Price = 2,
        Reword = 4,
        LevelReword = 8,
        Bonus = 16,
        HiddenBonus = 32,
        EscapeBonus = 64,
        HiddenEscape = 128,
        Requirement = 256,
        OppositeRequirement = 512,
        SpecialAgainsTo = 1042,

        Secial = 1048576, // opis skilla
        HiddenEscapeBonus = 2097152,
        HiddenExcapeBonusAfterGiveCardFromHand = 4194304, // jaki bonus, max kart mozliwych do oddania
        HiddenBonusAfterGiveCardFromHand = 8388608, // jaki bonus, max kart mozliwych do oddania
        HiddenPassFightWithOneMosterAndPickTreasure = 16777216, // ile minimum kard do oddania aby zabrac skarb potworowi bez jego pokonywania
        HiddenExchangeCardsWithRejectedStack = 33554432,
        HiddenBonusByCardFromCard = 67108864,
        HiddenThiefBonus = 134217728, // minimum aby wygrać rzut kostką, przegrana oznacza strate poziomu
    }

    [Flags]
    public enum CardFlags
    {
        IsHead = 1,
        IsArmor = 2,
        IsOneHandWeaponLeft = 4,
        IsOneHandWeaponRight = 8,
        IsTwoHandWeapon = 16,
        IsShoes = 32,
        IsBig = 64,
        HasBonus = 128,
        HasHiddenBonus = 256,
        IsLevelUpCard = 512,
        IsOnlyOnesUsable = 1024,
        IsFusionCard = 2048,
        HasSpecialSkills = 4096, 
        IsEscapeCard = 8192,
        CasFightPass = 16384,
        CasFightWin = 32768,
        HasRequirement = 65536,
        HasOppositeRequirement = 131072,
        HasSpecialAgainsTo = 262144,
        HasEscapeBonus = 524288,
        HasSpecialEfects = 2097152,
        HasPrice = 4194304,
    }
}
