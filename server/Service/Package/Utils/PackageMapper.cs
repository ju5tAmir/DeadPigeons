using Service.Package.Dto;

namespace Service.Package.Utils;

public class PackageMapper
{
    public static PackageResponse ToPackageResponse(DataAccess.Entities.Package package)
    {
        return new PackageResponse(
            package.PackageId,
            package.NumberOfFields,
            package.Price
            );
    }
}