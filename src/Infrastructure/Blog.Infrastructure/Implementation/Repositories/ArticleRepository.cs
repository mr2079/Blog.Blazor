using Blog.Application.Contracts.Persistence;
using Blog.Domain.Entites;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Implementation.Repositories.Common;

namespace Blog.Infrastructure.Implementation.Repositories;

public class ArticleRepository
	: Repository<Article, Guid>, IArticleRepository
{
	public ArticleRepository(ApplicationContext context)
		: base(context) { }
}