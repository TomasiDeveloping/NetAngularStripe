import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CheckoutResponseModel} from "../checkoutResponse.model";
import {environment} from "../../environments/environment";
import {StripeCheckoutRequestModel} from "../models/stripe/stripeCheckoutRequest.model";
import {StripeCheckoutResponseModel} from "../models/stripe/stripeCheckoutResponse.model";
import {StripeCustomerPortalRequestModel} from "../models/stripe/stripeCustomerPortalRequest.model";
import {StripeCustomerPortalResponseModel} from "../models/stripe/stripeCustomerPortalResponse.model";

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _serviceUrl: string = 'https://localhost:7054/api/payments'

  getSessionId(stripeCheckoutRequest: StripeCheckoutRequestModel): Observable<StripeCheckoutResponseModel> {
    return this._httpClient.post<StripeCheckoutResponseModel>(this._serviceUrl + '/checkout', stripeCheckoutRequest);
  }

  redirectToCustomerPortal(stripeCustomerPortalRequest: StripeCustomerPortalRequestModel): Observable<StripeCustomerPortalResponseModel> {
    return this._httpClient.post<StripeCustomerPortalResponseModel>(this._serviceUrl + '/customerPortal', stripeCustomerPortalRequest);

  }

}
