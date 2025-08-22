using System.Text.Json.Serialization;

namespace Toolidol.Api.Models.DTOs.LinkedInAnalytics
{
	public class OrganizationPageViewsResponse
	{
		[JsonPropertyName("elements")]
		public List<OrganizationPageViewElement> Elements { get; set; } = new();
	}

	public class OrganizationPageViewElement
	{
		[JsonPropertyName("pageViews")]
		public long? PageViews { get; set; }

		[JsonPropertyName("uniquePageViews")]
		public long? UniquePageViews { get; set; }

		[JsonPropertyName("timeRange")]
		public TimeRange? TimeRange { get; set; }

		[JsonPropertyName("organizationalEntity")]
		public string? OrganizationalEntity { get; set; }
	}
}

