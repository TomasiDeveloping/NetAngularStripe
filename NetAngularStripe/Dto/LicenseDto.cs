namespace NetAngularStripe.Dto;

/// <summary>
///     Represents the data transfer object for a license.
/// </summary>
public record LicenseDto(DateTime ExpiredAt, string SubscriptionName, decimal Price, string Description);