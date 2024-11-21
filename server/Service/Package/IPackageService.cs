using Service.Package.Dto;

namespace Service.Package;

public interface IPackageService
{
    Task<PackageResponse> GetPackageById(Guid packageId);
}