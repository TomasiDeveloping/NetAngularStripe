using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController(ICompanyRepository companyRepository, ILogger<CompaniesController> logger)
    : ControllerBase
{
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CompanyDto>> GetCustomer(Guid customerId)
    {
        try
        {
            var customer = await companyRepository.GetCustomerAsync(customerId);

            return customer is null ? NotFound() : Ok(customer);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}