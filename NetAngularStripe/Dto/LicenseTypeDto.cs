namespace NetAngularStripe.Dto;

public record LicenseTypeDto(Guid Id, string Name, string Description, decimal Price, string StripePriceId);