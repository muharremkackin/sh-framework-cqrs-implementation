using System.ComponentModel;

namespace SH.Framework.Library.Cqrs.Implementation;

public class Result<TResponse>: Result
{
    [ReadOnly(true)]
    public TResponse? Data { get; init; }
    
    public Result(): base() {}
}

public class Result
{
    [ReadOnly(true)]
    public int Code { get; init; }
    [ReadOnly(true)]
    public string? CategorizedCode { get; init; }
    [ReadOnly(true)]
    public string? Description { get; init; }
    [ReadOnly(true)]
    public bool IsSuccess => Code == 0;
    [ReadOnly(true)]
    public Dictionary<string, string[]> Errors { get; init; } = [];
    [ReadOnly(true)]
    public Guid? RequestId { get; init; }

    public Result()
    {
    }

    public static Result Success(ResultCode? resultCode = null, Guid? requestId = null)
    {
        resultCode ??= ResultCode.Success;

        return new()
        {
            Code = resultCode.Code,
            Description = resultCode.Description,
            CategorizedCode = resultCode.ToString(),
            RequestId = requestId
        };
    }

    
    public static Result<TResponse> Success<TResponse>(TResponse? data, ResultCode? resultCode = null, Guid? requestId = null)
    {
        resultCode ??= ResultCode.Success;

        return new()
        {
            Code = resultCode.Code,
            Description = resultCode.Description,
            CategorizedCode = resultCode.ToString(),
            Data = data,
            RequestId = requestId
        };
    }

    public static Result Failure(ResultCode resultCode, Dictionary<string, string[]>? errors = null, Guid? requestId = null) => new()
    {
        Code = resultCode.Code,
        Description = resultCode.Description,
        CategorizedCode =  resultCode.ToString(),
        Errors = errors ?? [],
        RequestId = requestId
    };

    public static Result<TResponse> Failure<TResponse>(ResultCode resultCode, Dictionary<string, string[]>? errors = null, Guid? requestId = null) => new()
        {
            Code = resultCode.Code,
            Description = resultCode.Description,
            CategorizedCode = resultCode.ToString(),
            Errors = errors ?? [],
            RequestId = requestId
        };
}