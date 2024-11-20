using System.Security.Claims;
using Service.Preference.Dto;

namespace Service.Preference;

public interface IPreferenceService
{
    PreferenceResponse GetById(Guid id, ClaimsPrincipal principal);
    Task<PreferenceResponse> Update(Guid preferenceId, ClaimsPrincipal principal, PreferenceUpdateRequest data);
}