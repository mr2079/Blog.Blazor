namespace Blog.Application.Models.Auth;

public class TokenModel
{
	public string? AccessToken { get; set; }
	public DateTime? AccessTokenExpiryDateTime { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime? RefreshTokenExpiryDateTime { get; set; }
}