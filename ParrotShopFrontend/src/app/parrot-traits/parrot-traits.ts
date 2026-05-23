import { Component, inject, signal } from '@angular/core';
import { Parrot, ParrotService, Traits, TraitsDTO } from '../services/parrot-service';
import { Environment } from '../global/env';
import { FormBuilder, ReactiveFormsModule, Validators,} from '@angular/forms';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-parrot-traits',
  imports: [ReactiveFormsModule],
  templateUrl: './parrot-traits.html',
  styleUrl: './parrot-traits.css',
})
export class ParrotTraits {
  env = inject(Environment);
  parrotService = inject(ParrotService);
  parrotList = signal<Parrot[]>([]);
  selected = signal(-1);
  selectedParrot = signal<Parrot>({} as Parrot);
  selectedTraits = signal<TraitsDTO>({} as TraitsDTO);
  success = signal(false);

  levels: string[] = ["Low", "Mid", "High"];
  sizes: string[] = ["Small", "Medium", "Large"];
  kidSafety: string[] = ["Yes", "No", "Cautious"];

  formTraits: any;

  newTraits = signal<TraitsDTO>({
    size: null,
    noiseLevel: null,
    sociability: null,
    trainability: null,
    talkativeness: null,
    chewingRisk: null,
    careComplexity: null,
    kidSafety: null
  });

  constructor(private fb : FormBuilder){
    this.formTraits = fb.group({
      size: ['', [Validators.required]],
      noiseLevel: ['', [Validators.required]],
      sociability: ['', [Validators.required]],
      trainability: ['',[Validators.required]],
      talkativeness: ['',[Validators.required]],
      chewingRisk: ['',[Validators.required]],
      careComplexity: ['',[Validators.required]],
      kidSafety: ['',[Validators.required]]
    });
    this.formTraits.valueChanges.subscribe((value:any) => {this.newTraits.set(value)});
  }

  onChange(e : Event){
    this.success.set(false);
    this.selected.set(Number((e.target as HTMLSelectElement).value));
    this.selectedParrot.set(this.parrotList()[this.selected()]);
    this.selectedTraits.set(this.selectedParrot().traits as TraitsDTO);
    if (this.selectedTraits() !=null){
      this.formTraits.patchValue({
        size: this.selectedTraits().size,
        noiseLevel: this.selectedTraits().noiseLevel,
        sociability: this.selectedTraits().sociability,
        trainability: this.selectedTraits().trainability,
        talkativeness: this.selectedTraits().talkativeness,
        chewingRisk: this.selectedTraits().chewingRisk,
        careComplexity: this.selectedTraits().careComplexity,
        kidSafety: this.selectedTraits().kidSafety
    });
    } else {
      this.formTraits.patchValue({
        size: '',
        noiseLevel: '',
        sociability: '',
        trainability: '',
        talkativeness:'',
        chewingRisk: '',
        careComplexity: '',
        kidSafety: ''
    });
    }
  }

  async ngOnInit(){
    await this.getParrots();
    this.selected.set(0);
    this.selectedParrot.set(this.parrotList()[this.selected()]);
    this.selectedTraits.set(this.selectedParrot().traits as TraitsDTO);
    this.formTraits.patchValue({
      size: this.selectedTraits().size,
      noiseLevel: this.selectedTraits().noiseLevel,
      sociability: this.selectedTraits().sociability,
      trainability: this.selectedTraits().trainability,
      talkativeness: this.selectedTraits().talkativeness,
      chewingRisk: this.selectedTraits().chewingRisk,
      careComplexity: this.selectedTraits().careComplexity,
      kidSafety: this.selectedTraits().kidSafety
    });
  }


  async getParrots() {
    this.parrotList.set([]);
    let page = 0;
    let hasMore = true;

    while (hasMore) {
      try {
        const data = await lastValueFrom(this.parrotService.getParrots(page));
        if (data && data.length > 0) {
          this.parrotList.update((pl) => [...pl, ...data]);
          page++;
        } else {
          hasMore = false;
        }
      } catch (err) {
        console.error("Fetch failed at page", page, err);
        hasMore = false;
      }
    }
  }

  onSubmit(){
    console.log(this.newTraits());
    this.parrotService.setTraits(this.selectedParrot().id as number, 
                                this.newTraits()).subscribe({
                                  next:()=>{
                                    this.success.set(true);
                                    this.getParrots();
                                    this.formTraits.reset();
                                  }
                                });
  }
}
