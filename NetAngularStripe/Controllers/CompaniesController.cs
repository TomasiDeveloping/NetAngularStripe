using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

/// <summary>
///     Controller for handling company-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CompaniesController(ICompanyRepository companyRepository, ILogger<CompaniesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Retrieves customer details by their unique ID.
    /// </summary>
    /// <param name="customerId">The unique ID of the customer.</param>
    /// <returns>Returns the details of the customer.</returns>
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CompanyDto>> GetCustomer(Guid customerId)
    {
        try
        {
            // Retrieve customer details asynchronously
            var customer = await companyRepository.GetCustomerAsync(customerId);

            // If customer not found, return 404
            // Else return customer details
            return customer is null ? NotFound() : Ok(customer);
        }
        catch (Exception e)
        {
            // Log any exception that occurs
            logger.LogError(e, e.Message);

            // Return 500 Internal Server Error status code
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}