using NetAngularStripe.Data.Entities;
using Subscription = Stripe.Subscription;

namespace NetAngularStripe.Interfaces;

/// <summary>
///     Interface for accessing Stripe checkout-related data.
/// </summary>
public interface IStripeCheckoutRepository
{
    /// <summary>
    ///     Retrieves all Stripe checkouts asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of Stripe checkout
    ///     entities.
    /// </returns>
    Task<List<StripeCheckout>> GetStripeCheckoutAsync();

    /// <summary>
    ///     Adds a new Stripe checkout asynchronously.
    /// </summary>
    /// <param name="stripeCheckout">The Stripe checkout to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(StripeCheckout stripeCheckout);

    /// <summary>
    ///     Handles the successful completion of a Stripe checkout session asynchronously.
    /// </summary>
    /// <param name="sessionId">The Stripe session ID.</param>
    /// <param name="stripeCustomerId">The Stripe customer ID.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleSuccessAsync(string sessionId, string stripeCustomerId);

    /// <summary>
    ///     Handles the update of a subscription asynchronously.
    /// </summary>
    /// <param name="subscription">The Stripe subscription.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleUpdateSubscription(Subscription subscription);

    /// <summary>
    ///     Handles the cancellation of a Stripe checkout session asynchronously.
    /// </summary>
    /// <param name="stripeSessionId">The Stripe session ID.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleCancel(string stripeSessionId);
}