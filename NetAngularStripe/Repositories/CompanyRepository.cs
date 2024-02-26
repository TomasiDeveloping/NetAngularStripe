using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Repositories;

public class CompanyRepository(ApplicationContext context) : ICompanyRepository
{
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