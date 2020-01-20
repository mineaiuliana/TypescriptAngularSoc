import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { BaseService } from '../../core/services/base.service';

import { Flight } from './models/flight.model';


@Injectable({
  providedIn: 'root'
})
export class FlightsService {
  private url = 'flights';

  constructor(private readonly baseService: BaseService) {

  }

  getFlights(): Observable<Flight[]> {
    return this.baseService.get<Flight[]>(this.url);
  }

  getById(id: number): Observable<Flight> {
    return this.baseService.get<Flight>(`${this.url}/${id}`);
  }

  save(flight: Flight): Observable<any> {
    if (flight.id) {
      return this.edit(flight);
    }
    return this.create(flight);
  }

  delete(flightId: number): Observable<any> {
    return this.baseService.delete(`${this.url}/${flightId}`);
  }

  private create(flight: Flight): Observable<Flight> {
    return this.baseService.post(this.url, flight);
  }

  private edit(flight: Flight): Observable<any> {
    return this.baseService.put(`${this.url}/${flight.id}`, flight);
  }
}
