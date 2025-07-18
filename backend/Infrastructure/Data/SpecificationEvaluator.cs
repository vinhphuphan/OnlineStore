// SpecificationEvaluator<T> applies specification rules to an IQueryable query.
// Handles filtering, sorting, projection, distinct, and pagination based on the given specification.

using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDesc != null)
            query = query.OrderByDescending(spec.OrderByDesc);

        if (spec.IsDistinct)
            query = query.Distinct();

        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        return query;
    }

    public static IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> spec)
    {
        var query = inputQuery;

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDesc != null)
            query = query.OrderByDescending(spec.OrderByDesc);


        var projected = spec.Select != null ? query.Select(spec.Select) : query.Cast<TResult>();

        if (spec.IsDistinct)
            projected = projected.Distinct();

        if (spec.IsPagingEnabled)
            projected = projected.Skip(spec.Skip).Take(spec.Take);

        return projected;
    }
}
