import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {LicenseTypeModel} from "../models/licenseType/licenseType.model";

@Injectable({
  providedIn: 'root'
})
export class LicenseTypeService {

  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _serviceUrl: string = 'https://localhost:7054/api/licenseTypes/';

  getLicenseTypes(): Observable<LicenseTypeModel[]> {
    return this._httpClient.get<LicenseTypeModel[]>(this._serviceUrl);
  }

  getLicenseType(licenseTypeId: string): Observable<LicenseTypeModel> {
    return this._httpClient.get<LicenseTypeModel>(this._serviceUrl + licenseTypeId);
  }
}
