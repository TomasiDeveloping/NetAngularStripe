using NetAngularStripe.Dto;

namespace NetAngularStripe.Interfaces;

public interface ICompanyRepository
{
    Task<CompanyDto?> GetCustomerAsync(Guid  customerId);
}