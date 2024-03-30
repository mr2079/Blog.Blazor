using System.Linq.Expressions;
using Blog.Application.Contracts.Persistence.Common;

namespace Blog.Infrastructure.Implementation;

public class OrderBy<TEntity, TProperty> : IOrderBy
{
    private readonly Expression<Func<TEntity, TProperty>> _expression;

    public OrderBy(Expression<Func<TEntity, TProperty>> expression)
    {
        _expression = expression;
    }

    public dynamic Expression => _expression;
}
