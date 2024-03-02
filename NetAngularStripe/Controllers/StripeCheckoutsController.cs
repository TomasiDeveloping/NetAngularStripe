using Microsoft.AspNetCore.Mvc;
using NetAngularStripe.Data.Entities;
using NetAngularStripe.Interfaces;

namespace NetAngularStripe.Controllers;

/// <summary>
///     Controller for handling Stripe checkout-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class StripeCheckoutsController(
    IStripeCheckoutRepository stripeCheckoutRepository,
    ILogger<StripeCheckoutsController> logger) : ControllerBase
{
    /// <summary>
    ///     Retrieves all Stripe checkouts.
    /// </summary>
    /// <returns>Returns a list of all Stripe checkouts.</returns>
    [HttpGet]
    public async Task<ActionResult<List<StripeCheckout>>> GetStripeCheckouts()
    {
        try
        {
            // Retrieve all Stripe checkouts asynchronously
            var stripeCheckouts = await stripeCheckoutRepository.GetStripeCheckoutAsync();

            // If no Stripe checkouts found, return 204 No Content
            // Else return the list of Stripe checkouts
            return stripeCheckouts.Count == 0 ? NoContent() : Ok(stripeCheckouts);
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