namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class RequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IHasRequestId, IRequest<TResponse> where TResponse : Result, new()
{
    public abstract Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = new CancellationToken());
}