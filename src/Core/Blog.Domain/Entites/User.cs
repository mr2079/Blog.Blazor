﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entites;

public class User : IdentityUser<Guid>, IBaseIdentityEntity
{
	public string Sku { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public DateTime CreateDate { get; set; }
	public DateTime? UpdateDate { get; set; }
	public bool IsDeleted { get; set; }

	public bool IsConfirmed { get; set; }

	[MaxLength(255)]
	public string ImageName { get; set; } = "Default.jpg";

	[MaxLength(255)]
	public string FirstName { get; set; } = string.Empty;

	[MaxLength(255)]
	public string LastName { get; set; } = string.Empty;

	[NotMapped] public string? DisplayName => $"{FirstName} {LastName}";

	[MaxLength(600)]
	public string? Description { get; set; } = string.Empty;

	// Navigation properties
	public ICollection<Comment>? Comments { get; set; }
	public ICollection<Article>? Articles { get; set; }

	public string? RefreshToken { get; set; }
	public DateTime? RefreshTokenExpiryTime { get; set; }
}

