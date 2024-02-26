using NetAngularStripe.Dto;

namespace NetAngularStripe.Interfaces;

public interface ILicenseRepository
{
    Task<LicenseDto?> GetCompanyLicenseAsync(Guid companyId);
}