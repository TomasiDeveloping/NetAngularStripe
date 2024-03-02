namespace NetAngularStripe.Dto;

/// <summary>
///     Represents the data transfer object for a license type.
/// </summary>
public record LicenseTypeDto(Guid Id, string Name, string Description, decimal Price, string StripePriceId);