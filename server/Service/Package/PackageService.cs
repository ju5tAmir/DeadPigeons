using Microsoft.EntityFrameworkCore;
using Service.Package.Dto;
using Service.Package.Utils;
using Service.Repositories;

namespace Service.Package;

public class PackageService(IRepository<DataAccess.Entities.Package> repository): IPackageService
{
    public async Task<PackageResponse> GetPackageById(Guid packageId)
    {
        var package = await repository
            .Query()
            .Where(p => p.PackageId == packageId)
            .FirstOrDefaultAsync();

        if (package == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = packageId });
        }

        return PackageMapper.ToPackageResponse(package);
    }
}