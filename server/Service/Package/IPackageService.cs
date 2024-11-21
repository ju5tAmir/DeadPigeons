using System.Security.Claims;
using Service.Package.Dto;

namespace Service.Package;

public interface IPackageService
{
    Task<List<PackageResponse>> GetAllPackages();
    Task<PackageResponse> GetPackageById(Guid packageId);
    Task<PackageResponse> CreatePackage(PackageRequest data);
    Task<bool> DeletePackageById(Guid packageId);
}