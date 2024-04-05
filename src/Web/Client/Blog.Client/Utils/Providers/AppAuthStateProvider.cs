using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blog.Client.Utils.Providers;

public class AppAuthStateProvider(
    ILocalStorageService localStorage
    ) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await localStorage.GetItemAsStringAsync("_at");
        var refreshToken = await localStorage.GetItemAsStringAsync("_rt");

        if (string.IsNullOrWhiteSpace(accessToken)
            || string.IsNullOrWhiteSpace(refreshToken))
        {
	        var unauthenticated = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
	        NotifyAuthenticationStateChanged(Task.FromResult(unauthenticated));

	        return unauthenticated;
        }

		var identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!))!;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}