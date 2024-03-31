using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entites;

public interface IBaseIdentityEntity
{
	[Key]
	public Guid Id { get; set; }
	public string Sku { get; set; }
	public string Slug { get; set; }
	public DateTime CreateDate { get; set; }
	public DateTime? UpdateDate { get; set; }
	public bool IsDeleted { get; set; }
}

public abstract class BaseEntity : IBaseIdentityEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
}
