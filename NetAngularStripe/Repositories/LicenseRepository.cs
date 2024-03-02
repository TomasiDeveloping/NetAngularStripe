using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Repositories;

/// <summary>
///     Repository implementation for accessing license-related data.
/// </summary>
public class LicenseRepository(ApplicationContext context) : ILicenseRepository
{
    /// <summary>
    ///     Retrieves a company's license by its unique identifier asynchronously.
    /// </summary>
    /// <param name="companyId">The unique identifier of the company.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the license DTO if found;
    ///     otherwise, null.
    /// </returns>
    public async Task<LicenseDto?> GetCompanyLicenseAsync(Guid companyId)
    {
        var license = await context.Licenses
            .Include(l => l.LicenseType)
            .FirstOrDefaultAsync(ps => ps.CompanyId == companyId);

        if (license == null) return null;

        var companySubscription = new LicenseDto(license.ExpiredAt,
            license.LicenseType!.Name, license.LicenseType.Price,
            license.LicenseType.Description);

        return companySubscription;
    }
}