namespace NetAngularStripe.Dto.Stripe;

/// <summary>
///     Represents the response data for a Stripe checkout session.
/// </summary>
public sealed record StripeCheckoutResponse(string SessionId, string StripePublicKey);