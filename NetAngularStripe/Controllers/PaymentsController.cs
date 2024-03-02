using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetAngularStripe.Data.Entities;
using NetAngularStripe.Dto.Stripe;
using NetAngularStripe.Helper;
using NetAngularStripe.Interfaces;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;
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
            var configuration = new ConfigurationCreateOptions()
            {
                BusinessProfile = new ConfigurationBusinessProfileOptions
                {
                    Headline = ""
                },
                Features = new ConfigurationFeaturesOptions()
                {
                    CustomerUpdate = new ConfigurationFeaturesCustomerUpdateOptions()
                    {
                        Enabled = true,
                        AllowedUpdates = StripeHelpers.CustomerUpdate.GetAllAllowedUpdates()
                    },
                    SubscriptionUpdate = new ConfigurationFeaturesSubscriptionUpdateOptions()
                    {
                        DefaultAllowedUpdates = [StripeHelpers.SubscriptionUpdate.DefaultAllowedUpdates.Price],
                        Enabled = true,
                        Products =
                        [
                            new ConfigurationFeaturesSubscriptionUpdateProductOptions
                            {
                                Prices = ["price_1OpqVTBuMtj0mRD4l8g8Cqiv"],
                                Product = "prod_PfArP1Npd6EYWr"
                            }
                        ]
                    },
                    PaymentMethodUpdate = new ConfigurationFeaturesPaymentMethodUpdateOptions()
                    {
                        Enabled = true
                    },
                    InvoiceHistory = new ConfigurationFeaturesInvoiceHistoryOptions()
                    {
                        Enabled = true
                    },
                    SubscriptionCancel = new ConfigurationFeaturesSubscriptionCancelOptions()
                    {
                        Enabled = true,
                        Mode = StripeHelpers.SubscriptionCancel.Mode.AtPeriodEnd,
                        CancellationReason = new ConfigurationFeaturesSubscriptionCancelCancellationReasonOptions()
                        {
                            Enabled = true,
                            Options = StripeHelpers.SubscriptionCancel.CancellationReason.GetAllCancellationReasonOptions()
                        }
                    },
                    SubscriptionPause = new ConfigurationFeaturesSubscriptionPauseOptions()
                    {
                        Enabled = false
                    }
                },
            };
      
            var configureService = new ConfigurationService();
            var configureConfiguration = await configureService.CreateAsync(configuration);

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = request.StripeCustomerId,
                ReturnUrl = "http://localhost:4200",
                Configuration = configureConfiguration.Id,
                Locale = "de"
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