import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {environment} from '../../../environments/environment';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class BaseService {
  constructor(private http: HttpClient) { }

  get<T>(url: string): Observable<T> {
    const completeUrl: string = environment.apiUrl + url;

    return this.http.get<T>(completeUrl, { headers: this.buildHeaders() })
      .pipe(catchError(r => this.onError(r)));
  }

  post<T>(url: string, data: any): Observable<T> {
    const completeUrl: string = environment.apiUrl + url;

    return this.http.post<T>(completeUrl, data, { headers: this.buildHeaders() })
      .pipe(catchError(r => this.onError(r)));
  }

  put<T>(url: string, data: any): Observable<T> {
    const completeUrl: string = environment.apiUrl + url;

    return this.http.put<T>(completeUrl, data, { headers: this.buildHeaders() })
      .pipe(catchError(r => this.onError(r)));
  }

  delete<T>(url: string): Observable<T> {
    const completeUrl: string = environment.apiUrl + url;
    return this.http.delete<T>(completeUrl, { headers: this.buildHeaders() })
      .pipe(catchError(r => this.onError(r)));
  }

  private buildHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  private onError(errorResponse: Response) {
    console.error(errorResponse);
    const errorAsJson: any = errorResponse.json();
    return throwError(errorAsJson.error || 'Server error');
  }

}
