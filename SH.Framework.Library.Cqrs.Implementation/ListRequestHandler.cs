using SH.Framework.Library.Cqrs.Implementation;

namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class ListRequestHandler<TRequest, TQueryResult>: RequestHandler<TRequest, ListResult<TQueryResult>> where TRequest : ListRequest<TQueryResult>
{
    
}