import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserRegisterModel } from '../models/userRegisterModel';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {

  err : any | undefined;

  constructor(private authService : AuthenticationService, private router: Router) { }

  ngOnInit(): void {
  }

  registerUser(user : any){
    let newUser : UserRegisterModel = {
      firstName : user.fName,
      lastName : user.lName,
      userName : user.uName,
      email : user.email,
      password : user.password,
      role : user.role
    }
    this.authService.register(newUser)
    .subscribe({
      next : (res) => {
        this.router.navigate(['/games'])
      },
      error : (err) => {console.log(err); this.err = err}
    });
  }

}
