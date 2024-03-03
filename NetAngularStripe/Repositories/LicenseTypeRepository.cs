using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Repositories;

/// <summary>
///     Repository implementation for accessing license type-related data.
/// </summary>
public class LicenseTypeRepository(ApplicationContext context) : ILicenseTypeRepository
{
    /// <summary>
    ///     Retrieves a license type by its unique identifier asynchronously.
    /// </summary>
    /// <param name="subscriptionId">The unique identifier of the license type.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the license type DTO if found;
    ///     otherwise, null.
    /// </returns>
    public async Task<LicenseTypeDto?> GetLicenseTypeAsync(Guid subscriptionId)
    {
        var licenseType = await context.LicenseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);

        return licenseType is null
            ? null
            : new LicenseTypeDto(licenseType.Id, licenseType.Name, licenseType.Description, licenseType.Price,
                licenseType.StripePriceId, licenseType.StripeProductId);
    }

    /// <summary>
    ///     Retrieves all subscription license types asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of license type DTOs.</returns>
    public async Task<List<LicenseTypeDto>> GetSubscriptionsAsync()
    {
        var licenseTypes = await context.LicenseTypes
            .AsNoTracking()
            .ToListAsync();

        var licenseTypeDtoList = new List<LicenseTypeDto>();

        if (licenseTypes.Count == 0) return licenseTypeDtoList;

        licenseTypeDtoList.AddRange(licenseTypes.Select(licenseType => new LicenseTypeDto(licenseType.Id,
            licenseType.Name, licenseType.Description, licenseType.Price, licenseType.StripePriceId, licenseType.StripeProductId)));

        return licenseTypeDtoList;
    }
}