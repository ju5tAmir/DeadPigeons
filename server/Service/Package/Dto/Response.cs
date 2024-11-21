namespace Service.Package.Dto;

public record PackageResponse(
    Guid PackageId,
    int NumberOfFields,
    decimal Price
    );