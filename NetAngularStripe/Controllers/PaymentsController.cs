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

/// <summary>
///     Controller for handling payment-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PaymentsController(
    IStripeCheckoutRepository stripeCheckoutRepository,
    ILicenseTypeRepository licenseTypeRepository,
    ILogger<PaymentsController> logger,
    IOptions<StripeSettings> stripeSettings)
    : ControllerBase
{
    /// <summary>
    ///     Initiates a new checkout session.
    /// </summary>
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(StripeCheckoutRequestDto request)
    {
        try
        {
            // Configure options for checkout session creation
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
                // Create a new checkout session
                var session = await service.CreateAsync(options);

                // Create response for client
                var response = new StripeCheckoutResponse(session.Id, stripeSettings.Value.PublicKey);

                // Add the new checkout session to the repository
                await stripeCheckoutRepository.AddAsync(new StripeCheckout
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
                // Log any Stripe-specific exceptions
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

    /// <summary>
    ///     Initiates a new customer portal session.
    /// </summary>
    [HttpPost("customerPortal")]
    public async Task<IActionResult> CustomerPortal(StripeCustomerPortalRequest request)
    {
        try
        {
            // Configure options for customer portal session creation
            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = request.StripeCustomerId,
                ReturnUrl = "http://localhost:4200",
                Locale = "de"
            };

            if (!stripeSettings.Value.WithStandardCustomerPortal)
            {
                // Configure options for customer portal session creation
                var configuration = await CreateCustomerPortalOptions();
                options.Configuration = configuration.Id;
            }

            var service = new Stripe.BillingPortal.SessionService();
            var session = await service.CreateAsync(options);

            // Create response for client
            var response = new StripeCustomerPortalResponse(session.Url);

            return Ok(response);
        }
        catch (StripeException e)
        {
            Console.WriteLine(e.StripeError.Message);
            return BadRequest();
        }
    }


    /// <summary>
    ///     Processes webhook events from Stripe.
    /// </summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            // Construct the event from the incoming webhook data
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], stripeSettings.Value.WHSecret, 300,
                (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                false);

            // Handle the event based on its type
            switch (stripeEvent.Type)
            {
                case Events.CustomerSubscriptionUpdated:
                    if (stripeEvent.Data.Object is Subscription subscription)
                        await stripeCheckoutRepository.HandleUpdateSubscription(subscription);
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


    /// <summary>
    ///     Redirects the user to a success page after completing a checkout session.
    /// </summary>
    [HttpGet("success")]
    public async Task<IActionResult> CheckoutSuccess([FromQuery] string sessionId)
    {
        try
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            // Handle success of checkout session
            await stripeCheckoutRepository.HandleSuccessAsync(sessionId, session.CustomerId);

            // Redirect to success page
            return Redirect(stripeSettings.Value.FrontendSuccessUrl);
        }
        catch (Exception ex)
        {
            logger.LogError("Error into order Controller on route /success " + ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    ///     Redirects the user to a canceled page if the checkout session is canceled.
    /// </summary>
    [HttpGet("canceled")]
    public async Task<IActionResult> CheckoutCanceled([FromQuery] string sessionId)
    {
        try
        {
            // Handle cancellation of checkout session
            await stripeCheckoutRepository.HandleCancel(sessionId);

            // Redirect to canceled page
            return Redirect(stripeSettings.Value.FrontendCancelUrl);
        }
        catch (Exception ex)
        {
            logger.LogError("error into order Controller on route /canceled " + ex.Message);
            return BadRequest();
        }
    }

    private async Task<Configuration> CreateCustomerPortalOptions()
    {
        // Fetch subscription-related license types asynchronously from the repository
        var licenceTypes = await licenseTypeRepository.GetSubscriptionsAsync();

        // Configure options for customer portal session creation
        var configuration = new ConfigurationCreateOptions
        {
            // Configure business profile
            BusinessProfile = new ConfigurationBusinessProfileOptions
            {
                Headline = ""
            },
            // Configure features of the portal
            Features = new ConfigurationFeaturesOptions
            {
                // Enable customer update feature
                CustomerUpdate = new ConfigurationFeaturesCustomerUpdateOptions
                {
                    Enabled = true,
                    AllowedUpdates = StripeHelpers.CustomerUpdate.GetAllAllowedUpdates()
                },
                // Configure subscription update feature
                SubscriptionUpdate = new ConfigurationFeaturesSubscriptionUpdateOptions
                {
                    DefaultAllowedUpdates = [StripeHelpers.SubscriptionUpdate.DefaultAllowedUpdates.Price],
                    Enabled = true,
                    Products = []
                },
                // Enable payment method update feature
                PaymentMethodUpdate = new ConfigurationFeaturesPaymentMethodUpdateOptions
                {
                    Enabled = true
                },
                // Enable invoice history feature
                InvoiceHistory = new ConfigurationFeaturesInvoiceHistoryOptions
                {
                    Enabled = true
                },
                // Configure subscription cancel feature
                SubscriptionCancel = new ConfigurationFeaturesSubscriptionCancelOptions
                {
                    Enabled = true,
                    Mode = StripeHelpers.SubscriptionCancel.Mode.AtPeriodEnd,
                    CancellationReason = new ConfigurationFeaturesSubscriptionCancelCancellationReasonOptions
                    {
                        Enabled = true,
                        Options = StripeHelpers.SubscriptionCancel.CancellationReason
                            .GetAllCancellationReasonOptions()
                    }
                },
                // Disable subscription pause feature
                SubscriptionPause = new ConfigurationFeaturesSubscriptionPauseOptions
                {
                    Enabled = false
                }
            }
        };

        // Iterate through each license type and add corresponding product options for subscription update feature
        foreach (var licenceType in licenceTypes)
        {
            configuration.Features.SubscriptionUpdate.Products.Add(new ConfigurationFeaturesSubscriptionUpdateProductOptions()
            {
                // Set the price ID associated with the license type
                Prices = [licenceType.StripePriceId],
                // Set the product ID associated with the license type
                Product = licenceType.StripeProductId
            });
        }

        // Create a new configuration service and return the asynchronously created configuration
        var service = new ConfigurationService();
        return await service.CreateAsync(configuration);
    }
}