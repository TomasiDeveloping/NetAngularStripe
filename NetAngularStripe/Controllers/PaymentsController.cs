using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetAngularStripe.Data.Entities;
using NetAngularStripe.Dto.Stripe;
using NetAngularStripe.Helper;
using NetAngularStripe.Interfaces;
using Stripe;
using Stripe.Checkout;
using Subscription = Stripe.Subscription;

namespace NetAngularStripe.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(IStripeCheckoutRepository stripeCheckoutRepository, ILogger<PaymentsController> logger, IOptions<StripeSettings> stripeSettings)
    : ControllerBase
{
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(StripeCheckoutRequestDto request)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        Price = request.StripePriceId
                    }
                ],
                Mode = "subscription",
                SuccessUrl = stripeSettings.Value.StripeSuccessUrl + "{CHECKOUT_SESSION_ID}",
                CancelUrl = stripeSettings.Value.StripeCancelUrl + "{CHECKOUT_SESSION_ID}"
            };
            var service = new SessionService();
            try
            {
                var session = await service.CreateAsync(options);

                var response = new StripeCheckoutResponse(session.Id, stripeSettings.Value.PublicKey);

                await stripeCheckoutRepository.AddAsync(new StripeCheckout()
                {
                    StripeSessionId = session.Id,
                    CompanyId = request.CompanyId,
                    LicenceTypeId = request.LicenseTypeId,
                    Status = StripeStatus.Created,
                    CreatedAt = DateTime.Now
                });

                return Ok(response);
            }
            catch (StripeException ex)
            {
                logger.LogError("Stripe error: {error}", ex.StripeError);
                return BadRequest(ex.StripeError.Message);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest();
        }
    }


    [HttpPost("customerPortal")]
    public async Task<IActionResult> CustomerPortal(StripeCustomerPortalRequest request)
    {
        try
        {
            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = request.StripeCustomerId,
                ReturnUrl = "http://localhost:4200"
            };
            var service = new Stripe.BillingPortal.SessionService();
            var session = await service.CreateAsync(options);

            var response = new StripeCustomerPortalResponse(session.Url);

            return Ok(response);
        }
        catch (StripeException e)
        {
            Console.WriteLine(e.StripeError.Message);
            return BadRequest();
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], stripeSettings.Value.WHSecret, 300,
                (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                false);

            switch (stripeEvent.Type)
            {
                case Events.CustomerSubscriptionUpdated:
                    if (stripeEvent.Data.Object is Subscription subscription)
                    {
                        await stripeCheckoutRepository.HandleUpdateSubscription(subscription);
                    }
                    break;
                default:
                    logger.LogInformation("{0}: Unhandled event type: {1}", DateTime.Now, stripeEvent.Type);
                    break;
            }

            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }

    [HttpGet("success")]
    public async Task<IActionResult> CheckoutSuccess([FromQuery] string sessionId)
    {
        try
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            await stripeCheckoutRepository.HandleSuccessAsync(sessionId, session.CustomerId);


            return Redirect(stripeSettings.Value.FrontendSuccessUrl);
        }
        catch (Exception ex)
        {
            logger.LogError("Error into order Controller on route /success " + ex.Message);
            return BadRequest();
        }
    }

    [HttpGet("canceled")]
    public async Task<IActionResult> CheckoutCanceled([FromQuery] string sessionId)
    {
        try
        {
            await stripeCheckoutRepository.HandleCancel(sessionId);
            // Insert here failure data in data base
            return Redirect(stripeSettings.Value.FrontendCancelUrl);
        }
        catch (Exception ex)
        {
            logger.LogError("error into order Controller on route /canceled " + ex.Message);
            return BadRequest();
        }
    }
}