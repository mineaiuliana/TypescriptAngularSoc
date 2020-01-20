import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { DashboardComponent } from './dashboard/dashboard.component';
import { DetailsComponent } from './details/details.component';
import { FlightsRoutingModule } from './flights-routing.module';
import { ConfirmationModalComponent } from './confirmation-modal/confirmation-modal.component';



@NgModule({
  declarations: [DashboardComponent, DetailsComponent, ConfirmationModalComponent],
  imports: [CommonModule,
     FlightsRoutingModule, ReactiveFormsModule],
})
export class FlightsModule { }
