using MunchkinLib.Models.Source;
using MunchkinLib.Overrides;
using System.Collections.Generic;

namespace MunchkinLib.Models
{
    public interface ICard
    {
        CardTypeFlags CardType { get; }

        CardFlags CardFlags { get; }

        string Name { get; }

        string Description { get; }

        bool IsAdditional { get; }

        MunchkinDictionary<CardAttributes, int> IntegerAttributes { get; }

        MunchkinDictionary<CardAttributes, string> StringAttributes { get; }
    }
}
