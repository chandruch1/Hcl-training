import { Component, inject,output } from '@angular/core';
import { EmployeeService } from '../../../core/services/EmployeeService';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators
} from '@angular/forms';


@Component({
  selector: 'app-employee-add',
  imports: [ReactiveFormsModule],
  templateUrl: './employee-add.html',
  styleUrl: './employee-add.css',
})
export class EmployeeAdd {

  closeForm = output<void>();

  cancelForm():void{
    this.closeForm.emit();
  }
  employeeForm = new FormGroup({

    fullName: new FormControl('', [
      Validators.required
    ]),

    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),

    phone: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9]{10}$/)
    ]),

    dateOfJoining: new FormControl('', [
      Validators.required
    ]),

    salary: new FormControl(0, [
      Validators.required,
      Validators.min(1)
    ]),

    departmentId: new FormControl(0, [
      Validators.required,
      Validators.min(1)
    ]),


    isActive: new FormControl(true)

  });

  private employeeservice = inject(EmployeeService);

  

  saveEmployee() : void{
    console.log(`SaveEmployee is  clicked`)
    if(this.employeeForm.invalid){
      this.employeeForm.markAllAsTouched();
      return;
    }
    const employee = this.employeeForm.getRawValue();

    this.employeeservice.createEmployee(employee).subscribe({
      next : (response)=>{
        console.log('Employee created successfully:', response);
      alert('Employee created successfully!');
      },
      error : (err)=>{
        console.error('Error creating employee:', err);
        alert('Failed to create employee');
      }
    })
  }

//   saveEmployee(): void {

//   console.log("1. Save button clicked");

//   console.log("2. Form status:", this.employeeForm.status);

//   console.log("3. Form values:", this.employeeForm.getRawValue());

//   if (this.employeeForm.invalid) {

//     console.log("4. Form is INVALID");

//     Object.keys(this.employeeForm.controls).forEach(key => {
//       const control = this.employeeForm.get(key);

//       if (control?.invalid) {
//         console.log("Invalid field:", key);
//         console.log("Validation errors:", control.errors);
//       }
//     });

//     this.employeeForm.markAllAsTouched();
//     return;
//   }

//   console.log("5. Form is VALID");

//   const employee = this.employeeForm.getRawValue();

//   console.log("6. Calling POST API");

//   this.employeeservice.createEmployee(employee).subscribe({

//     next: (response) => {
//       console.log("7. Employee created:", response);
//       alert("Employee created successfully!");
//     },

//     error: (err) => {
//       console.error("API Error:", err);
//       alert("Failed to create employee");
//     }

//   });

// }

  
}
