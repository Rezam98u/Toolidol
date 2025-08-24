namespace Toolidol.Api.Options
{
	public class LinkedInOptions
	{
		public string ApiBaseUrl { get; set; } = "https://api.linkedin.com/v2";
		public string OrganizationId { get; set; } = string.Empty;
		public string AccessToken { get; set; } = string.Empty;
		public bool Mock { get; set; } = true;
	}
}

