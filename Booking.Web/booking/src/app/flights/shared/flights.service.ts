import { Injectable } from '@angular/core';
import{HttpClient, HttpHeaders} from '@angular/common/http';
import { Flight } from './flight.model';
import { Observable } from 'rxjs';
import{environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FlightsService {

  constructor(private httpClient: HttpClient) {

   }

   get(): Observable<Flight[]> {
     let url= `${environment.serviceUrl}/flights`;

     return this.httpClient.get<Flight[]>(url,{headers: this.buildHeaders() });
   }

   getById(id: number): Observable<Flight> {
    let url= `${environment.serviceUrl}/flights/${id}`;

    return this.httpClient.get<Flight>(url,{headers: this.buildHeaders() });
  }

   post(data: Flight):Observable<any> {
     let url= `${environment.serviceUrl}/flights`;
     return this.httpClient.post(url,data, {headers:this.buildHeaders()});
   }

   put(data:Flight):Observable<any> {
    let url= `${environment.serviceUrl}/flights/${data.id}`;
    return this.httpClient.put(url,data, {headers:this.buildHeaders()});
   }

   delete(id: number): Observable<any>{
    let url= `${environment.serviceUrl}/flights/${id}`;
    return this.httpClient.delete(url, {headers:this.buildHeaders()});
   }

   private buildHeaders():HttpHeaders{
     return new HttpHeaders({'Content-Type': 'application/json'});
   }
}
