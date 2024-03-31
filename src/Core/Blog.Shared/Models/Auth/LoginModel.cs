using System.ComponentModel.DataAnnotations;

namespace Blog.Shared.Models.Auth;

public class LoginModel
{
	[Display(Name = "شماره موبایل")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	[RegularExpression(@"^(09[0-9]{9})$", ErrorMessage = "فرمت {0} وارد شده صحیح نمی باشد")]
	public string PhoneNumber { get; set; } = string.Empty;

	[Display(Name = "کلمه عبور")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	public string Password { get; set; } = string.Empty;

	[Display(Name = "مرا به خاطر بسپار")]
	public bool RememberMe { get; set; }
}