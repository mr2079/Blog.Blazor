using Blog.Application.Contracts.Persistence;
using Blog.Domain.Entites;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Implementation.Repositories.Common;

namespace Blog.Infrastructure.Implementation.Repositories;

public class CommentRepository
	: Repository<Comment, Guid>, ICommentRepository
{
	public CommentRepository(ApplicationContext context)
		: base(context) { }
}