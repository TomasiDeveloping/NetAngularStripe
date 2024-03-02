export interface CompanyModel {
  // Unique identifier for the company
  id: string;

  // Name of the company
  companyName: string;

  // Optional Stripe customer ID associated with the company
  stripeCustomerId?: string;
}
