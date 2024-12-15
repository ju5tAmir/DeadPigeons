using System.Security.Claims;

namespace Service.Authorization;

public interface IAuthority
{
    public void AuthorizeAndThrow(ClaimsPrincipal principal, Guid userId);
    public Task AuthorizeAndThrowAsync(ClaimsPrincipal principal);
    public void AuthorizeAccessAndThrow(ClaimsPrincipal currentPrincipal, Guid targetPrincipalGuid);
}