using System.ComponentModel.DataAnnotations.Schema;

namespace NetAngularStripe.Data.Entities;

public class Company
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string CompanyName { get; set; }

    public string? StripeCustomerId { get; set; }

    public License? License { get; set; }
}