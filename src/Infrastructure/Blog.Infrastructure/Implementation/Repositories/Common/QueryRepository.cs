using System.Linq.Expressions;
using Blog.Application.Contracts.Persistence.Common;
using Blog.Domain.Entites;
using Blog.Domain.Enums;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Blog.Infrastructure.Implementation.Repositories.Common;

public class QueryRepository<TEntity, TKey> : IQueryRepository<TEntity, TKey>
    where TEntity : class, IBaseIdentityEntity
{
    protected readonly ApplicationContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public QueryRepository(ApplicationContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Tuple<List<IOrderBy>, OrderTypeEnum?>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? thenIncludes = null,
            bool disableTracking = true)
    {
        try
        {
            var query = DbSet.AsQueryable();
            if (disableTracking) query = query.AsNoTracking();
            if (thenIncludes != null) query = thenIncludes(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null)
            {
                foreach (var orderByItem in orderBy.Item1)
                {
                    query = orderBy.Item2 == OrderTypeEnum.Descending ?
                        Queryable.OrderByDescending(query, orderByItem.Expression) :
                        Queryable.OrderBy(query, orderByItem.Expression);
                }
            }

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(typeof(TEntity).Name, ex);
        }
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
	    try
	    {
		    var query = DbSet.AsQueryable();
		    if (predicate != null) query = query.Where(predicate);

		    return await query.CountAsync();
	    }
	    catch (Exception ex)
	    {
		    throw new Exception(typeof(TEntity).Name, ex);
	    }
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        try
        {
            return (await GetAsync(
                    predicate: x => x.Id!.Equals(id),
                    disableTracking: false))
                .ToList()
                .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(typeof(TEntity).Name, ex);
        }
    }
}