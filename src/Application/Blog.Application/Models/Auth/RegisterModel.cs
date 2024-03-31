using System.ComponentModel.DataAnnotations;

namespace Blog.Application.Models.Auth;

public class RegisterModel
{
	[Display(Name = "نام")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	[MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
	public string FirstName { get; set; } = string.Empty;

	[Display(Name = "نام خانوادگی")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	[MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
	public string LastName { get; set; } = string.Empty;

	[Display(Name = "شماره موبایل")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	[RegularExpression(@"^(09[0-9]{9})$", ErrorMessage = "فرمت {0} وارد شده صحیح نمی باشد")]
	public string PhoneNumber { get; set; } = string.Empty;

	[Display(Name = "کلمه عبور")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	[MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
	public string Password { get; set; } = string.Empty;

	[Display(Name = "تکرار کلمه عبور")]
	[Compare(nameof(Password), ErrorMessage = "{0} با {1} برابر نیست")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	public string Confirm { get; set; } = string.Empty;

	[Display(Name = "موافقت با شرایط و ضوابط")]
	[Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
	public bool IsRulesAccepted { get; set; }
}