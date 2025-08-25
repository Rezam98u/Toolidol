using System.Diagnostics;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Toolidol.Api.Services.Http
{
	public class LoggingDelegatingHandler : DelegatingHandler
	{
		private readonly ILogger<LoggingDelegatingHandler> _logger;

		public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
		{
			_logger = logger;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var sw = Stopwatch.StartNew();
			try
			{
				_logger.LogInformation("HTTP {Method} {Url}", request.Method, request.RequestUri);
				var response = await base.SendAsync(request, cancellationToken);
				sw.Stop();
				_logger.LogInformation("HTTP {StatusCode} {Url} in {Elapsed}ms", (int)response.StatusCode, request.RequestUri, sw.ElapsedMilliseconds);
				return response;
			}
			catch (Exception ex)
			{
				sw.Stop();
				_logger.LogError(ex, "HTTP error for {Url} after {Elapsed}ms", request.RequestUri, sw.ElapsedMilliseconds);
				throw;
			}
		}
	}
}

