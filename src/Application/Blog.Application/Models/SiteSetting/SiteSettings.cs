using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Models.SiteSetting;

public class SiteSettings
{
    public JwtSettings Jwt { get; set; } = null!;
    public List<UserSeed> DefaultUsers { get; set; } = null!;
    public SmsSettings Sms { get; set; } = null!;
    public SmtpSettings Smtp { get; set; } = null!;
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
    public TimeSpan EmailConfirmationTokenProviderLifespan { get; set; }
    public PasswordOptions PasswordOptions { get; set; } = null!;
    public LockoutOptions LockoutOptions { get; set; } = null!;
}