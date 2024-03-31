using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entites;

public class Category : BaseEntity
{
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public virtual ICollection<Article>? Articles { get; set; }
}
