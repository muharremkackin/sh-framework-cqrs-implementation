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
    public virtual Dictionary<string, Dictionary<string, string>> Errors { get; init; } = new();
    public Guid? RequestId { get; set; }

    public Result()
    {
    }

    public static Result Success(ResultCode? resultCode = null) => new()
    {
        Code = resultCode != null ? resultCode.Code : ResultCode.Success.Code,
        Description = resultCode != null ? resultCode.Description : ResultCode.Success.Description,
        CategorizedCode = resultCode != null ? resultCode.ToString() : ResultCode.Success.ToString()
    };

    public static Result Success(int? code = null, string? description = null) => new()
    {
        Code = code ?? ResultCode.Success.Code,
        Description = description ?? ResultCode.Success.Description,
        CategorizedCode = code != null ? $"CUSTOM{code}" : ResultCode.Success.ToString()
    };

    public static Result<TResponse> Success<TResponse>(TResponse? data, ResultCode? resultCode = null) => new()
    {
        Code =  resultCode != null ? resultCode.Code : ResultCode.Success.Code,
        Description = resultCode != null ? resultCode.Description : ResultCode.Success.Description,
        CategorizedCode = resultCode != null ? resultCode.ToString() : ResultCode.Success.ToString(),
        Data = data,
    };

    public static Result<TResponse> Success<TResponse>(TResponse? data, int? code = null, string? description = null) => new()
    {
        Code = code ?? ResultCode.Success.Code,
        Description = description ?? ResultCode.Success.Description,
        CategorizedCode = code != null ? $"CUSTOM{code}" : ResultCode.Success.ToString(),
        Data = data,
    };

    public static Result Failure(int code, string description,
        Dictionary<string, Dictionary<string, string>>? errors = null, Guid? requestId = null) => new()
    {
        Code = code,
        Description = description,
        Errors = errors ?? [],
        RequestId = requestId
    };

    public static Result Failure(ResultCode resultCode, Dictionary<string, Dictionary<string, string>>? errors = null,
        Guid? requestId = null) => new()
    {
        Code = resultCode.Code,
        Description = resultCode.Description,
        CategorizedCode =  resultCode.ToString(),
        Errors = errors ?? [],
        RequestId = requestId
    };

    public static Result<TResponse> Failure<TResponse>(int code, string description,
        Dictionary<string, Dictionary<string, string>>? errors = null, Guid? requestId = null) => new()
        {
            Code = code,
            Description = description,
            Errors = errors ?? [],
            RequestId = requestId
        };

    public static Result<TResponse> Failure<TResponse>(ResultCode resultCode,
        Dictionary<string, Dictionary<string, string>>? errors = null, Guid? requestId = null) => new()
        {
            Code = resultCode.Code,
            Description = resultCode.Description,
            CategorizedCode = resultCode.ToString(),
            Errors = errors ?? [],
            RequestId = requestId
        };
}