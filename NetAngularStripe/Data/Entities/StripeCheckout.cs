using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

public class StripeCheckout
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid LicenceTypeId { get; set; }

    public Guid CompanyId { get; set; }

    public required string StripeSessionId { get; set; }

    public required string Status { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class StripeStatus
{
    public const string Created = "Created";

    public const string Canceled = "Canceled";

    public const string Success = "Success";
}