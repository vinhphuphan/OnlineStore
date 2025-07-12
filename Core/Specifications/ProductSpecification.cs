// ProductSpecifications.cs

using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams specParams) : base(
        x => (string.IsNullOrWhiteSpace(specParams.Search) || x.Name.ToLower().Contains(specParams.Search))
          && (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand))
          && (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))
    )
    {
        // Pagination
        ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        // Sort
        switch (specParams.Sort?.Trim().ToLower())
        {
            case "priceasc": AddOrderBy(x => x.Price); break;
            case "pricedesc": AddOrderByDesc(x => x.Price); break;
            case "namedesc": AddOrderByDesc(x => x.Name); break;
            default: AddOrderBy(x => x.Name); break;
        }
    }
}

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}

public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
