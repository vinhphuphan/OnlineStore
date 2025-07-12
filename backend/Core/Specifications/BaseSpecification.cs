// BaseSpecification<T> implements the core logic for filtering, sorting, and pagination in queries.
// BaseSpecification<T, TResult> extends it with projection support using a Select expression.

using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria = null) : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria => criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDesc { get; private set; }

    public bool IsDistinct { get; private set; }
    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>> OrderByExpr) => OrderBy = OrderByExpr;

    protected void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpr) => OrderByDesc = OrderByDescExpr;

    protected void ApplyDistinct() => IsDistinct = true;

    protected void ApplyPagination(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if (Criteria != null)
            query = query.Where(Criteria);
        return query;
    }

}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria = null) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>>? expr) => Select = expr;
}