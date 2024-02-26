namespace NetAngularStripe.Dto;

public record LicenseDto(DateTime ExpiredAt, string SubscriptionName, decimal Price, string Description);