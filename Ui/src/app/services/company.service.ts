import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CompanyModel} from "../models/company/company.model";

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _serviceUrl: string = 'https://localhost:7054/api/companies/'

  getCompany(companyId: string): Observable<CompanyModel> {
    return this._httpClient.get<CompanyModel>(this._serviceUrl + companyId);
  }
}
