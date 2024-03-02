namespace NetAngularStripe.Dto.Stripe;

/// <summary>
///     Represents the request data for accessing the Stripe Customer Portal.
/// </summary>
public sealed record StripeCustomerPortalRequest(string StripeCustomerId);