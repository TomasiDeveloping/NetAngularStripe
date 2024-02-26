namespace NetAngularStripe.Dto.Stripe;

public sealed record StripeCheckoutResponse(string SessionId, string StripePublicKey);