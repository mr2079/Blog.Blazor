namespace Blog.Shared.Models.Auth;

public class RefreshModel
{
	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }
}