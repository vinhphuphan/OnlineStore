// ProductSpecParams holds query parameters for filtering, sorting, and paginating products.
// Supports brand/type filtering, keyword search, and page size constraints.

namespace Core.Specifications;

public class ProductSpecParams
{
    // Brands
    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
        set => _brands = [.. value.SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))];
    }

    // Types
    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        set => _types = [.. value.SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))];
    }

    // Sort
    public string? Sort;

    // Search
    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }

    // Pagination
    private readonly int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
