using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Blog.Application.Contracts.Services;
using Blog.Application.Models.SiteSetting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Infrastructure.Implementation.Services;

public class TokenService(
	IOptionsSnapshot<SiteSettings> siteSettings) : ITokenService
{

	public JwtSecurityToken CreateAccessToken(List<Claim>? authClaims)
	{
		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteSettings.Value.Jwt.SecurityKey));

		var token = new JwtSecurityToken(
			issuer: siteSettings.Value.Jwt.ValidIssuer,
			audience: siteSettings.Value.Jwt.ValidAudience,
			expires: DateTime.Now.AddMinutes(siteSettings.Value.Jwt.TokenValidityInMinutes),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);

		return token;
	}

	public string GenerateToken(int length)
	{
		var randomNumber = new byte[length];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false,
			ValidateIssuer = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siteSettings.Value.Jwt.SecurityKey)),
			ValidateLifetime = false
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
		if (securityToken is not JwtSecurityToken jwtSecurityToken ||
			!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			throw new SecurityTokenException("Invalid token");

		return principal;
	}

	public bool ValidateToken(string token)
	{
		if (string.IsNullOrEmpty(token))
			return false;

		var key = Encoding.UTF8.GetBytes(siteSettings.Value.Jwt.SecurityKey);

		try
		{
			new JwtSecurityTokenHandler()
				.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;

			return true;
		}
		catch
		{
			return false;
		}
	}
}