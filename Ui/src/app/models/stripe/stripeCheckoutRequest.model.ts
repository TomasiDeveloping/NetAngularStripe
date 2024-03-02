export interface StripeCheckoutRequestModel {
  // The ID of the Stripe Price associated with the product or service
  stripePriceId: string;

  // The ID of the company making the purchase
  companyId: string;

  // The ID of the license type being purchased
  licenseTypeId: string;
}
