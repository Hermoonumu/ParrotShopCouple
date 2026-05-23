import { Component, inject, signal } from '@angular/core';
import { Parrot, ParrotService } from '../services/parrot-service';
import { Environment } from '../global/env';
import { ItemCard } from '../item-card/item-card';

@Component({
  selector: 'app-quiz-res-display',
  imports: [ItemCard],
  templateUrl: './quiz-res-display.html',
  styleUrl: './quiz-res-display.css',
})
export class QuizResDisplay {
  parrotSvc = inject(ParrotService);
  env = inject(Environment);
  parrots = signal<Parrot[]>([]);

  ngOnInit(){
    this.parrotSvc.recommendation(this.env.parrotAnswers()).subscribe({
      next: (data)=>{
        this.parrots.set(data);
      }
    });
  }

}
