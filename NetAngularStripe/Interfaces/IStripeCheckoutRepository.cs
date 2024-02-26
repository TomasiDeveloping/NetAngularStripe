using NetAngularStripe.Data.Entities;
using Subscription = Stripe.Subscription;

namespace NetAngularStripe.Interfaces;

public interface IStripeCheckoutRepository
{
    Task<List<StripeCheckout>> GetStripeCheckoutAsync();
    Task AddAsync(StripeCheckout stripeCheckout);

    Task HandleSuccessAsync(string sessionId, string stripeCustomerId);

    Task HandleUpdateSubscription(Subscription subscription);

    Task HandleCancel(string stripeSessionId);
}