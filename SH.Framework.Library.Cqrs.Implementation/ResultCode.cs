namespace SH.Framework.Library.Cqrs.Implementation;

public class ResultCode
{
    public int Code { get;  }
    public string? Description { get;  }
    
    protected ResultCode(int code, string? description)
    {
        Code = code;
        Description = description;
    }
    
    public static ResultCode Instance(int code, string? description) => new(code, description);
    
    public static ResultCode Success => Instance(0, "Success");
    public static ResultCode Exception => Instance(1, "Exception");
    public static ResultCode Failure => Instance(2, "Failure");
}