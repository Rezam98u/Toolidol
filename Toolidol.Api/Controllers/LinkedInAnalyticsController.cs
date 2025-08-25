using Microsoft.AspNetCore.Mvc;
using Toolidol.Api.Services;

namespace Toolidol.Api.Controllers
{
	[ApiController]
	[Route("api/linkedin-analytics")]
	public class LinkedInAnalyticsController : ControllerBase
	{
		private readonly ILinkedInAnalyticsService _analyticsService;

		public LinkedInAnalyticsController(ILinkedInAnalyticsService analyticsService)
		{
			_analyticsService = analyticsService;
		}

		[HttpGet("page-views")]
		public async Task<IActionResult> GetPageViews(CancellationToken cancellationToken)
		{
			var result = await _analyticsService.GetPageViewsAsync(cancellationToken);
			return Ok(result);
		}

		[HttpGet("followers")]
		public async Task<IActionResult> GetFollowers(CancellationToken cancellationToken)
		{
			var result = await _analyticsService.GetFollowersAsync(cancellationToken);
			return Ok(result);
		}

		[HttpGet("engagement")]
		public async Task<IActionResult> GetEngagement(CancellationToken cancellationToken)
		{
			var result = await _analyticsService.GetEngagementAsync(cancellationToken);
			return Ok(result);
		}
	}
}

