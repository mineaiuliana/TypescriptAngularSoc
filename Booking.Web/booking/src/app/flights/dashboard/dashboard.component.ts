import { Component, OnInit, OnDestroy, createPlatformFactory } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { FlightsService, Flight } from '../shared';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
  flights: Flight[];
  deleteModalIsVisible: boolean;
  flight: Flight = new Flight();

  private getFlightsSubscription: Subscription;
  private deleteSubscription: Subscription;

  constructor(private readonly service: FlightsService, private readonly router: Router) { }

  ngOnInit() {
    this.getFlights();
  }

  ngOnDestroy(): void {
    if (this.getFlightsSubscription) {
      this.getFlightsSubscription.unsubscribe();
    }
    if (this.deleteSubscription) {
      this.deleteSubscription.unsubscribe();
    }
  }

  onRowClick(flight: Flight) {
    this.router.navigate(['/flights', flight.id]);
  }

  onAddFlight() {
    this.router.navigate(['/flights/new']);
  }

  onDeleteClick(event: Event, flight: Flight) {
    event.stopPropagation();
    this.flight = flight;
    this.deleteModalIsVisible = true;
  }

  onDeleteModalClose(deleteOption: boolean) {
    this.deleteModalIsVisible = false;
    if (deleteOption) {
      this.deleteSubscription = this.service.delete(this.flight.id).subscribe(() => {
        this.getFlights();
      });
    }
  }

  private getFlights() {
    this.getFlightsSubscription = this.service.getFlights().subscribe((data: Flight[]) => {
      this.flights = data;
    });
  }
}
