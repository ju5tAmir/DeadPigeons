using FluentValidation;

namespace Service.Users.Dto;

public record UpdateUserRequest
(
    string FirstName, 
    string LastName, 
    string Email, 
    string PhoneNumber, 
    bool IsActive, 
    bool IsAutoplay
);


public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status must be specified.");

        RuleFor(x => x.IsAutoplay)
            .NotNull().WithMessage("IsAutoplay status must be specified.");
    }
}

