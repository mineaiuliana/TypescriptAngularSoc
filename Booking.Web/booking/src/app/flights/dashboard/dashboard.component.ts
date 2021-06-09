import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Flight } from '../shared/flight.model';
import { FlightsService } from '../shared/flights.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
   flights: Flight[];
  constructor(private service: FlightsService, private router:Router) { }

  ngOnInit(): void {
    this.getFlights();
  }

  onGoToDetails(flightId: number){
   this.router.navigate(['/flights', flightId]);
  }

  onDeleteClick(flightId: number){
    this.service.delete(flightId).subscribe(()=>{
      this.getFlights();
    })
  }

  private getFlights(){
    this.service.get().subscribe((data : Flight[]) => {
      this.flights = data;
    });
  }
}
