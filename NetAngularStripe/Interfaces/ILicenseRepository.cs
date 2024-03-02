using NetAngularStripe.Dto;

namespace NetAngularStripe.Interfaces;

/// <summary>
///     Interface for accessing license-related data.
/// </summary>
public interface ILicenseRepository
{
    /// <summary>
    ///     Retrieves a company's license by its unique identifier asynchronously.
    /// </summary>
    /// <param name="companyId">The unique identifier of the company.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the license DTO if found;
    ///     otherwise, null.
    /// </returns>
    Task<LicenseDto?> GetCompanyLicenseAsync(Guid companyId);
}