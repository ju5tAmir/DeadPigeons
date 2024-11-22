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
}