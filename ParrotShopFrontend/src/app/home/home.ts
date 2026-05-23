import { Component, inject } from '@angular/core';
import { Navbar } from '../navbar/navbar';
import { Searchbar } from '../searchbar/searchbar';
import { AuthService } from '../services/auth-service';
import { ActivatedRoute, RouterLink } from "@angular/router";
import { Environment } from '../global/env';

@Component({
  selector: 'app-home',
  imports: [Searchbar, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  auth = inject(AuthService);
  env = inject(Environment);
  ar = inject(ActivatedRoute);


  ngOnInit() {
    this.ar.fragment.subscribe(f => {
      const element = document.querySelector("#" + f)
      if (element) element.scrollIntoView()
    })
  }
}
