using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Repositories;

public class LicenseRepository(ApplicationContext context) : ILicenseRepository
{
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