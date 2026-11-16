import { Component, inject, OnInit } from '@angular/core';
import { EmployeeService } from '../../../core/services/EmployeeService';
import { Employee } from '../../../models/employee';
import { EmployeeAdd } from '../employee-add/employee-add';
import { EmployeeCard } from '../employee-card/employee-card';
import { Token } from '../../../core/services/token';
import { EmployeeEdit } from '../employee-edit/employee-edit';

@Component({
  selector: 'app-employee-list',
  imports: [EmployeeAdd,EmployeeCard,EmployeeEdit],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeList implements OnInit {

  private employeeService = inject(EmployeeService);
  private TokenService = inject(Token);

  employees: Employee[] = [];

  selectedEmployee : Employee | null= null;
  showEditEmployee = false;

  openEditEmployee(employee : Employee) : void{
    this.showEditEmployee= true;
    this.selectedEmployee=employee;
  }

  closeEditEmployee(): void{
    this.selectedEmployee=null;
    this.showEditEmployee=false;
  }

  showAddEmployee = false;

  userRole : string | null = this.TokenService.getUserRole();

  isAdmin(): boolean{
    console.log(`user role : ${this.userRole}`);
    return this.userRole==='Admin';
  }

  openAddEmployee():void {
    this.showAddEmployee=true;
  }
  closeAddEmployee() : void{
    this.showAddEmployee=false;
  }
  searchEmployees(name:string):void{
  if(!name.trim()){
    this.loadEmployees();
    return;
  }
  this.employeeService.getEmployeeByName(name).subscribe({
    next : (Response)=>{
      this.employees=Response;
    },
    error: (error) => {
      console.error('Search failed:', error);
    }
  });
  }

  loadEmployees():void{
    this.employeeService.getEmployees().subscribe({
      next : (response)=>{
        this.employees=response;
        console.log( `Load employee succesfully`);
      },
      error : (err)=>{
        console.error(`have problem in loading employees`);
      }
    })
  }

  ngOnInit(): void {
    this.loadEmployees();
  }
}