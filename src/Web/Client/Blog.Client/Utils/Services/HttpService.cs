using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Blog.Client.Utils.Services;

public class HttpService(HttpClient httpClient)
{
	public async Task<TResponse?> PostAsync<TResponse>(string path, object body)
	{
		var response = await httpClient.PostAsJsonAsync(path, body);
		var content = await response.Content.ReadAsStringAsync();

		return JsonConvert.DeserializeObject<TResponse>(content);
	}
}