using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

/// <summary>
///     Controller for handling license-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LicensesController(
    ILicenseRepository licenseRepository,
    ILogger<LicensesController> logger) : ControllerBase
{
    /// <summary>
    ///     Retrieves the subscription details of a company portal by its ID.
    /// </summary>
    /// <param name="companyId">The unique ID of the company.</param>
    /// <returns>Returns the subscription details of the company portal.</returns>
    [HttpGet("{companyId:guid}")]
    public async Task<ActionResult<LicenseDto>> GetCompanyPortalSubscription(Guid companyId)
    {
        try
        {
            // Retrieve company portal subscription details asynchronously
            var companyLicense =
                await licenseRepository.GetCompanyLicenseAsync(companyId);

            // If company license not found, return 404
            // Else return company license details
            return companyLicense is null ? NotFound() : Ok(companyLicense);
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