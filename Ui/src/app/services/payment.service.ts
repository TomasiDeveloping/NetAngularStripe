import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {StripeCheckoutRequestModel} from "../models/stripe/stripeCheckoutRequest.model";
import {StripeCheckoutResponseModel} from "../models/stripe/stripeCheckoutResponse.model";
import {StripeCustomerPortalRequestModel} from "../models/stripe/stripeCustomerPortalRequest.model";
import {StripeCustomerPortalResponseModel} from "../models/stripe/stripeCustomerPortalResponse.model";

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  // Inject HttpClient dependency
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Service URL for payment-related API endpoints
  private readonly _serviceUrl: string = 'https://localhost:7054/api/payments'

  // Method to get Stripe checkout session ID
  getSessionId(stripeCheckoutRequest: StripeCheckoutRequestModel): Observable<StripeCheckoutResponseModel> {
    // Send POST request to checkout endpoint and return Observable of StripeCheckoutResponseModel
    return this._httpClient.post<StripeCheckoutResponseModel>(this._serviceUrl + '/checkout', stripeCheckoutRequest);
  }

  // Method to redirect to Stripe customer portal
  redirectToCustomerPortal(stripeCustomerPortalRequest: StripeCustomerPortalRequestModel): Observable<StripeCustomerPortalResponseModel> {
    // Send POST request to customerPortal endpoint and return Observable of StripeCustomerPortalResponseModel
    return this._httpClient.post<StripeCustomerPortalResponseModel>(this._serviceUrl + '/customerPortal', stripeCustomerPortalRequest);

  }

}
