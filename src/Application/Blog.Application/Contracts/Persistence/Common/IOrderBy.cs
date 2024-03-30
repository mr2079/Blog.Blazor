namespace Blog.Application.Contracts.Persistence.Common;

public interface IOrderBy
{
    dynamic Expression { get; }
}
