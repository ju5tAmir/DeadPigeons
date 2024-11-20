using FluentValidation;

namespace Service.Preference.Dto;

public record PreferenceUpdateRequest(
    bool ifBalanceIsNegative,
    bool ifPlayerWon,
    string NotificationType);
    
public class DraftFormDataValidator : AbstractValidator<PreferenceUpdateRequest>
{
    public DraftFormDataValidator()
    {
        RuleFor(x => x.ifBalanceIsNegative)
            .Must(value => value == true || value == false)
            .WithMessage("The 'ifBalanceIsNegative' field must be a valid boolean.");

        RuleFor(x => x.ifPlayerWon)
            .Must(value => value == true || value == false)
            .WithMessage("The 'ifPlayerWon' field must be a valid boolean.");

        RuleFor(x => x.NotificationType)
            .Must(value => value == NotificationType.Email || 
                           value == NotificationType.Phone ||
                           value == NotificationType.None ||
                           value == NotificationType.All
                           )
            .WithMessage("The 'NotificationType' field must be one of the following values: 'All', 'Email', 'Phone', 'None'.");
    }
}