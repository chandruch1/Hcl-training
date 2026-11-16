import { Component, inject } from '@angular/core';
import { Inject } from '@angular/core';
import { LoginRequest } from '../../../models/login-request';
import { Auth } from '../../../core/services/auth';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Token } from '../../../core/services/token';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ ReactiveFormsModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private Authservice = inject(Auth);
  private router = inject(Router);
  private tokenService = inject(Token);

  loginForm  = new FormGroup({
    email: new FormControl('',[Validators.required,Validators.email]),
    password : new FormControl('',[Validators.required])
  })

  loginUser():void{

    console.log("login is clicked")
    console.log(this.loginForm.invalid)
    if(this.loginForm.invalid){
      this.loginForm.markAllAsTouched();
      return;
    }

    const data : LoginRequest = {
      email:this.loginForm.value.email!,
      Password : this.loginForm.value.password!
    }

    this.Authservice.login(data).subscribe({
      next : (Response)=>{
        console.log(`login Succesfully`,Response);
        this.tokenService.saveTokens(Response.accessToken,Response.refreshToken)
        alert('Login successful!');
        console.log(`login succesfull`)
        this.router.navigate(['employees']).then((success)=>{
          console.log(`Suceesfully navigated`,success)
        });
      },
      error : (err)=>{
        console.log(err.error?.message || 'Login failed');
      }
    })
  }
}
