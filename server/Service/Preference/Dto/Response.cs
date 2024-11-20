namespace Service.Preference.Dto;

public record PreferenceResponse(
    bool? ifBalanceIsNegative,
    bool? ifPlayerWon,
    string? NotificationType);