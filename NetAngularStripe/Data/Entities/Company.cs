using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

/// <summary>
/// Represents a company entity.
/// </summary>
public class Company
{
    /// <summary>
    /// Gets or sets the unique identifier of the company.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    public required string CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the Stripe customer ID associated with the company.
    /// </summary>
    public string? StripeCustomerId { get; set; }

    /// <summary>
    /// Gets or sets the license associated with the company.
    /// </summary>
    public License? License { get; set; }
}