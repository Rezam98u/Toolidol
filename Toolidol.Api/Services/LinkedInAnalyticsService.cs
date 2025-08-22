using Microsoft.Extensions.Configuration;
using Toolidol.Api.Models.DTOs.LinkedInAnalytics;

namespace Toolidol.Api.Services
{
	public class LinkedInAnalyticsService : ILinkedInAnalyticsService
	{
		private readonly HttpService _httpService;
		private readonly ILinkedInOrganizationService _orgService;
		private readonly IConfiguration _configuration;

		public LinkedInAnalyticsService(HttpService httpService, ILinkedInOrganizationService orgService, IConfiguration configuration)
		{
			_httpService = httpService;
			_orgService = orgService;
			_configuration = configuration;
		}

		public async Task<OrganizationPageViewsResponse?> GetPageViewsAsync(CancellationToken cancellationToken = default)
		{
			if (IsMock())
			{
				return new OrganizationPageViewsResponse
				{
					Elements = new List<OrganizationPageViewElement>
					{
						new OrganizationPageViewElement { PageViews = 123, UniquePageViews = 90, TimeRange = new TimeRange { Start = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(), End = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() }, OrganizationalEntity = "urn:li:organization:123" }
					}
				};
			}
			var token = await _orgService.GetAccessTokenAsync(cancellationToken);
			var orgUrn = await _orgService.GetOrganizationUrnAsync(cancellationToken);
			// LinkedIn Organization page views (example endpoint placeholder)
			var path = "organizationPageStatistics";
			var query = new Dictionary<string, string>
			{
				["q"] = "organization",
				["organization"] = orgUrn
			};
			return await _httpService.GetFromLinkedInApiAsync<OrganizationPageViewsResponse>(path, token, query, cancellationToken);
		}

		public async Task<FollowersResponse?> GetFollowersAsync(CancellationToken cancellationToken = default)
		{
			if (IsMock())
			{
				return new FollowersResponse
				{
					Elements = new List<FollowersElement>
					{
						new FollowersElement { FollowerCounts = 2500, FollowerGains = 20, FollowerLosses = 5, TimeRange = new TimeRange { Start = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(), End = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() }, OrganizationalEntity = "urn:li:organization:123" }
					}
				};
			}
			var token = await _orgService.GetAccessTokenAsync(cancellationToken);
			var orgUrn = await _orgService.GetOrganizationUrnAsync(cancellationToken);
			var path = "organizationFollowerStatistics";
			var query = new Dictionary<string, string>
			{
				["q"] = "organizationalEntity",
				["organizationalEntity"] = orgUrn
			};
			return await _httpService.GetFromLinkedInApiAsync<FollowersResponse>(path, token, query, cancellationToken);
		}

		public async Task<OrganizationShareStatisticsResponse?> GetEngagementAsync(CancellationToken cancellationToken = default)
		{
			if (IsMock())
			{
				return new OrganizationShareStatisticsResponse
				{
					Elements = new List<OrganizationShareStatistic>
					{
						new OrganizationShareStatistic
						{
							TotalShareStatistics = new TotalShareStatistics { ShareCount = 10, ImpressionCount = 500, ClickCount = 42, Engagement = 0.08, LikeCount = 30, CommentCount = 4, ShareMentionsCount = 1 },
							TimeRange = new TimeRange { Start = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(), End = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() },
							OrganizationalEntity = "urn:li:organization:123"
						}
					}
				};
			}
			var token = await _orgService.GetAccessTokenAsync(cancellationToken);
			var orgUrn = await _orgService.GetOrganizationUrnAsync(cancellationToken);
			var path = "organizationalEntityShareStatistics";
			var query = new Dictionary<string, string>
			{
				["q"] = "organizationalEntity",
				["organizationalEntity"] = orgUrn
			};
			return await _httpService.GetFromLinkedInApiAsync<OrganizationShareStatisticsResponse>(path, token, query, cancellationToken);
		}

		private bool IsMock()
		{
			return string.Equals(_configuration["LinkedIn:Mock"], "true", StringComparison.OrdinalIgnoreCase);
		}
	}
}

