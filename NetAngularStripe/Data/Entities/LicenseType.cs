using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

/// <summary>
/// Represents a license type entity.
/// </summary>
public class LicenseType
{
    /// <summary>
    /// Gets or sets the unique identifier of the license type.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the license type.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the license type.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the license type.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the Stripe price ID associated with the license type.
    /// </summary>
    public required string StripePriceId { get; set; }

    /// <summary>
    /// Gets or sets the Stripe product ID associated with the license type.
    /// </summary>
    public required string StripeProductId { get; set; }

    /// <summary>
    /// Gets or sets the collection of licenses associated with the license type.
    /// </summary>
    public ICollection<License> Licenses { get; set; } = [];
}