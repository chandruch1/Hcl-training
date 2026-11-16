import { Component, input, Input } from '@angular/core';
import { Employee } from '../../../models/employee';

@Component({
  selector: 'app-employee-edit',
  imports: [],
  templateUrl: './employee-edit.html',
  styleUrl: './employee-edit.css',
})
export class EmployeeEdit {
  @Input() employee! : Employee
}
