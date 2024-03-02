using NetAngularStripe.Dto;

namespace NetAngularStripe.Interfaces;

/// <summary>
///     Interface for accessing company-related data.
/// </summary>
public interface ICompanyRepository
{
    /// <summary>
    ///     Retrieves a company by its unique identifier asynchronously.
    /// </summary>
    /// <param name="customerId">The unique identifier of the company.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the company DTO if found;
    ///     otherwise, null.
    /// </returns>
    Task<CompanyDto?> GetCustomerAsync(Guid customerId);
}