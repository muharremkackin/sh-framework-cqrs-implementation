namespace SH.Framework.Library.Cqrs.Implementation;

public class ResultCode
{
    public string? Category { get; set; }
    public int Code { get;  }
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"{Category ?? "UC"}{Code}";
    }
    
    protected ResultCode(int code, string? category, string? description = null)
    {
        Code = code;
        Category = category;
        Description = description;
    }
    
    public static ResultCode Instance(int code, string? category, string? description = null) => new(code, category, description);
    
    public static ResultCode Success => Instance(0, DefaultResultCodeCategory.Category,"Success");
    public static ResultCode Exception => Instance(1, DefaultResultCodeCategory.Category,"Exception");
    public static ResultCode Failure => Instance(2,  DefaultResultCodeCategory.Category, "Failure");
}

public static class DefaultResultCodeCategory
{
    public static readonly string Category = "DF";
}