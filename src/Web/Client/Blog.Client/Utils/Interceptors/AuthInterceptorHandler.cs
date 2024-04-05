using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Blog.Client.Utils.Services;
using Blog.Shared.Models.Auth;
using Blog.Shared.Models.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Blog.Client.Utils.Interceptors;

public class AuthInterceptorHandler(
	ILocalStorageService localStorage,
	IHttpClientFactory httpClientFactory,
	IConfiguration configuration,
	AuthenticationStateProvider stateProvider
	) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var accessToken = await localStorage.GetItemAsStringAsync("_at", cancellationToken);
		request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
		var response = await base.SendAsync(request, cancellationToken);

		if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

		var httpClient = httpClientFactory.CreateClient();
		httpClient.BaseAddress = new Uri(configuration["BaseAddress"]!);

		var httpRes = await httpClient.PostAsJsonAsync(
			"auth/refresh",
			new RefreshModel
			{
				AccessToken = await localStorage.GetItemAsStringAsync("_at", cancellationToken),
				RefreshToken = await localStorage.GetItemAsStringAsync("_rt", cancellationToken)
			}, cancellationToken);

		var refreshRes = JsonConvert
			.DeserializeObject<ApiResponse<TokenModel>>(await httpRes.Content.ReadAsStringAsync(cancellationToken));

		if (refreshRes is { Succeeded: false })
		{
			await localStorage.RemoveItemsAsync(new List<string> { "_at", "_rt" }, cancellationToken);
			await stateProvider.GetAuthenticationStateAsync();

			return new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
		}

		await localStorage.SetItemAsStringAsync("_at", refreshRes?.Content?.AccessToken!, cancellationToken);
		await localStorage.SetItemAsStringAsync("_rt", refreshRes?.Content?.RefreshToken!, cancellationToken);

		accessToken = await localStorage.GetItemAsStringAsync("_at", cancellationToken);

		request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
		response = await base.SendAsync(request, cancellationToken);

		return response;
	}
}