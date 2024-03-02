using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

/// <summary>
///     Controller for handling license type-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LicenseTypesController(
    ILicenseTypeRepository licenseTypeRepository,
    ILogger<LicenseTypesController> logger) : ControllerBase
{
    /// <summary>
    ///     Retrieves all available subscriptions.
    /// </summary>
    /// <returns>Returns a list of all available subscriptions.</returns>
    [HttpGet]
    public async Task<ActionResult<List<LicenseTypeDto>>> GetSubscriptions()
    {
        try
        {
            // Retrieve all available license types asynchronously
            var licenseTypes = await licenseTypeRepository.GetSubscriptionsAsync();

            // If no license types found, return 204 No Content
            // Else return the list of license types
            return licenseTypes.Count == 0 ? NoContent() : Ok(licenseTypes);
        }
        catch (Exception e)
        {
            // Log any exception that occurs
            logger.LogError(e, e.Message);

            // Return 500 Internal Server Error status code
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    ///     Retrieves the details of a specific subscription by its ID.
    /// </summary>
    /// <param name="licenseId">The unique ID of the subscription.</param>
    /// <returns>Returns the details of the specified subscription.</returns>
    [HttpGet("{licenseId:guid}")]
    public async Task<ActionResult<LicenseTypeDto>> GetSubscription(Guid licenseId)
    {
        try
        {
            // Retrieve license type details asynchronously
            var licenceType = await licenseTypeRepository.GetLicenseTypeAsync(licenseId);

            // If license type not found, return 404
            // Else return license type details
            return licenceType is null ? NotFound() : Ok(licenceType);
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