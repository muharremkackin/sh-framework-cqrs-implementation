namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class Request: IRequest<Result>, IHasRequestId
{
    private readonly Guid _requestId = Guid.NewGuid();
    public Guid RequestId() => _requestId;
}

public abstract class Request<TResponse>: IRequest<Result<TResponse>>, IHasRequestId
{
    private readonly Guid _requestId = Guid.NewGuid();
    public Guid RequestId() => _requestId;
}