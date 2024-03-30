using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Blog.Client.Utils.Interceptors;

public class AuthInterceptorHandler(
	ILocalStorageService localStorage,
	HttpClient http
	) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var accessToken = await localStorage.GetItemAsStringAsync("_at", cancellationToken);
		request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
		var response = await base.SendAsync(request, cancellationToken);

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			// TODO: Refresh tokens and send the request again

			await localStorage.SetItemAsStringAsync("_at", "", cancellationToken);
			await localStorage.SetItemAsStringAsync("_rt", "", cancellationToken);
		}

		return response;
	}
}