import {Component, inject, OnInit} from '@angular/core';
import {loadStripe, Stripe} from "@stripe/stripe-js";
import {PaymentService} from "../services/payment.service";
import {StripeCheckoutRequestModel} from "../models/stripe/stripeCheckoutRequest.model";
import {StripeCustomerPortalRequestModel} from "../models/stripe/stripeCustomerPortalRequest.model";
import {CompanyService} from "../services/company.service";
import {CompanyModel} from "../models/company/company.model";
import {LicenseTypeService} from "../services/licenseType.service";
import {LicenseTypeModel} from "../models/licenseType/licenseType.model";
import {LicenseModel} from "../models/license/license.model";
import {LicenseService} from "../services/license.service";
import {StripeCheckoutResponseModel} from "../models/stripe/stripeCheckoutResponse.model";
import {StripeCustomerPortalResponseModel} from "../models/stripe/stripeCustomerPortalResponse.model";

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {

  // Properties to store data retrieved from services
  public company: CompanyModel | undefined;
  public companyLicense: LicenseModel | undefined;
  public licenseTypes: LicenseTypeModel[] = [];
  public readonly _licenseService: LicenseService = inject(LicenseService);
  // Promise for Stripe instance
  private stripePromise?: Promise<Stripe | null>;
  // Services injected using dependency injection
  private readonly _paymentService: PaymentService = inject(PaymentService);
  private readonly _companyService: CompanyService = inject(CompanyService);
  private readonly _licenseTypeService: LicenseTypeService = inject(LicenseTypeService);

  ngOnInit(): void {
    // Fetch company details on component initialization
    const companyId: string = 'C4D8BA8D-ACE3-442E-A84D-D72D3F4D6F29';
    this.getCompany(companyId);
  }

  // Method to fetch company details by ID
  getCompany(companyId: string): void {
    this._companyService.getCompany(companyId).subscribe({
      next: ((response: CompanyModel): void => {
        // If company details are retrieved successfully
        if (response) {
          this.company = response;
          // If the company has a Stripe customer ID, fetch its license; otherwise, fetch available license types
          if (!response.stripeCustomerId) {
            this.getLicenseTypes();
          } else {
            this.getCompanyLicense(response.id);
          }
        }
      })
    });
  }

  // Method to fetch available license types
  getLicenseTypes(): void {
    this._licenseTypeService.getLicenseTypes().subscribe({
      next: ((response: LicenseTypeModel[]): void => {
        if (response.length >= 1) {
          this.licenseTypes = response;
        }
      })
    });
  }

  // Method to fetch company license by company ID
  getCompanyLicense(companyId: string): void {
    this._licenseService.getCompanyLicense(companyId).subscribe({
      next: ((response: LicenseModel): void => {
        if (response) {
          this.companyLicense = response;
        }
      })
    });
  }

  // Method to initiate order creation using Stripe Checkout
  onCreateOrder(licenseType: LicenseTypeModel): void {
    // Prepare data for creating the order
    const checkoutRequest: StripeCheckoutRequestModel = {
      stripePriceId: licenseType.stripePriceId,
      companyId: this.company?.id!,
      licenseTypeId: licenseType.id
    };

    // Call payment service to get the Stripe session ID
    this._paymentService.getSessionId(checkoutRequest).subscribe({
      next: ((response: StripeCheckoutResponseModel): void => {
        // Redirect to Stripe Checkout with the obtained session ID
        this.redirectToCheckout(response.sessionId, response.stripePublicKey).then();
      })
    });

  }

  // Method to redirect to Stripe Checkout
  async redirectToCheckout(sessionId: string, stripePublicKey: string): Promise<void> {
    // Load Stripe instance
    this.stripePromise = loadStripe(stripePublicKey);
    const stripe: Stripe | null = await this.stripePromise;
    // Redirect to Stripe Checkout using the obtained session ID
    stripe?.redirectToCheckout({sessionId: sessionId}).then(error => {
      // Handle any errors during redirection
      console.log(error.error.message);
    });
  }

  // Method to redirect to Stripe Customer Portal
  goToPortal(): void {
    // Prepare data for redirecting to Customer Portal
    const customerPortalRequest: StripeCustomerPortalRequestModel = {
      stripeCustomerId: this.company?.stripeCustomerId!
    };

    // Call payment service to get the Customer Portal URL
    this._paymentService.redirectToCustomerPortal(customerPortalRequest).subscribe({
      next: ((response: StripeCustomerPortalResponseModel): void => {
        // Redirect the user to the Customer Portal URL
        window.location.href = response.customerPortalUrl;
      })
    });
  }
}
