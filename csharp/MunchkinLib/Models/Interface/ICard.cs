using MunchkinLib.Models.Source;
using MunchkinLib.Overrides;

namespace MunchkinLib.Models
{
    public interface ICard
    {
        CardType CardType { get; }

        CardFlags CardFlags { get; }
        CardTypeFlags CardTypeFlags { get; }

        string Name { get; }

        string Description { get; }

        bool IsFromAdditional { get; }

        MunchkinDictionary<CardAttributes, int> IntegerAttributes { get; }

        MunchkinDictionary<CardAttributes, string> StringAttributes { get; }
    }
}
