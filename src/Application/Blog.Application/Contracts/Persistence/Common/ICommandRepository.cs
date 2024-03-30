using Blog.Domain.Entites;

namespace Blog.Application.Contracts.Persistence.Common;

public interface ICommandRepository<in TEntity, in TKey> where TEntity : IBaseIdentityEntity
{
    void Add(TEntity entity);
    void Add(params TEntity[] entities);
    void Add(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Update(params TEntity[] entities);
    void Update(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void Delete(params TEntity[] entities);
    void Delete(IEnumerable<TEntity> entities);
    Task Delete(TKey id);
    Task<int> CommitAsync();
}