using NetAngularStripe.Dto;

namespace NetAngularStripe.Interfaces;

/// <summary>
///     Interface for accessing license type-related data.
/// </summary>
public interface ILicenseTypeRepository
{
    /// <summary>
    ///     Retrieves a license type by its unique identifier asynchronously.
    /// </summary>
    /// <param name="licenceId">The unique identifier of the license type.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the license type DTO if found;
    ///     otherwise, null.
    /// </returns>
    Task<LicenseTypeDto?> GetLicenseTypeAsync(Guid licenceId);

    /// <summary>
    ///     Retrieves all subscription license types asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of license type DTOs.</returns>
    Task<List<LicenseTypeDto>> GetSubscriptionsAsync();
}