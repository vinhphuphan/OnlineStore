// Pagination<T> represents a paginated result for a list of items.
// Parameters include metadata like page index, page size, total count, and the actual data.

namespace API.RequestHelpers;

public class Pagination<T>(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public int Count { get; set; } = count;
    public IReadOnlyList<T> Data { get; set; } = data;
}
