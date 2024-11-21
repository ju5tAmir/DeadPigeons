using FluentValidation;

namespace Service.Package.Dto;

public record PackageRequest(int NumberOfFields, decimal Price);

public class PackageRequestValidator : AbstractValidator<PackageRequest>
{
    public PackageRequestValidator()
    {
        RuleFor(x => x.NumberOfFields)
            .GreaterThan(0).WithMessage("Number of fields must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Number of fields must not exceed 100."); // Adjust the upper limit if needed

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.")
            .ScalePrecision(2, 10).WithMessage("Price must have a maximum of 10 digits in total and 2 decimal places.");
    }
}