using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Toolidol.Api.Options;

namespace Toolidol.Api.Services
{
	public class LinkedInOrganizationService : ILinkedInOrganizationService
	{
		private readonly IConfiguration _configuration;
		private readonly IOptions<LinkedInOptions> _options;

		public LinkedInOrganizationService(IConfiguration configuration, IOptions<LinkedInOptions> options)
		{
			_configuration = configuration;
			_options = options;
		}

		public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
		{
			var token = _options.Value.AccessToken;
			return Task.FromResult(token);
		}

		public Task<string> GetOrganizationUrnAsync(CancellationToken cancellationToken = default)
		{
			var orgId = _options.Value.OrganizationId;
			// LinkedIn expects URN like: urn:li:organization:{id}
			var urn = orgId.StartsWith("urn:") ? orgId : $"urn:li:organization:{orgId}";
			return Task.FromResult(urn);
		}
	}
}

