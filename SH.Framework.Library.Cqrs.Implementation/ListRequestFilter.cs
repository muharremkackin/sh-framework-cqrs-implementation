using System.Collections.Frozen;

namespace SH.Framework.Library.Cqrs.Implementation;

public sealed class ListRequestFilter
{
    public enum FilterOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Contains,
        NotContains,
        StartsWith,
        NotStartsWith,
        EndsWith,
        NotEndsWith,
        Between,
        NotBetween,
        IsNull,
        IsNotNull,
        IsEmpty,
        IsNotEmpty,
        In,
        NotIn
    }

    private static readonly FrozenDictionary<string, FilterOperator> Operators =
        new Dictionary<string, FilterOperator>
        {
            { "eq", FilterOperator.Equals },
            { "ne", FilterOperator.NotEquals },
            { "gt", FilterOperator.GreaterThan },
            { "gte", FilterOperator.GreaterThanOrEqual },
            { "lt", FilterOperator.LessThan },
            { "lte", FilterOperator.LessThanOrEqual },
            { "contains", FilterOperator.Contains },
            { "not-contains", FilterOperator.NotContains },
            { "starts-with", FilterOperator.StartsWith },
            { "not-starts-with", FilterOperator.NotStartsWith },
            { "ends-with", FilterOperator.EndsWith },
            { "not-ends-with", FilterOperator.NotEndsWith },
            { "between", FilterOperator.Between },
            { "not-between", FilterOperator.NotBetween },
            { "is-null", FilterOperator.IsNull },
            { "is-not-null", FilterOperator.IsNotNull },
            { "is-empty", FilterOperator.IsEmpty },
            { "is-not-empty", FilterOperator.IsNotEmpty },
            { "in", FilterOperator.In },
            { "not-in", FilterOperator.NotIn }
        }.ToFrozenDictionary();

    public required string Field { get; set; }
    public string? Value { get; set; }
    public string Operator { get; set; } = "eq";

    public FilterOperator GetOperator()
    {
        return Operators.GetValueOrDefault(Operator, FilterOperator.Equals);
    }
}