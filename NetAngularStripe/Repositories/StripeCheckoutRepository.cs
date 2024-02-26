using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Data.Entities;
using NetAngularStripe.Interfaces;
using Stripe;

namespace NetAngularStripe.Repositories;

public class StripeCheckoutRepository(ApplicationContext context) : IStripeCheckoutRepository
{
    public async Task<List<StripeCheckout>> GetStripeCheckoutAsync()
    {
        var stripeCheckouts = await context.StripeCheckouts
            .AsNoTracking()
            .ToListAsync();

        return stripeCheckouts;
    }

    public async Task AddAsync(StripeCheckout stripeCheckout)
    {
        await context.StripeCheckouts.AddAsync(stripeCheckout);
        await context.SaveChangesAsync();
    }

    public async Task HandleSuccessAsync(string sessionId, string stripeCustomerId)
    {
        var checkout = await context.StripeCheckouts
            .FirstOrDefaultAsync(co => co.StripeSessionId == sessionId);
        
        if (checkout is null) return;

        var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == checkout.CompanyId);

        if (company is null) return;

        company.StripeCustomerId = stripeCustomerId;
        company.License = new License
        {
            CompanyId = company.Id,
            LicenceTypeId = checkout.LicenceTypeId,
            ExpiredAt = DateTime.Now.AddMonths(1)
        };

        checkout.Status = StripeStatus.Success;
        await context.SaveChangesAsync();

    }

    public async Task HandleUpdateSubscription(Subscription subscription)
    {
        var company = await context.Companies.FirstOrDefaultAsync(c => c.StripeCustomerId == subscription.CustomerId);

        if (company is null) return;

        var companyLicense = await context.Licenses
                .Include(l => l.LicenseType)
                .FirstOrDefaultAsync(ps => ps.CompanyId == company.Id);

        if (companyLicense is null) return;

        var subscriptionItem = subscription.Items.Data.FirstOrDefault();
        if (subscriptionItem is null) return;

        var stripePriceId = subscriptionItem.Price.Id;

        if (companyLicense.LicenseType!.StripePriceId != stripePriceId)
        {
            // Customer has up or downgrade
            var licenceType = await context.LicenseTypes.FirstOrDefaultAsync(lt => lt.StripePriceId == stripePriceId);
            if (licenceType is null) return;
            companyLicense.LicenceTypeId = licenceType.Id;
            await context.SaveChangesAsync();
        }
        else
        {
            // Customer Plan Update
            var startDate = subscription.CurrentPeriodEnd;
            companyLicense.ExpiredAt = startDate;
            await context.SaveChangesAsync();
        }


    }

    public async Task HandleCancel(string stripeSessionId)
    {
        var checkout = await context.StripeCheckouts
            .FirstOrDefaultAsync(co => co.StripeSessionId == stripeSessionId);

        if (checkout is null) return;

        checkout.Status = StripeStatus.Canceled;
        await context.SaveChangesAsync();
    }
}