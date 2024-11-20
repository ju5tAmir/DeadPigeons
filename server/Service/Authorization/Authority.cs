using System.Security.Claims;
using Service.Security;

namespace Service.Authorization;

public class Authority: IAuthority
{
    // Access control for users
    // Admin is allowed 
    public void AuthorizeAndThrow(ClaimsPrincipal principal, Guid userId)
    {
        if (principal?.Identity == null || !principal.Identity.IsAuthenticated) throw new AuthenticationError();
        if (principal.IsInRole(Role.Admin)) return;
        if (principal.GetUserId() != userId.ToString()) throw new ForbiddenError();
    }
}