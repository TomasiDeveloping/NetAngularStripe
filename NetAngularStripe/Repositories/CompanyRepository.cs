using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Repositories;

/// <summary>
///     Repository implementation for accessing company-related data.
/// </summary>
public class CompanyRepository(ApplicationContext context) : ICompanyRepository
{
    /// <summary>
    ///     Retrieves a company by its unique identifier asynchronously.
    /// </summary>
    /// <param name="customerId">The unique identifier of the company.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the company DTO if found;
    ///     otherwise, null.
    /// </returns>
    public async Task<CompanyDto?> GetCustomerAsync(Guid customerId)
    {
        var company = await context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return company is null
            ? null
            : new CompanyDto(company.Id, company.CompanyName, company.StripeCustomerId);
    }
}