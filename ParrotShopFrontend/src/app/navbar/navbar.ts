import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { Searchbar } from '../searchbar/searchbar';
import { AuthService } from '../services/auth-service';
import { CheckoutService } from '../services/checkout-service';
import { ɵInternalFormsSharedModule } from "@angular/forms";

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, Searchbar, ɵInternalFormsSharedModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  auth = inject(AuthService);
  isMenuOpen = signal(false);
  router = inject(Router);
  cart = inject(CheckoutService);

  toggleMenu() {
    this.isMenuOpen.update(open => !open);
  }

  closeMenu() {
    this.isMenuOpen.set(false);
  }
}
