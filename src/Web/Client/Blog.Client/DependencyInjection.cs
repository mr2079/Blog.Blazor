using Blazored.LocalStorage;
using Blog.Client.Utils.Interceptors;
using Blog.Client.Utils.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blog.Client;

public static class DependencyInjection
{
	public static IServiceCollection AddAppServices(this IServiceCollection services)
	{
		services.AddTransient<AuthInterceptorHandler>();

		services.AddHttpClient("AppHttpClient",
				client => client.BaseAddress = new Uri(""))
			.AddHttpMessageHandler<AuthInterceptorHandler>();

		services.AddBlazoredLocalStorage();

		services.AddScoped<AuthenticationStateProvider, AppAuthStateProvider>();

		services.AddAuthorizationCore();

		return services;
	}
}