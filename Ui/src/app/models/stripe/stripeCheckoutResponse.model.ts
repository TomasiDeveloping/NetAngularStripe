export interface StripeCheckoutResponseModel {
  // The ID of the Stripe Checkout session
  sessionId: string;

  // The public key associated with the Stripe account
  stripePublicKey: string;
}
