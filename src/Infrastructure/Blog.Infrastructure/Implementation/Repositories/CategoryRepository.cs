using Blog.Application.Contracts.Persistence;
using Blog.Domain.Entites;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Implementation.Repositories.Common;

namespace Blog.Infrastructure.Implementation.Repositories;

public class CategoryRepository
	: Repository<Category, Guid>, ICategoryRepository
{
	public CategoryRepository(ApplicationContext context)
		: base(context) { }
}