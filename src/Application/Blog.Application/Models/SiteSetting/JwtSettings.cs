﻿namespace Blog.Application.Models.SiteSetting;

public class JwtSettings
{
	public required string ValidIssuer { get; set; }
	public required string ValidAudience { get; set; }
	public required string SecurityKey { get; set; }
	public required int TokenValidityInMinutes { get; set; }
	public required int RefreshTokenValidityInDays { get; set; }
}