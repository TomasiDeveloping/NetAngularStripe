using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Dto;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LicensesController(
    ILicenseRepository licenseRepository,
    ILogger<LicensesController> logger) : ControllerBase
{
    [HttpGet("{companyId:guid}")]
    public async Task<ActionResult<LicenseDto>> GetCompanyPortalSubscription(Guid companyId)
    {
        try
        {
            var companyLicense =
                await licenseRepository.GetCompanyLicenseAsync(companyId);

            return companyLicense is null ? NotFound() : Ok(companyLicense);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}