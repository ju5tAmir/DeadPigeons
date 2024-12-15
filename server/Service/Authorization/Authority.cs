using System.Reflection.Metadata.Ecma335;
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
    

    public async Task AuthorizeAndThrowAsync(ClaimsPrincipal principal)
    {
        if (principal?.Identity == null || !principal.Identity.IsAuthenticated)
        {
            throw new AuthenticationError();
        }

        if (principal.IsInRole(Role.Admin)) return;
    }

    /// <summary>
    /// Authorizes access based on the current user's role or user ID and throws an exception if access is forbidden.
    /// </summary>
    /// <param name="currentPrincipal">The current user's claims principal.</param>
    /// <param name="targetPrincipalGuid">The GUID of the user whose access is being authorized.</param>
    /// <exception cref="ForbiddenError">Thrown if the user is not authorized to access the resource.</exception>
    public void AuthorizeAccessAndThrow(ClaimsPrincipal currentPrincipal, Guid targetPrincipalGuid)
    {
        // If the user is an admin, bypass all further checks and allow access.
        if (currentPrincipal?.IsInRole(Role.Admin) == true)
        {
            return;
        }

        // Check if the current user's ID matches the target user's ID.
        // If they do not match, throw a ForbiddenError to deny access.
        if (currentPrincipal?.GetUserId().ToString() != targetPrincipalGuid.ToString())
        {
            throw new ForbiddenError();
        }
    }


}