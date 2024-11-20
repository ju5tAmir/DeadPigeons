using System.Security.Claims;

namespace Service.Preference;

public interface IPreferenceService
{
    Dto.PreferenceResponse GetById(Guid id, ClaimsPrincipal principal);
    // Task Update(Guid preferenceId, Guid userId, ClaimsPrincipal principal, Dto.PreferenceUpdateRequest data);
}