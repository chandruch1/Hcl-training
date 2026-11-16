import { Component,input } from '@angular/core';
import { Employee } from '../../../models/employee';

@Component({
  selector: 'app-employee-card',
  imports: [],
  templateUrl: './employee-card.html',
  styleUrl: './employee-card.css',
})
export class EmployeeCard {
  employee = input.required<Employee>();
}
