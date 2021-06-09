import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { Flight, FlightsService } from '../shared';

@Component({
  selector: 'app-entry',
  templateUrl: './entry.component.html',
  styleUrls: ['./entry.component.scss']
})
export class EntryComponent implements OnInit, OnDestroy {
  flightForm: FormGroup;

  private updateSubscription: Subscription;

  constructor(private service: FlightsService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.createForm();
    this.loadFlight();
  }

  ngOnDestroy(): void {
     if(this.updateSubscription){
       this.updateSubscription.unsubscribe();
     }
  }

  onSubmit() {
    let flight: Flight = this.flightForm.value;
    if (flight.id) {
     this.updateSubscription = this.service.put(flight).subscribe(() => {
        alert('update finished');
      });
    }
    else {
      this.service.post(flight).subscribe(() => {
        alert('saved finished');
      });
    }
  }

  private createForm() {
    this.flightForm = new FormGroup({
      id: new FormControl(''),
      to: new FormControl('', Validators.required),
      from: new FormControl('', Validators.required),
      price: new FormControl('', [Validators.required, Validators.min(9.99)]),
      date: new FormControl(new Date(), Validators.required)
    });
  }

  private loadFlight(){
    this.activatedRoute.params.subscribe(p => {
      if (p.id) {
        let id = +p.id;
        this.service.getById(id).subscribe((flight: Flight) => {
          this.flightForm.patchValue(flight);
        });
      }
    })
  }
}
