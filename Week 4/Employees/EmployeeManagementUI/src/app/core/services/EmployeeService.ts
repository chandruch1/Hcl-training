import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../config/api.config';
import { Employee } from '../../models/employee';
import { UpdateEmployee } from '../../models/update-employee';



@Injectable({
  providedIn: 'root',
})
export class EmployeeService  {
  private http = inject(HttpClient);
  
  private apiUrl = `${API_CONFIG.baseUrl}/Employees`;

  getEmployees() : Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  createEmployee(employee : any) : Observable<any>{
    return this.http.post(this.apiUrl,employee);
  }

  getEmployeeByName(name:string) : Observable<Employee[]>{
    return this.http.get<Employee[]>(`${this.apiUrl}/search?name=${name}`);
  }
  
  updateEmployee(id:number,employee:UpdateEmployee) : Observable<void>{
    return this.http.put<void>(`${this.apiUrl}/${id}`,employee);
  }
}
