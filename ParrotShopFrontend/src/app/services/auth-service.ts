import { computed, inject, Injectable, signal } from '@angular/core';
import { Environment } from '../global/env';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { firstValueFrom, Observable } from 'rxjs';
import { CheckoutService } from './checkout-service';
import { JsonPatch } from './parrot-service';

export interface UserStatus{
  username:string;
  name: string;
  email: string;
  phone: string;
  shippingaddress: string;
  role: string;
  id:number;
  userColour: string;
}

export interface Login{

  username: string;
  password: string;
}

export interface Register{
  name: string;
  username: string;
  phone: string;
  shippingaddress: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface RegisterDTO{
  name: string;
  username: string;
  phone: string;
  shippingaddress: string;
  email: string;
  password: string;
}

export interface RegistrationErrors{
  email: string[];
  password: string[];
  username: string[];
  name: string[];
  phone: string[];
  shippingaddress: string[];
  ConfirmPassword: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  env = inject(Environment);
  isLoggedIn = signal(false);
  state = signal<UserStatus>({} as UserStatus);
  message = signal("");

  regSuccess = signal(false);

  regErrors = signal({} as RegistrationErrors);

  serverUnreachable = signal(false);

  delConfirmation = signal(false);
  delUltimateConf = signal("");

  checkout = inject(CheckoutService);


  accIconColours = [
    'green',
    'darkgreen',
    'maroon',
    'navy',
    'grey',
    'midnightblue',
    'peru',
    'teal',
    '#81007f',
    '#007466'
  ];



  constructor(private http: HttpClient, private ar : Router){ }
  

  isAdmin = computed(() => this.state()?.role?.toLowerCase() == 'admin');

  login(form: Login){
    this.http.post(this.env.APIURI+"/auth/login", form, {withCredentials: true}).subscribe(
      {
        next: ()=>{
          this.probe();
          this.message.set("");
          this.ar.navigate(['/']);
          this.checkout.getCart().subscribe({
            next: (data)=>{
              this.checkout.cart.set(data);
            }
          });
        },
        error: ()=>{
          this.message.set("Check your username or password");
        }
      }
    );
    
  }

  signup(form: Register){
    if (form.password != form.confirmPassword){
      this.regErrors.set({ConfirmPassword:"Passwords do not match!"} as RegistrationErrors);
      return;
    }
    this.http.post(this.env.APIURI+"/auth/register", form).subscribe(
      {
        next: ()=>{
          this.message.set("");
          this.regSuccess.set(true);
          this.ar.navigate(['/login']);
        },
        error: (payload)=>{
          console.log(payload);
          if (payload.status==400){
            this.regErrors.set(
            { username:payload.error.errors['username'],
              name:payload.error.errors['name'],
              phone:payload.error.errors['phone'],
              shippingaddress:payload.error.errors['shippingaddress'],
              email:payload.error.errors['email'],
              password:payload.error.errors['Password']

            } as RegistrationErrors);
          }

          if (payload.status==409){
            this.regErrors.set({username:["This username already exists!"]} as RegistrationErrors);
          }
        }
      }
    );
  }

  logout(){
    this.http.get(this.env.APIURI+"/auth/logout", {withCredentials: true}).subscribe();
    this.isLoggedIn.set(false);
    this.state.set({} as UserStatus);
    this.ar.navigate(['']);
    this.checkout.onLogout();

  }

  probe(){
    this.http.get<UserStatus>(this.env.APIURI+"/auth/whoami", {withCredentials: true}).subscribe({
      next: (data)=>{
        this.state.set(data);
        let sum = 0;
        for (let i = 0; i < data.username.length; i++){
          sum += data.username.charCodeAt(i);
        }
        this.state.update((s)=>({...s, userColour: this.accIconColours[sum % this.accIconColours.length]}));
        console.log(this.state().userColour);
        this.isLoggedIn.set(true);
      },
      error: (err)=>{
        if (err.status==0){
          this.serverUnreachable.set(true);
          return;
        }
        this.http.post(this.env.APIURI+"/auth/refresh", {}, {withCredentials: true}).subscribe({
          next: ()=>{
            this.probe();
          },
          error: ()=>{ 
            this.state.set({} as UserStatus);
            this.isLoggedIn.set(false);
          }
        });
      }
    });
  }    

  public async verifyAuthStatus(): Promise<boolean> {
    if (this.isLoggedIn()) return true;

    try {
      await firstValueFrom(this.http.get(`${this.env.APIURI}/auth/whoami`, { withCredentials: true }));
      this.isLoggedIn.set(true);
      return true;
    } catch {
      this.isLoggedIn.set(false);
      return false;
    }
  }

  public async Refresh():Promise<boolean>{
    try{
      await firstValueFrom(this.http.post(this.env.APIURI+"/auth/refresh", {}, {withCredentials: true}));
      return true;
    }catch{
      return false;
    }
  }

  onDelInput(s: string){
    this.delUltimateConf.set(s);
  }

  public delete(){
    if(this.delUltimateConf() != "Yes, I confirm"){
      return;
    }
    this.http.delete(this.env.APIURI+"/user", {withCredentials: true}).subscribe({
      next: ()=>{
        this.logout();
        this.delConfirmation.set(false);
        this.delUltimateConf.set("");
      }
    });

  }

  public patchUser(patch_b:UserStatus):Observable<any>{
    let patch: JsonPatch[] = [];
    let patchFields = Object.keys(patch_b);
    let patchValues = Object.values(patch_b);
    for (let i = 0; i < patchFields.length; i++){
      if (patchValues[i]==null||patchValues[i]==""||patchFields[i]=="id") continue;
      patch.push({
        op: "replace",
        path: "/"+patchFields[i],
        value: patchValues[i]
      });
    }
    return this.http.patch(this.env.APIURI+"/user", patch, {withCredentials: true, params:{
      id: this.state().id.toString()
    }, headers:{
      'Content-Type': 'application/json-patch+json'
    }});
  }
}
