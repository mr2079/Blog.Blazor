using Blog.Application.Contracts.Persistence.Common;
using Blog.Domain.Entites;

namespace Blog.Application.Contracts.Persistence;

public interface IArticleRepository
	: IQueryRepository<Article, Guid>, ICommandRepository<Article, Guid>;