namespace NetAngularStripe.Dto.Stripe;

public sealed record StripeCheckoutRequestDto(Guid CompanyId, Guid LicenseTypeId, string StripePriceId);