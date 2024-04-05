using System.ComponentModel.DataAnnotations;

namespace Blog.Shared.Models.Auth;

public class LogoutModel
{
	public string? UserName { get; set; }
}