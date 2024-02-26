using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LicenseTypesController(
    ILicenseTypeRepository licenseTypeRepository,
    ILogger<LicenseTypesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LicenseTypeDto>>> GetSubscriptions()
    {
        try
        {
            var licenseTypes = await licenseTypeRepository.GetSubscriptionsAsync();

            return licenseTypes.Count == 0 ? NoContent() : Ok(licenseTypes);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{licenseId:guid}")]
    public async Task<ActionResult<LicenseTypeDto>> GetSubscription(Guid licenseId)
    {
        try
        {
            var licenceType = await licenseTypeRepository.GetLicenseTypeAsync(licenseId);

            return licenceType is null ? NotFound() : Ok(licenceType);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}