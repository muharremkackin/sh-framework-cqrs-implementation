namespace SH.Framework.Library.Cqrs.Implementation;

public class Result<TResponse>: Result
{
    public virtual TResponse? Data { get; set; }
    
    public Result(): base() {}
}

public class Result
{
    public virtual int Code { get; init; }
    public virtual string? CategorizedCode { get; init; }
    public virtual string? Description { get; init; }
    public virtual bool IsSuccess => Code == 0;
    public virtual bool IsFailure => !IsSuccess;
    public virtual Dictionary<string, Dictionary<string, string>>? Errors { get; init; } = new();
    public Guid? RequestId { get; set; }

    public Result()
    {
    }

    public static Result Success(Guid? requestId = null) => new()
    {
        Code = ResultCode.Success.Code,
        Description = ResultCode.Success.Description,
        CategorizedCode = ResultCode.Success.ToString(),
        RequestId = requestId
    };

    public static Result Failure(int code, string description,
        Dictionary<string, Dictionary<string, string>>? errors = null, Guid? requestId = null) => new()
    {
        Code = code,
        Description = description,
        Errors = errors,
        RequestId = requestId
    };

    public static Result Failure(ResultCode resultCode, Dictionary<string, Dictionary<string, string>>? errors = null,
        Guid? requestId = null) => new()
    {
        Code = resultCode.Code,
        Description = resultCode.Description,
        CategorizedCode =  resultCode.ToString(),
        Errors = errors,
        RequestId = requestId
    };

    public static Result<TResponse> Success<TResponse>(TResponse? data, Guid? requestId = null) => new()
    {
        Code = ResultCode.Success.Code,
        Description = ResultCode.Success.Description,
        CategorizedCode =  ResultCode.Success.ToString(),
        Data = data,
        RequestId = requestId
    };

    public static Result<TResponse> Failure<TResponse>(int code, string description,
        Dictionary<string, Dictionary<string, string>>? errors = null, Guid? requestId = null) => new()
    {
        Code = code,
        Description = description,
        Errors = errors,
        RequestId = requestId
    };

    public static Result<TResponse> Failure<TResponse>(ResultCode resultCode,
        Dictionary<string, Dictionary<string, string>>? errors = null, Guid? requestId = null) => new()
    {
        Code = resultCode.Code,
        Description = resultCode.Description,
        CategorizedCode = resultCode.ToString(),
        Errors = errors,
        RequestId = requestId
    };
}