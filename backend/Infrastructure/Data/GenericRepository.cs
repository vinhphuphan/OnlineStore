// GenericRepository<T> provides a reusable implementation of IGenericRepository for basic data operations.
// Supports CRUD, filtering, sorting, and projecting data using the Specification Pattern.

using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity) => context.Set<T>().Add(entity);

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = context.Set<T>().AsQueryable();
        query = spec.ApplyCriteria(query);
        return await query.CountAsync();
    }

    public async Task<bool> ExistsAsync(int id) => await context.Set<T>().AnyAsync(e => e.Id == id);

    public async Task<T?> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        => await ApplySpecification(spec).FirstOrDefaultAsync();

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
        => await ApplySpecification(spec).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<T>> ListAllAsync()
        => await context.Set<T>().ToListAsync();

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        => await ApplySpecification(spec).ToListAsync();

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
        => await ApplySpecification(spec).ToListAsync();

    public void Remove(T entity) => context.Set<T>().Remove(entity);

    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        => SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
       => SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
}
