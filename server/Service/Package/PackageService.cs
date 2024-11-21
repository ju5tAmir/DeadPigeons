using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Package.Dto;
using Service.Package.Utils;
using Service.Repositories;

namespace Service.Package;

public class PackageService(
    IRepository<DataAccess.Entities.Package> repository,
    IValidator<PackageRequest> validator
    ): IPackageService
{
    public async Task<List<PackageResponse>> GetAllPackages()
    {
        return await repository
            .Query()
            .Select(p => PackageMapper.ToResponse(p))
            .ToListAsync();
    }

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

        return PackageMapper.ToResponse(package);
    }

    public async Task<PackageResponse> CreatePackage(PackageRequest data)
    {
        await validator.ValidateAndThrowAsync(data);

        var package = new DataAccess.Entities.Package
        {
            PackageId = Guid.NewGuid(),
            Price = data.Price,
            NumberOfFields = data.NumberOfFields
        };

        await repository
            .Add(package);
        
        return PackageMapper.ToResponse(package);
    }

    public async Task<bool> DeletePackageById(Guid packageId)
    {
        var package = await repository
            .Query()
            .Where(p => p.PackageId == packageId)
            .FirstOrDefaultAsync();

        if (package == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = packageId });
        }

        await repository
            .Delete(package);

        return true;
    }
}