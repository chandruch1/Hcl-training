import { Component, Input , OnChanges, SimpleChanges, inject,Output, EventEmitter } from '@angular/core';
import { Employee } from '../../../models/employee';
import { ReactiveFormsModule, Validators,FormControl, FormGroup } from '@angular/forms';
import { EmployeeService } from '../../../core/services/EmployeeService';
import { UpdateEmployee } from '../../../models/update-employee';

@Component({
  selector: 'app-employee-edit',
  imports: [ReactiveFormsModule],
  templateUrl: './employee-edit.html',
  styleUrl: './employee-edit.css',
})
export class EmployeeEdit implements OnChanges {

  private EmployeeService = inject(EmployeeService);
  @Input() employee! : Employee;
  @Output() closeForm = new EventEmitter<void>();
  

  editForm = new FormGroup({
  fullName: new FormControl('', Validators.required),
  email: new FormControl('', [Validators.required, Validators.email]),
  phone: new FormControl('', Validators.required),
  dateOfJoining: new FormControl('', Validators.required),
  salary: new FormControl(0, Validators.required),
  departmentId: new FormControl(0, Validators.required),
  role: new FormControl(0, Validators.required),
  isActive: new FormControl(true)
});

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['employee']&& this.employee){
      this.editForm.patchValue({
        fullName : this.employee.fullName,
        email : this.employee.email,
        phone : this.employee.phone,
        dateOfJoining : this.employee.dateOfJoining,
        salary : this.employee.salary,
        departmentId : this.employee.departmentId,
        role : Number(this.employee.role),
        isActive : this.employee.isActive

      });
    }
  }

  updateEmployee():void{
    if(this.editForm.invalid){
      this.editForm.markAllAsTouched();
      true;
    }

    const updateEmployee : UpdateEmployee = {
        FullName: this.editForm.value.fullName!,
        Email: this.editForm.value.email!,
        Phone: this.editForm.value.phone!,
        DateOfJoining: this.editForm.value.dateOfJoining!,
        Salary: this.editForm.value.salary!,
        DepartmentId: this.editForm.value.departmentId!,
        Role: this.editForm.value.role!,
        IsActive: this.editForm.value.isActive!
    
    }

    this.EmployeeService.updateEmployee(this.employee.employeeId,updateEmployee).subscribe({
      next : ()=>{
        alert(`Employee updated Succesfully`);
      },
      error : (error)=>{
        console.log(`error: ${error}`);
        alert('Failed to update employee');
      }
    });
  }
}
