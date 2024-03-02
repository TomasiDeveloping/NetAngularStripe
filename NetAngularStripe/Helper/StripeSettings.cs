namespace NetAngularStripe.Helper;

/// <summary>
///     Represents the settings required for Stripe integration.
/// </summary>
public class StripeSettings
{
    /// <summary>
    ///     Gets or sets the private key for Stripe.
    /// </summary>
    public required string PrivateKey { get; set; }

    /// <summary>
    ///     Gets or sets the public key for Stripe.
    /// </summary>
    public required string PublicKey { get; set; }

    /// <summary>
    ///     Gets or sets the webhook secret for Stripe.
    /// </summary>
    public required string WHSecret { get; set; }

    /// <summary>
    ///     Gets or sets the frontend success URL.
    /// </summary>
    public required string FrontendSuccessUrl { get; set; }

    /// <summary>
    ///     Gets or sets the Stripe success URL.
    /// </summary>
    public required string StripeSuccessUrl { get; set; }

    /// <summary>
    ///     Gets or sets the frontend cancel URL.
    /// </summary>
    public required string FrontendCancelUrl { get; set; }

    /// <summary>
    ///     Gets or sets the Stripe cancel URL.
    /// </summary>
    public required string StripeCancelUrl { get; set; }
}