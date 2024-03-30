using Blog.Application.Contracts.Persistence.Common;
using Blog.Domain.Entites;

namespace Blog.Application.Contracts.Persistence;

public interface ICategoryRepository
	: IQueryRepository<Category, Guid>, ICommandRepository<Category, Guid>;