using NetAngularStripe.Dto;

namespace NetAngularStripe.Interfaces;

public interface ILicenseTypeRepository
{
    Task<LicenseTypeDto?> GetLicenseTypeAsync(Guid licenceId);

    Task<List<LicenseTypeDto>> GetSubscriptionsAsync();
}