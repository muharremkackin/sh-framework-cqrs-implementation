namespace SH.Framework.Library.Cqrs.Implementation;

/// <summary>
/// <see cref="SH.Framework.Library.Cqrs.Implementation.Result"/> için <c>Dictionary&lt;string, string[]&gt;</c> oluşturan akıcı yardımcı.
/// Örnek:
/// <code>
/// var errors = ResultError.Instance()
///     .Add("Reason")
///         .AddDetail("The service is currently unavailable. Please try again later.")
///         .AddDetail("SomeWeatherForecast Service connection error")
///     .Add("Email")
///         .AddDetail("Invalid format");
///
/// return Result.Failure&lt;T&gt;(MyResultCode.Something, errors);
/// </code>
/// </summary>
public sealed class ResultError
{
    private readonly Dictionary<string, List<string>> _messagesByField = new(StringComparer.Ordinal);

    public static ResultError Instance() => new();

    private ResultError()
    {
    }

    /// <summary>Yeni veya mevcut alan için detay satırları eklemeye başlar.</summary>
    public Field Add(string fieldName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);
        if (!_messagesByField.TryGetValue(fieldName, out var list))
        {
            list = [];
            _messagesByField[fieldName] = list;
        }

        return new Field(this, fieldName);
    }

    /// <summary><see cref="Result"/> hata sözlüğü.</summary>
    public Dictionary<string, string[]> ToDictionary() =>
        _messagesByField.ToDictionary(
            static kv => kv.Key,
            static kv => kv.Value.Count == 0 ? [] : kv.Value.ToArray(),
            StringComparer.Ordinal);

    public Dictionary<string, string[]> Build() => ToDictionary();

    public static implicit operator Dictionary<string, string[]>(ResultError errors) => errors.ToDictionary();

    internal void AppendDetail(string fieldName, string detail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(detail);
        if (!_messagesByField.TryGetValue(fieldName, out var list))
        {
            list = [];
            _messagesByField[fieldName] = list;
        }

        list.Add(detail);
    }

    /// <summary>Akıcı zincir: aynı alana birden çok <see cref="AddDetail"/> veya başka bir alan için <see cref="Add"/>.</summary>
    public sealed class Field
    {
        private readonly ResultError _root;
        private readonly string _fieldName;

        internal Field(ResultError root, string fieldName)
        {
            _root = root;
            _fieldName = fieldName;
        }

        public Field AddDetail(string detail)
        {
            _root.AppendDetail(_fieldName, detail);
            return this;
        }

        public Field Add(string fieldName) => _root.Add(fieldName);

        /// <summary>Akıcı zincirin sonunda <see cref="ResultError.ToDictionary"/> ile aynı.</summary>
        public Dictionary<string, string[]> ToDictionary() => _root.ToDictionary();

        public Dictionary<string, string[]> Build() => _root.Build();

        public static implicit operator Dictionary<string, string[]>(Field field) => field._root.ToDictionary();
    }
}
