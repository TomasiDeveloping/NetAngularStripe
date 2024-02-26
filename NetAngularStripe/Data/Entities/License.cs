using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

public class License
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid LicenceTypeId { get; set; }

    public DateTime ExpiredAt { get; set; }

    public Guid CompanyId { get; set; }

    public Company? Company { get; set; }

    public LicenseType? LicenseType { get; set; }

}