import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {LicenseModel} from "../models/license/license.model";

@Injectable({
  providedIn: 'root'
})
export class LicenseService {

  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _serviceUrl: string = 'https://localhost:7054/api/licenses/';

  getCompanyLicense(companyId: string): Observable<LicenseModel> {
    return this._httpClient.get<LicenseModel>(this._serviceUrl + companyId);
  }
}
