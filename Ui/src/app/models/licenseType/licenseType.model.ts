export interface LicenseTypeModel {
  // Unique identifier for the license type
  id: string;

  // Name of the license type
  name: string;

  // Description of the license type
  description: string;

  // Price of the license type
  price: number;

  // ID of the associated Stripe Price
  stripePriceId: string;
}
