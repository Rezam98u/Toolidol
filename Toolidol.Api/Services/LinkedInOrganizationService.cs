using Microsoft.Extensions.Configuration;

namespace Toolidol.Api.Services
{
	public class LinkedInOrganizationService : ILinkedInOrganizationService
	{
		private readonly IConfiguration _configuration;

		public LinkedInOrganizationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
		{
			var token = _configuration["LinkedIn:AccessToken"] ?? string.Empty;
			return Task.FromResult(token);
		}

		public Task<string> GetOrganizationUrnAsync(CancellationToken cancellationToken = default)
		{
			var orgId = _configuration["LinkedIn:OrganizationId"] ?? string.Empty;
			// LinkedIn expects URN like: urn:li:organization:{id}
			var urn = orgId.StartsWith("urn:") ? orgId : $"urn:li:organization:{orgId}";
			return Task.FromResult(urn);
		}
	}
}

