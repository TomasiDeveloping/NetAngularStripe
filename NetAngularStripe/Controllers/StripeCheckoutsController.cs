using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Data.Entities;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StripeCheckoutsController(
    IStripeCheckoutRepository stripeCheckoutRepository,
    ILogger<StripeCheckoutsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<StripeCheckout>>> GetStripeCheckouts()
    {
        try
        {
            var stripeCheckouts = await stripeCheckoutRepository.GetStripeCheckoutAsync();

            return stripeCheckouts.Count == 0 ? NoContent() : Ok(stripeCheckouts);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}