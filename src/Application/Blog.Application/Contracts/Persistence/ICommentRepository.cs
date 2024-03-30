using Blog.Application.Contracts.Persistence.Common;
using Blog.Domain.Entites;

namespace Blog.Application.Contracts.Persistence;

public interface ICommentRepository
	: IQueryRepository<Comment, Guid>, ICommandRepository<Comment, Guid>;