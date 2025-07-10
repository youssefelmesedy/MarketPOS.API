namespace MarketPOS.Shared.DTOs;

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public bool HasNextPage => (PageIndex * PageSize) < TotalCount;
    public bool HasPreviousPage => PageIndex > 1;

    public PagedResultDto(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}
