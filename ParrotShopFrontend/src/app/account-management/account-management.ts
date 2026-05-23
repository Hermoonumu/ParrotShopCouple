import { Component, inject, signal } from '@angular/core';
import { AuthService, UserStatus } from '../services/auth-service';
import { FormBuilder, FormsModule, ReactiveFormsModule, ɵInternalFormsSharedModule } from '@angular/forms';
import { User } from '../app';

@Component({
  selector: 'app-account-management',
  imports: [ReactiveFormsModule],
  templateUrl: './account-management.html',
  styleUrl: './account-management.css',
})
export class AccountManagement {
  auth = inject(AuthService);
  
  userForm:any;
  user = signal<UserStatus>({} as UserStatus);
  errmsg = signal("");
  successmessage=signal("");


  constructor(private fb:FormBuilder){
    this.userForm=fb.group({
      username:[''],
      name:[''],
      email:[''],
      phone:[''],
      shippingaddress:['']
    });
    this.userForm.valueChanges.subscribe((val:UserStatus)=>{
      this.user.set(val);
    });
  }

  ngOnInit(){
    this.user.set(this.auth.state());
    this.userForm.patchValue(this.user());
    this.errmsg.set('')
  }

  deleteAccount(){
    this.auth.delConfirmation.set(true);
  }
  logout(){
    this.auth.logout();
  }

  saveChanges(){
    console.log(this.user())
    this.auth.patchUser(this.user()).subscribe({
      error:(data)=>{
        this.errmsg.set(data.error.message);
        this.successmessage.set('');
      },
      next:()=>{
        this.errmsg.set('');
        this.successmessage.set('Changes saved!');
      }
    });
  }


}
