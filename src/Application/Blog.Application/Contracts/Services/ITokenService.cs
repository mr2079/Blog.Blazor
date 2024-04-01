using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.Application.Contracts.Services;

public interface ITokenService
{
	JwtSecurityToken CreateAccessToken(List<Claim>? authClaims);
	string GenerateToken(int length);
	ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}