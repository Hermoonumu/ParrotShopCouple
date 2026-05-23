import { Component, computed, input, signal } from '@angular/core';
import { Parrot } from '../services/parrot-service';

@Component({
  selector: 'app-parrot-trait-show',
  imports: [],
  templateUrl: './parrot-trait-show.html',
  styleUrl: './parrot-trait-show.css',
})
export class ParrotTraitShow {

  size: Record<string, number> ={
    "Small": 1,
    "Medium": 2,
    "Large": 3
  }

  level : Record<string, number> ={
    "Low": 1,
    "Mid": 2,
    "High": 3
  }

  kidsafety: Record<string, number> ={
    "Yes": 3,
    "No": 1,
    "Cautious": 2
  }


  parrotMid = input<Parrot>();
  parrot = computed<Parrot>(() => this.parrotMid() as Parrot);

}
