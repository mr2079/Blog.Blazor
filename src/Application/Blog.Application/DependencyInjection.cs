using System.Reflection;
using Blog.Application.Models.SiteSetting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.Configure<SiteSettings>(configuration.Bind);

		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		return services;
	}
}