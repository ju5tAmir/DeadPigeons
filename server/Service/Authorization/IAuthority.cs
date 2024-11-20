using System.Security.Claims;

namespace Service.Authorization;

public interface IAuthority
{
    public void AuthorizeAndThrow(ClaimsPrincipal principal, Guid userId);
}