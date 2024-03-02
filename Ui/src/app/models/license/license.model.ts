export interface LicenseModel {
  // Date when the license expires
  expiredAt: Date

  // Name of the subscription
  subscriptionName: string;

  // Price of the license
  price: number;

  // Description of the license
  description: string
}
