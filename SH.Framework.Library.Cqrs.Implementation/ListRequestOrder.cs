using System.Collections.Frozen;

namespace SH.Framework.Library.Cqrs.Implementation;

public sealed class ListRequestOrder
{
    public enum OrderDirection
    {
        Asc,
        Desc
    }

    private static readonly FrozenDictionary<string, OrderDirection> Directions =
        new Dictionary<string, OrderDirection>
        {
            { "asc", OrderDirection.Asc },
            { "desc", OrderDirection.Desc }
        }.ToFrozenDictionary();

    public required string Field { get; set; }
    public string Direction { get; set; } = "asc";

    public OrderDirection GetDirection()
    {
        return Directions.GetValueOrDefault(Direction, OrderDirection.Asc);
    }
}