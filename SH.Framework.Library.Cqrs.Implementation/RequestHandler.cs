namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IHasRequestId, IRequest<Result<TResponse>>
{
    public abstract Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = new());
}

public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IHasRequestId, IRequest<Result>
{
    public abstract Task<Result> HandleAsync(TRequest request,
        CancellationToken cancellationToken = new());
}