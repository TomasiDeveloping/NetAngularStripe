import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {LicenseModel} from "../models/license/license.model";

@Injectable({
  providedIn: 'root'
})
export class LicenseService {

  // Inject HttpClient dependency
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Service URL for licenses API endpoint
  private readonly _serviceUrl: string = 'https://localhost:7054/api/licenses/';

  // Method to get the license for a specific company by its ID
  getCompanyLicense(companyId: string): Observable<LicenseModel> {
    // Send GET request to license endpoint with specific company ID and return Observable of LicenseModel
    return this._httpClient.get<LicenseModel>(this._serviceUrl + companyId);
  }
}
