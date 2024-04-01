using Blog.Domain.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data;

public class ApplicationContext : IdentityDbContext<User, Role, Guid>
{
	public ApplicationContext() { }
	public ApplicationContext(
		DbContextOptions<ApplicationContext> options
		) : base(options) { }

	#region DbSets

	public DbSet<Article> Articles => Set<Article>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Comment> Comments => Set<Comment>();

	#endregion

	#region Configuration

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<IBaseIdentityEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreateDate = DateTime.Now;
					entry.Entity.UpdateDate = entry.Entity.CreateDate;
					entry.Entity.IsDeleted = false;
					break;
				case EntityState.Modified:
					entry.Entity.UpdateDate = DateTime.Now;
					break;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	#endregion
}
