import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CompanyModel} from "../models/company/company.model";

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  // Inject HttpClient dependency
  private readonly _httpClient: HttpClient = inject(HttpClient);

  // Service URL for companies API endpoint
  private readonly _serviceUrl: string = 'https://localhost:7054/api/companies/'

  // Method to get company details by its ID
  getCompany(companyId: string): Observable<CompanyModel> {
    // Send GET request to company endpoint with specific company ID and return Observable of CompanyModel
    return this._httpClient.get<CompanyModel>(this._serviceUrl + companyId);
  }
}
