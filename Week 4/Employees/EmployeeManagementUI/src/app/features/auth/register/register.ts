import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { Auth } from '../../../core/services/auth';
import { RegisterRequest } from '../../../models/register-request';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  registerForm  = new FormGroup({
    email : new FormControl('',[Validators.required,Validators.email]),
    password : new FormControl('',Validators.required)
  })

  private authService = inject(Auth);

  registerUser():void{
    console.log(`Register Button is clicked`);

    if(this.registerForm.invalid){
      this.registerForm.markAllAsTouched();
      return;
    }
    
    const data: RegisterRequest = {
      email:this.registerForm.value.email!,
      password : this.registerForm.value.password!,
      role : 0 
    }

    this.authService.register(data).subscribe({
      next : (response)=>{
        console.log(`sucessfully data Recieved`,response);
        alert('Registration successful!');
      },
      error: (err) => {
      console.error('Registration failed:', err);
      alert(err.error?.message || 'Registration failed');
    }
    });
  }  
}
