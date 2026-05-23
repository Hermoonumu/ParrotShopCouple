import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService, Login, Register } from '../services/auth-service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login-register.html',
  styleUrl: './login-register.css',
})
export class LoginRegister {
  auth = inject(AuthService);
  router = inject(Router);
  nativeMsg = signal("");

  isLoginMode = signal(true);

  login = signal<Login>({
    username: '',
    password: ''
});

  register = signal<Register>({
    name: '',
    username:'',
    phone: '',
    shippingaddress: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  formLogin:    any;
  formRegister: any;
  constructor(private fb : FormBuilder, private ar: ActivatedRoute){
    const segment = this.ar.snapshot.url[0].path;
    this.isLoginMode.set(segment === 'login');

    this.formLogin = fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
    this.formRegister = fb.group({
      name: [''],
      username: [''],
      phone: [''],
      shippingaddress: [''],
      email: [''],
      password: [''],
      confirmPassword: ['']
    });

    this.formLogin.valueChanges.subscribe((value:any) => {this.login.set(value)});
    this.formRegister.valueChanges.subscribe((value:any) => {this.register.set(value)});
  }

  toggleMode(state: boolean) {
    this.isLoginMode.set(state);
    this.formLogin.reset();
    this.formRegister.reset();
    this.router.navigate([this.isLoginMode() ? 'login' : 'register']);
    
  }

  submitLogin(){
    this.auth.message.set("");
    if (!this.formLogin.valid) {
      if (this.formLogin.get('username')?.invalid) {
        this.nativeMsg.set("Enter username")
      }
      if (this.formLogin.get('password')?.invalid) {
        this.nativeMsg.set("Password must be at least 8 characters long")
      }
      return;
    };
    this.auth.login(this.login());
    this.auth.regSuccess.set(false);
  }

  submitSignUp(){
    this.auth.signup(this.register());
  }

}
