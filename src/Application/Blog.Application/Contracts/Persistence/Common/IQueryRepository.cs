using System.Linq.Expressions;
using Blog.Domain.Entites;
using Blog.Domain.Enums;
using Microsoft.EntityFrameworkCore.Query;

namespace Blog.Application.Contracts.Persistence.Common;

public interface IQueryRepository<TEntity, in TKey> where TEntity : IBaseIdentityEntity
{
    Task<IReadOnlyList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Tuple<List<IOrderBy>, OrderTypeEnum?>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? thenIncludes = null,
            bool disableTracking = true);

    Task<int> GetCountAsync(
	    Expression<Func<TEntity, bool>>? predicate = null);

	Task<TEntity?> GetByIdAsync(TKey id);
}