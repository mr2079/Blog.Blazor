using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entites;

public class Role : IdentityRole<Guid>, IBaseIdentityEntity
{
	public string Sku { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public DateTime CreateDate { get; set; }
	public DateTime? UpdateDate { get; set; }
	public bool IsDeleted { get; set; }
}