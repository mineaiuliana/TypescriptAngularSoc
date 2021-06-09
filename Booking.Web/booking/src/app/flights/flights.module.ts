import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FlightsRoutingModule } from './flights-rounting.module';
import { EntryComponent } from './entry/entry.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    DashboardComponent,
    EntryComponent
  ],
  imports: [
    CommonModule, FlightsRoutingModule, ReactiveFormsModule, SharedModule
  ]
})
export class FlightsModule { }
