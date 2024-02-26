using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

public class LicenseType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public decimal Price { get; set; }
    public required string StripePriceId { get; set; }

    public ICollection<License> Licenses { get; set; } = [];
}