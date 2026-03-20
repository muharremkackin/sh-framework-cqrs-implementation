namespace SH.Framework.Library.Cqrs.Implementation;

public class ListResult<TDto>
{
    public List<TDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    public static ListResult<TDto> Create(List<TDto> items, int totalCount, int page, int pageSize)
    {
        return new ListResult<TDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}