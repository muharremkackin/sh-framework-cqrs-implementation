using SH.Framework.Library.Cqrs;

namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class Request: IRequest<Result>, IHasRequestId
{
    public Guid RequestId => Guid.NewGuid();
}

public abstract class Request<TResponse>: IRequest<Result<TResponse>>, IHasRequestId
{
    public Guid RequestId => Guid.NewGuid();
}