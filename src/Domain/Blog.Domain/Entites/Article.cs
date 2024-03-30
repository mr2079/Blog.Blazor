using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Domain.Entites;

public class Article : BaseEntity
{
    public Guid CategoryId { get; set; }
    public Guid AuthorId { get; set; }
    [MaxLength(255)]
    public string ImageName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Tags { get; set; }
    public bool IsAccepted { get; set; } = false;
    public int View { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Comment>? Comments { get; set; }
}
