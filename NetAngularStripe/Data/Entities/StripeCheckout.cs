using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

/// <summary>
/// Represents a Stripe checkout entity.
/// </summary>
public class StripeCheckout
{
    /// <summary>
    /// Gets or sets the unique identifier of the Stripe checkout.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated license type.
    /// </summary>
    public Guid LicenceTypeId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated company.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the Stripe session ID associated with the checkout.
    /// </summary>
    public required string StripeSessionId { get; set; }

    /// <summary>
    /// Gets or sets the status of the checkout.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the checkout was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Represents different statuses of a Stripe checkout.
/// </summary>
public class StripeStatus
{
    /// <summary>
    /// Represents the "Created" status.
    /// </summary>
    public const string Created = "Created";

    /// <summary>
    /// Represents the "Canceled" status.
    /// </summary>
    public const string Canceled = "Canceled";

    /// <summary>
    /// Represents the "Success" status.
    /// </summary>
    public const string Success = "Success";
}