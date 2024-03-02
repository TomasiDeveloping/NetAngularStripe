using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

/// <summary>
/// Represents a license entity.
/// </summary>
public class License
{
    /// <summary>
    /// Gets or sets the unique identifier of the license.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the license type.
    /// </summary>
    public Guid LicenceTypeId { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the license.
    /// </summary>
    public DateTime ExpiredAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated company.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the associated company.
    /// </summary>
    public Company? Company { get; set; }

    /// <summary>
    /// Gets or sets the license type associated with the license.
    /// </summary>
    public LicenseType? LicenseType { get; set; }

}