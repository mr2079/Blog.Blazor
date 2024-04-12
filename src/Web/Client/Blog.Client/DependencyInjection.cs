using Blazored.LocalStorage;
using Blog.Client.Utils.Interceptors;
using Blog.Client.Utils.Providers;
using Blog.Client.Utils.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blog.Client;

public static class DependencyInjection
{
	public static IServiceCollection AddAppServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddTransient<AuthInterceptorHandler>();

		services.AddScoped<HttpService>();

		services.AddHttpClient<HttpService>(client => client.BaseAddress = new Uri(configuration["BaseAddress"]!))
			.AddHttpMessageHandler<AuthInterceptorHandler>();

		services.AddBlazoredLocalStorage();

		services.AddBlazorBootstrap();

		services.AddScoped<AuthenticationStateProvider, AppAuthStateProvider>();

		services.AddAuthorizationCore();

		return services;
	}
}