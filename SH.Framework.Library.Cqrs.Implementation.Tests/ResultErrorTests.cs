
namespace SH.Framework.Library.Cqrs.Implementation.Tests;

/// <summary>
/// <see cref="ResultError"/> birim testleri.
/// Yerelde <see cref="System.IO.FileLoadException"/> (0x800711C7) görürseniz Windows Akıllı Uygulama Denetimi yerel derlemeleri engelliyor olabilir.
/// </summary>
public class ResultErrorTests
{
    [Fact]
    public void Instance_ThenAddReason_AndTwoAddDetails_ProducesOneFieldWithTwoMessages()
    {
        var errors = ResultError.Instance()
            .Add("Reason")
                .AddDetail("The service is currently unavailable. Please try again later.")
                .AddDetail("SomeWeatherForecast Service connection error");

        var dict = errors.ToDictionary();

        Assert.Single(dict);
        Assert.True(dict.ContainsKey("Reason"));
        var messages = dict["Reason"];
        Assert.Equal(2, messages.Length);
        Assert.Equal("The service is currently unavailable. Please try again later.", messages[0]);
        Assert.Equal("SomeWeatherForecast Service connection error", messages[1]);
    }

    [Fact]
    public void MultipleFields_EachPreservesOrder()
    {
        var dict = ResultError.Instance()
            .Add("Reason")
                .AddDetail("r1")
            .Add("Email")
                .AddDetail("e1")
                .AddDetail("e2")
            .ToDictionary();

        Assert.Equal(2, dict.Count);
        Assert.Equal(new[] { "r1" }, dict["Reason"]);
        Assert.Equal(new[] { "e1", "e2" }, dict["Email"]);
    }

    [Fact]
    public void AddSameFieldTwice_AppendsDetailsToSameList()
    {
        var dict = ResultError.Instance()
            .Add("Reason")
                .AddDetail("first")
            .Add("Reason")
                .AddDetail("second")
            .ToDictionary();

        Assert.Single(dict);
        Assert.Equal(new[] { "first", "second" }, dict["Reason"]);
    }

    [Fact]
    public void AddFieldWithoutAddDetail_YieldsEmptyArrayForThatKey()
    {
        var dict = ResultError.Instance()
            .Add("Reason")
            .ToDictionary();

        Assert.Single(dict);
        Assert.Empty(dict["Reason"]);
    }

    [Fact]
    public void EmptyBuilder_ToDictionary_IsEmpty()
    {
        var dict = ResultError.Instance().ToDictionary();
        Assert.Empty(dict);
    }

    [Fact]
    public void Build_Aliases_ToDictionary()
    {
        var root = ResultError.Instance()
            .Add("X")
                .AddDetail("one");

        Assert.Equal(root.ToDictionary(), root.Build());
    }

    [Fact]
    public void ImplicitConversion_ToDictionary_WorksWithResultFailureShape()
    {
        var errors = ResultError.Instance()
            .Add("Reason")
                .AddDetail("down");

        Dictionary<string, string[]> dict = errors;

        Assert.Single(dict);
        Assert.Equal(new[] { "down" }, dict["Reason"]);
    }

    [Fact]
    public void ImplicitConversion_FromField_AfterLastAddDetail_WorksLikeRoot()
    {
        ResultError.Field tail = ResultError.Instance()
            .Add("Reason")
                .AddDetail("a")
                .AddDetail("b");

        Dictionary<string, string[]> dict = tail;

        Assert.Equal(new[] { "a", "b" }, dict["Reason"]);
    }

    [Fact]
    public void FieldNamesAreOrdinalCaseSensitive()
    {
        var dict = ResultError.Instance()
            .Add("Reason")
                .AddDetail("a")
            .Add("reason")
                .AddDetail("b")
            .ToDictionary();

        Assert.Equal(2, dict.Count);
        Assert.Equal(new[] { "a" }, dict["Reason"]);
        Assert.Equal(new[] { "b" }, dict["reason"]);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_ThrowsForInvalidFieldName(string? fieldName)
    {
        // null -> ArgumentNullException; "" / whitespace -> ArgumentException (ikisi de ArgumentException türevi)
        Assert.ThrowsAny<ArgumentException>(() =>
            ResultError.Instance().Add(fieldName!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddDetail_ThrowsForInvalidDetail(string? detail)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            ResultError.Instance()
                .Add("Reason")
                .AddDetail(detail!));
    }
}
