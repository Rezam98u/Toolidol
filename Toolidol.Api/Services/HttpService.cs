using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Toolidol.Api.Options;

namespace Toolidol.Api.Services
{
	public class HttpService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		private readonly IOptions<LinkedInOptions> _linkedInOptions;

		public HttpService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IOptions<LinkedInOptions> linkedInOptions)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
			_linkedInOptions = linkedInOptions;
		}

		public async Task<T?> GetFromLinkedInApiAsync<T>(string relativePath, string accessToken, IDictionary<string, string>? queryParameters = null, CancellationToken cancellationToken = default)
		{
			var client = _httpClientFactory.CreateClient();
			var baseUrl = _linkedInOptions.Value.ApiBaseUrl;
			var baseUri = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), relativePath.TrimStart('/'));
			var queryParams = new Dictionary<string, string?>();
			if (queryParameters != null)
			{
				foreach (var kv in queryParameters)
				{
					queryParams[kv.Key] = kv.Value;
				}
			}
			var uri = new Uri(QueryHelpers.AddQueryString(baseUri.ToString(), queryParams!));

			using var request = new HttpRequestMessage(HttpMethod.Get, uri);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
			using var response = await client.SendAsync(request, cancellationToken);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
		}
	}
}

