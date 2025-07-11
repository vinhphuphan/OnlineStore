// ProductSpecifications.cs

using Core.Entities;

namespace Core.Specifications;

#region ProductSpecification - Filtering + Sorting
public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? brand, string? type, string? sort) : base(
        x => (string.IsNullOrWhiteSpace(brand) || x.Brand == brand)
          && (string.IsNullOrWhiteSpace(type) || x.Type == type))
    {
        switch (sort?.ToLower())
        {
            case "priceasc": AddOrderBy(x => x.Price); break;
            case "pricedesc": AddOrderByDesc(x => x.Price); break;
            case "namedesc": AddOrderByDesc(x => x.Name); break;
            default: AddOrderBy(x => x.Name); break;
        }
    }
}
#endregion

#region BrandListSpecification - Distinct Brand
public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}
#endregion

#region TypeListSpecification - Distinct Type
public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
#endregion
