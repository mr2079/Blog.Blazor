namespace Blog.Application.Models.SiteSetting;

public class SmsSettings
{
    public string SendUrl { get; set; } = string.Empty;
    public string GetCreditUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public object Source { get; set; } = string.Empty;
}