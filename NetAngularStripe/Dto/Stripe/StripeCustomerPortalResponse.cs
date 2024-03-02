namespace NetAngularStripe.Dto.Stripe;

/// <summary>
///     Represents the response data containing the URL to access the Stripe Customer Portal.
/// </summary>
public sealed record StripeCustomerPortalResponse(string CustomerPortalUrl);