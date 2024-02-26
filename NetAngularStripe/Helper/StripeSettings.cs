namespace NetAngularStripe.Helper;

public class StripeSettings
{
    public required string PrivateKey { get; set; }

    public required string PublicKey { get; set; }

    public required string WHSecret { get; set; }

    public required string FrontendSuccessUrl { get; set; }

    public required string StripeSuccessUrl { get; set; }

    public required string FrontendCancelUrl { get; set; }

    public required string StripeCancelUrl { get; set; }
}