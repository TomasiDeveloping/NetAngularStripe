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

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit{

  public company: CompanyModel | undefined;
  public companyLicense: LicenseModel | undefined;
  public licenseTypes: LicenseTypeModel[] = [];

  private stripePromise?: Promise<Stripe | null>;

  private readonly _paymentService: PaymentService = inject(PaymentService);
  private readonly _companyService: CompanyService = inject(CompanyService);
  private readonly _licenseTypeService: LicenseTypeService = inject(LicenseTypeService);
  public readonly _licenseService: LicenseService = inject(LicenseService);
  ngOnInit(): void {
    const companyId: string = 'C4D8BA8D-ACE3-442E-A84D-D72D3F4D6F29';
    this.getCompany(companyId);
  }

  getCompany(companyId: string): void {
    this._companyService.getCompany(companyId).subscribe({
      next: ((response: CompanyModel): void => {
        if (response) {
          this.company = response;
          if (!response.stripeCustomerId) {
            this.getLicenseTypes();
          } else {
            this.getCompanyLicense(response.id);
          }
        }
      })
    });
  }

  getLicenseTypes(): void {
    this._licenseTypeService.getLicenseTypes().subscribe({
      next: ((response) => {
        if (response.length >=1) {
          this.licenseTypes = response;
        }
      })
    });
  }

  getCompanyLicense(companyId: string): void {
    this._licenseService.getCompanyLicense(companyId).subscribe({
      next: ((response) => {
        if (response) {
          this.companyLicense = response;
        }
      })
    });
  }


  onCreateOrder(licenseType: LicenseTypeModel) {
    const checkoutRequest: StripeCheckoutRequestModel = {
      stripePriceId: licenseType.stripePriceId,
      companyId: this.company?.id!,
      licenseTypeId: licenseType.id
    };
    this._paymentService.getSessionId(checkoutRequest).subscribe({
      next: ((response) => {
        this.redirectToCheckout(response.sessionId, response.stripePublicKey).then();
      })
    });

  }

  async redirectToCheckout(sessionId: string, stripePublicKey: string) {
    this.stripePromise = loadStripe(stripePublicKey);
    const stripe: Stripe | null = await this.stripePromise;
    stripe?.redirectToCheckout({sessionId: sessionId}).then(x => {
      console.log(x.error.message);
    });
  }


  goToPortal() {
    const customerPortalRequest: StripeCustomerPortalRequestModel = {
      stripeCustomerId: this.company?.stripeCustomerId!
    };
    this._paymentService.redirectToCustomerPortal(customerPortalRequest).subscribe({
      next: ((response) => {
        window.location.href = response.customerPortalUrl;
      })
    });
  }
}
