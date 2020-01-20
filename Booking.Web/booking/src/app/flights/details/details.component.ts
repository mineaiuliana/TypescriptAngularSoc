import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

import { Flight, FlightsService } from '../shared';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit, OnDestroy {
  form: FormGroup;

  private routeSubscription: Subscription;
  private saveSubscription: Subscription;
  private getSubscription: Subscription;

  constructor(private readonly activatedRoute: ActivatedRoute,
              private readonly service: FlightsService) {
  }

  ngOnInit() {
    this.createForm(new Flight());
    this.routeSubscription = this.activatedRoute.params.subscribe(p => {
      const id = +p.id;
      if (id) {
        this.getSubscription = this.service.getById(id).subscribe((flight: Flight) => {
          this.form.patchValue(flight);
        });
      }
    });

  }

  ngOnDestroy(): void {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
    if (this.saveSubscription) {
      this.saveSubscription.unsubscribe();
    }
    if (this.getSubscription) {
      this.getSubscription.unsubscribe();
    }
  }

  onSubmit(): void {
    this.saveSubscription = this.service.save(this.form.value).subscribe(() => {
      alert('Save finished successfully');
    });
  }

  private createForm(flight: Flight): void {
    this.form = new FormGroup({
      id: new FormControl(flight.id),
      to: new FormControl(flight.to, Validators.required),
      from: new FormControl(flight.from, Validators.required),
      price: new FormControl(flight.price, [Validators.required, Validators.min(9.99)]),
      date: new FormControl(flight.date, Validators.required)
    });
  }
}
