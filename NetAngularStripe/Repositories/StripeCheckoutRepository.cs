using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Data.Entities;
using NetAngularStripe.Interfaces;
using Stripe;

namespace NetAngularStripe.Repositories;

/// <summary>
/// Repository implementation for managing Stripe checkout-related data.
/// </summary>
public class StripeCheckoutRepository(ApplicationContext context) : IStripeCheckoutRepository
{
    /// <summary>
    /// Retrieves all Stripe checkouts asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of Stripe checkout entities.</returns>
    public async Task<List<StripeCheckout>> GetStripeCheckoutAsync()
    {
        // Fetch all Stripe checkouts from the database asynchronously
        var stripeCheckouts = await context.StripeCheckouts
            .AsNoTracking()
            .ToListAsync();

        // Return the list of Stripe checkout entities
        return stripeCheckouts;
    }

    /// <summary>
    /// Adds a new Stripe checkout asynchronously.
    /// </summary>
    /// <param name="stripeCheckout">The Stripe checkout to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddAsync(StripeCheckout stripeCheckout)
    {
        // Add the new Stripe checkout to the database asynchronously
        await context.StripeCheckouts.AddAsync(stripeCheckout);

        // Save changes to the database
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Handles the successful completion of a Stripe checkout session asynchronously.
    /// </summary>
    /// <param name="sessionId">The Stripe session ID.</param>
    /// <param name="stripeCustomerId">The Stripe customer ID.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleSuccessAsync(string sessionId, string stripeCustomerId)
    {
        // Find the Stripe checkout associated with the given session ID
        var checkout = await context.StripeCheckouts
            .FirstOrDefaultAsync(co => co.StripeSessionId == sessionId);

        // If no checkout found, return
        if (checkout is null) return;

        // Find the company associated with the checkout
        var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == checkout.CompanyId);

        // If no company found, return
        if (company is null) return;

        // Update company's Stripe customer ID and create a new license for the company
        company.StripeCustomerId = stripeCustomerId;
        company.License = new License
        {
            CompanyId = company.Id,
            LicenceTypeId = checkout.LicenceTypeId,
            ExpiredAt = DateTime.Now.AddMonths(1)
        };

        // Update checkout status to success and save changes to the database
        checkout.Status = StripeStatus.Success;
        await context.SaveChangesAsync();

    }

    /// <summary>
    /// Handles the update of a subscription asynchronously.
    /// </summary>
    /// <param name="subscription">The Stripe subscription.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleUpdateSubscription(Subscription subscription)
    {
        // Find the company associated with the subscription's customer ID
        var company = await context.Companies.FirstOrDefaultAsync(c => c.StripeCustomerId == subscription.CustomerId);

        // If no company found, return
        if (company is null) return;

        // Find the license associated with the company
        var companyLicense = await context.Licenses
                .Include(l => l.LicenseType)
                .FirstOrDefaultAsync(ps => ps.CompanyId == company.Id);

        // If no license found, return
        if (companyLicense is null) return;

        // Get the subscription item
        var subscriptionItem = subscription.Items.Data.FirstOrDefault();
        // If no subscription item found, return
        if (subscriptionItem is null) return;

        // Get the Stripe price ID from the subscription item
        var stripePriceId = subscriptionItem.Price.Id;

        // Check if the subscription plan has changed
        if (companyLicense.LicenseType!.StripePriceId != stripePriceId)
        {
            // Customer has upgraded or downgraded subscription plan
            // Find the new license type based on the Stripe price ID
            var licenceType = await context.LicenseTypes.FirstOrDefaultAsync(lt => lt.StripePriceId == stripePriceId);

            // If no new license type found, return
            if (licenceType is null) return;

            // Update company's license type and save changes to the database
            companyLicense.LicenceTypeId = licenceType.Id;
            await context.SaveChangesAsync();
        }
        else
        {
            // Customer plan update, update the license expiration date to the end of the current subscription period
            var startDate = subscription.CurrentPeriodEnd;
            companyLicense.ExpiredAt = startDate;
            await context.SaveChangesAsync();
        }


    }

    /// <summary>
    /// Handles the cancellation of a Stripe checkout session asynchronously.
    /// </summary>
    /// <param name="stripeSessionId">The Stripe session ID.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleCancel(string stripeSessionId)
    {
        // Find the Stripe checkout associated with the given session ID
        var checkout = await context.StripeCheckouts
            .FirstOrDefaultAsync(co => co.StripeSessionId == stripeSessionId);

        // If no checkout found, return
        if (checkout is null) return;

        // Update checkout status to canceled and save changes to the database
        checkout.Status = StripeStatus.Canceled;
        await context.SaveChangesAsync();
    }
}