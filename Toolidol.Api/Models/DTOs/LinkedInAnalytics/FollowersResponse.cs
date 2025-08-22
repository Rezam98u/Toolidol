using System.Text.Json.Serialization;

namespace Toolidol.Api.Models.DTOs.LinkedInAnalytics
{
	// LinkedIn org follower statistics (aggregations by time and total)
	public class FollowersResponse
	{
		[JsonPropertyName("elements")]
		public List<FollowersElement> Elements { get; set; } = new();
	}

	public class FollowersElement
	{
		[JsonPropertyName("followerGains")]
		public long? FollowerGains { get; set; }

		[JsonPropertyName("followerLosses")]
		public long? FollowerLosses { get; set; }

		[JsonPropertyName("followerCounts")]
		public long? FollowerCounts { get; set; }

		[JsonPropertyName("timeRange")]
		public TimeRange? TimeRange { get; set; }

		[JsonPropertyName("organizationalEntity")]
		public string? OrganizationalEntity { get; set; }
	}
}

