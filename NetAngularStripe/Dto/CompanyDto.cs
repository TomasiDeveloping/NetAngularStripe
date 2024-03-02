namespace NetAngularStripe.Dto;

/// <summary>
///     Represents the data transfer object for a company.
/// </summary>
public record CompanyDto(Guid Id, string CompanyName, string? StripeCustomerId);