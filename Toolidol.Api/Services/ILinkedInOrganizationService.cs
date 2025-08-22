namespace Toolidol.Api.Services
{
	public interface ILinkedInOrganizationService
	{
		Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
		Task<string> GetOrganizationUrnAsync(CancellationToken cancellationToken = default);
	}
}

