import { Component, inject } from '@angular/core';
import { AuthService } from '../services/auth-service';

@Component({
  selector: 'app-acc-del-card',
  imports: [],
  templateUrl: './acc-del-card.html',
  styleUrl: './acc-del-card.css',
})
export class AccDelCard {
  auth = inject(AuthService);
}
