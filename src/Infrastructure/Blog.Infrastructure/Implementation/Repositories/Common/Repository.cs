using Blog.Application.Contracts.Persistence.Common;
using Blog.Domain.Entites;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Implementation.Repositories.Common;

public class Repository<TEntity, TKey> : QueryRepository<TEntity, TKey>, ICommandRepository<TEntity, TKey>
	where TEntity : class, IBaseIdentityEntity
{
	public Repository(ApplicationContext context) :
		base(context)
	{ }

	public void Add(TEntity entity)
	{
		try
		{
			Context.Entry(entity).State = EntityState.Added;
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Add(params TEntity[] entities)
	{
		try
		{
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
        }
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Add(IEnumerable<TEntity> entities)
	{
		try
		{
            foreach (var entity in entities)
            {
			    Context.Entry(entity).State = EntityState.Added;
            }
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Update(TEntity entity)
	{
		try
		{
			Context.Entry(entity).State = EntityState.Modified;
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Update(params TEntity[] entities)
	{
		try
		{
			Context.Entry(entities).State = EntityState.Modified;
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Update(IEnumerable<TEntity> entities)
	{
		try
		{
			Context.Entry(entities).State = EntityState.Modified;
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Delete(TEntity entity)
	{
		try
		{
			entity.IsDeleted = true;
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Delete(params TEntity[] entities)
	{
		try
		{
			entities.ToList().ForEach(e => e.IsDeleted = true);
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public void Delete(IEnumerable<TEntity> entities)
	{
		try
		{
			entities.ToList().ForEach(e => e.IsDeleted = true);
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public async Task Delete(TKey id)
	{
		try
		{
			var entity = await GetByIdAsync(id);
			if (entity == null) throw new ArgumentNullException();
			Delete(entity);
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}

	public Task<int> CommitAsync()
	{
		try
		{
			return Context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			throw new Exception(typeof(TEntity).Name, e);
		}
	}
}