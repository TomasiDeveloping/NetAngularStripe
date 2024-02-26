using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Repositories;

public class LicenseTypeRepository(ApplicationContext context) : ILicenseTypeRepository
{
    public async Task<LicenseTypeDto?> GetLicenseTypeAsync(Guid subscriptionId)
    {
        var licenseType = await context.LicenseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);

        return licenseType is null
            ? null
            : new LicenseTypeDto(licenseType.Id, licenseType.Name, licenseType.Description, licenseType.Price,
                licenseType.StripePriceId);
    }


    public async Task<List<LicenseTypeDto>> GetSubscriptionsAsync()
    {
        var licenseTypes = await context.LicenseTypes
            .AsNoTracking()
            .ToListAsync();

        var licenseTypeDtoList = new List<LicenseTypeDto>();

        if (licenseTypes.Count == 0) return licenseTypeDtoList;

        licenseTypeDtoList.AddRange(licenseTypes.Select(licenseType => new LicenseTypeDto(licenseType.Id,
            licenseType.Name, licenseType.Description, licenseType.Price, licenseType.StripePriceId)));

        return licenseTypeDtoList;
    }
}