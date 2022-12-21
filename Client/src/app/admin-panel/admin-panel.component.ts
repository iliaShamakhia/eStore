import { Component, OnInit } from '@angular/core';
import { UserWithRole } from '../models/userWithRole';
import { AuthenticationService } from '../services/authentication.service';
import { RoleService } from '../services/role.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  users : UserWithRole[] = [];

  constructor(private roleService : RoleService, private authService : AuthenticationService) { }

  ngOnInit(): void {
    this.getRoles();
  }

  getRoles(){
    this.roleService.getRoles()
    .subscribe({
      next : (res) => this.users = res
    });
  }

  getUsername(){
    return this.authService.getUsername();
  }

  changeRole(username : string, role : string){
    this.roleService.changeRole(username, role)
    .subscribe({
      next : () => this.getRoles()
    })
  }

}
