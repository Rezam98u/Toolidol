using Toolidol.Api.Models.DTOs.LinkedInAnalytics;

namespace Toolidol.Api.Services
{
	public interface ILinkedInAnalyticsService
	{
		Task<OrganizationPageViewsResponse?> GetPageViewsAsync(CancellationToken cancellationToken = default);
		Task<FollowersResponse?> GetFollowersAsync(CancellationToken cancellationToken = default);
		Task<OrganizationShareStatisticsResponse?> GetEngagementAsync(CancellationToken cancellationToken = default);
	}
}

