using System.Text.Json.Serialization;

namespace Toolidol.Api.Models.DTOs.LinkedInAnalytics
{
	public class OrganizationShareStatisticsResponse
	{
		[JsonPropertyName("elements")]
		public List<OrganizationShareStatistic> Elements { get; set; } = new();
	}

	public class OrganizationShareStatistic
	{
		[JsonPropertyName("totalShareStatistics")]
		public TotalShareStatistics? TotalShareStatistics { get; set; }

		[JsonPropertyName("timeRange")]
		public TimeRange? TimeRange { get; set; }

		[JsonPropertyName("organizationalEntity")]
		public string? OrganizationalEntity { get; set; }
	}

	public class TotalShareStatistics
	{
		[JsonPropertyName("shareCount")]
		public long? ShareCount { get; set; }

		[JsonPropertyName("impressionCount")]
		public long? ImpressionCount { get; set; }

		[JsonPropertyName("clickCount")]
		public long? ClickCount { get; set; }

		[JsonPropertyName("engagement")]
		public double? Engagement { get; set; }

		[JsonPropertyName("likeCount")]
		public long? LikeCount { get; set; }

		[JsonPropertyName("commentCount")]
		public long? CommentCount { get; set; }

		[JsonPropertyName("shareMentionsCount")]
		public long? ShareMentionsCount { get; set; }
	}

	public class TimeRange
	{
		[JsonPropertyName("start")]
		public long? Start { get; set; }

		[JsonPropertyName("end")]
		public long? End { get; set; }
	}
}

