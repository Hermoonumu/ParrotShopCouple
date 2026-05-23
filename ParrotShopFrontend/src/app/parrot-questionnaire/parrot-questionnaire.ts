import { Component, inject, signal } from '@angular/core';
import { Color, ParrotFilter, ParrotService } from '../services/parrot-service';
import { Router } from '@angular/router';
import { Environment } from '../global/env';

@Component({
  selector: 'app-parrot-questionnaire',
  imports: [],
  templateUrl: './parrot-questionnaire.html',
  styleUrl: './parrot-questionnaire.css',
})
export class ParrotQuestionnaire {
  parrotService = inject(ParrotService);
  router = inject(Router);
  env = inject(Environment);
  currQuestion = signal(0);
  questions = [ "Parrots can be demanding pets. Have you ever owned one?",
                "Parrots can be noisy and exploratory. How spacious is your living space?",
                "Some parrots can get little bit aggressive. Do you have children of guests frequently?",
                "A number of parrots are like a velcro. How much time can you dedicate to it?",
                "Some parrots are a chatterboxes. What personality do you want?",
                "Select your favourite colour."
  ];
  selectedAnswer = signal<number|null>(null);
  answers = signal({} as ParrotFilter);

  progress=signal(0);
  progressIncrement=(100/6);
  colours : Record<string, number> = {};
  colourNames = Object.values(Color).slice(0, 14);

  answerTexts:Record<number, string>[]=[
    { 0: "Nope. complete beginner",
      1: "I've had a bird or two",
      2: "I'm an experienced avian enthusiast"
    }
  ,{
      1: "Apartment or condo (small)",
      2: "A house with a yard (spacious)",
      3: "A large house in countryside (huge)"
  },{
      0: "Yes, and they love pets",
      1: "Yes, but they know their distance",
      2: "No kids around"
  },{
      0: "Just a little feeding and interaction",
      1: "A few hours of playtime",
      2: "I'll make the parrot my ride or die"
  },{
      0: "I prefer quieter chirps and chill vibes",
      1: "Mix of both. Some noise is fine, but nothing crazy",
      2: "I want to teach the bird to talk and mimic"
  }];

  ngOnInit(){
    let colourNums = Object.values(Color).slice(14);
    for (let i = 0; i < this.colourNames.length; i++){
      this.colours[this.colourNames[i] as string] = colourNums[i] as number;
    }
  }

  currentOptions(): string[] {
    if (this.currQuestion()==5){
      return Object.keys(this.colours);
    }
    const currentSet = this.answerTexts[this.currQuestion()];
    return currentSet ? Object.values(currentSet) : [];
  }

  onNext(){
    
    switch(this.currQuestion()){
      case 0: this.answers.update(c => ({...c,  CareComplexity: this.selectedAnswer()!})); break;
      case 1: this.answers.update(c => ({...c,  NoiseLevel: this.selectedAnswer()!, 
                                                Size: this.selectedAnswer()!})); break;
      case 2: this.answers.update(c => ({...c,  KidSafety: this.selectedAnswer()!})); break;
      case 3: this.answers.update(c => ({...c,  Sociability: this.selectedAnswer()!, 
                                              Trainability: this.selectedAnswer()!})); break;
      case 4: this.answers.update(c => ({...c,  ChewingRisk: this.selectedAnswer()!,
                                                Talkativeness: this.selectedAnswer()!,
      })); break;
      case 5: this.answers.update(c => ({...c, Color: [this.colourNames[this.selectedAnswer()!] as Color] })); break;
    }
    this.selectedAnswer.set(null);
    if (this.currQuestion()==5){
      this.env.parrotAnswers.set(this.answers());
      this.router.navigate(['/recommendedParrots']);
    }
    this.currQuestion.update(c => c+1);
    this.progress.update(c => c+this.progressIncrement);
    console.log(this.currQuestion())
  }

  onBack(){
    this.currQuestion.update(c => c-1);
    this.progress.update(c => c-this.progressIncrement);
    if (this.progress()<0){
      this.progress.set(0);
    }
  }

  onSubmit(){
  }

}
