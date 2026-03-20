namespace SH.Framework.Library.Cqrs.Implementation;

public class ListRequest<TResponse> : Request<ListResult<TResponse>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public List<ListRequestFilter> Filters { get; set; } = [];
    public List<ListRequestOrder> Orders { get; set; } = [];
}