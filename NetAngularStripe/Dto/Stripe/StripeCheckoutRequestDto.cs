namespace NetAngularStripe.Dto.Stripe;

/// <summary>
///     Represents the data transfer object for creating a Stripe checkout session.
/// </summary>
public sealed record StripeCheckoutRequestDto(Guid CompanyId, Guid LicenseTypeId, string StripePriceId);