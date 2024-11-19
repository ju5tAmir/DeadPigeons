using FluentValidation;

namespace Service.Auth.Dto;

public record RegisterRequest(string FirsName, string LastName, string Username, string Email, string PhoneNumber, string Password);

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirsName)
            .NotEmpty()
            .WithMessage("FirstName is required.")
            .MinimumLength(2)
            .WithMessage("FirstName must be at least 2 characters long.")
            .MaximumLength(64)
            .WithMessage("FirstName cannot exceed 64 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required.")
            .MinimumLength(2)
            .WithMessage("LastName must be at least 2 characters long.")
            .MaximumLength(64)
            .WithMessage("LastName cannot exceed 64 characters.");
        
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(4)
            .WithMessage("Username must be at least 4 characters long.")
            .MaximumLength(32)
            .WithMessage("Username cannot exceed 32 characters.")
            .Matches(@"^[a-zA-Z0-9_.]+$")
            .WithMessage("Username can only contain letters, numbers, underscores, or periods.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be a valid international format (e.g., +1234567890).");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]")
            .WithMessage("Password must contain at least one special character.");
    }
}


public record LoginRequest(string Email, string Password);

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}