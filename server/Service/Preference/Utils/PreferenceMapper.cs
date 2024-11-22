using DataAccess.Entities;
using Service.Preference.Dto;

namespace Service.Preference.Utils;

public class PreferenceMapper
{
    public static PreferenceResponse ToResponse(DataAccess.Entities.Preference preference)
    {
        return new PreferenceResponse(
            preference.IfBalanceIsNegative,
            preference.IfPlayerWon,
            preference.NotificationType
        );
    }

    public static DataAccess.Entities.Preference GetDefaultPreferences(User user)
    {
        return new DataAccess.Entities.Preference()
        {
            UserSettingsId = Guid.NewGuid().ToString(),
            UserId = user.Id,
            IfBalanceIsNegative = true,
            IfPlayerWon = true,
            NotificationType = "Email"
        };
    }
}