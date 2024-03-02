import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {LicenseTypeModel} from "../models/licenseType/licenseType.model";

@Injectable({
  providedIn: 'root'
})
export class LicenseTypeService {

  // Inject HttpClient dependency
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Service URL for license types API endpoint
  private readonly _serviceUrl: string = 'https://localhost:7054/api/licenseTypes/';

  // Method to get all license types
  getLicenseTypes(): Observable<LicenseTypeModel[]> {
    // Send GET request to license types endpoint and return Observable of LicenseTypeModel array
    return this._httpClient.get<LicenseTypeModel[]>(this._serviceUrl);
  }

  // Method to get a specific license type by ID
  getLicenseType(licenseTypeId: string): Observable<LicenseTypeModel> {
    // Send GET request to license type endpoint with specific ID and return Observable of LicenseTypeModel
    return this._httpClient.get<LicenseTypeModel>(this._serviceUrl + licenseTypeId);
  }
}
