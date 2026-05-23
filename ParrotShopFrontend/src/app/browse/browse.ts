import { Component, inject, signal } from '@angular/core';
import { Form, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ItemCard } from '../item-card/item-card';
import { Parrot, ParrotFilter, ParrotService } from '../services/parrot-service';
// ... other imports

@Component({
  selector: 'app-browse',
  standalone: true,
  imports: [ReactiveFormsModule, ItemCard],
  templateUrl: './browse.html',
  styleUrls: ['./browse.css']
})
export class Browse {
  parrotService = inject(ParrotService);
  results = signal<Parrot[]>([]);
  page = signal(0);

  filterForm: FormGroup;
  filter = signal<ParrotFilter>({} as ParrotFilter);
  
  constructor(fb : FormBuilder){
    this.filterForm = fb.group({
      species: [null],
      gender: [null],
      color: [null],
      priceFrom: [null],
      priceTo: [null],
      ascendingPrice: [null],
      size: [null],
      noiseLevel: [null],
      sociability: [null],
      talkativeness: [null],
      trainability: [null],
      chewingRisk: [null],
      careComplexity: [null],
      kidSafety: [null]
    });
    this.filterForm.valueChanges.subscribe((value:any) => {this.filter.set(value)});
  }

  ngOnInit(){
    this.applyFilters();
  }

  public applyFilters() {
    console.log(this.filter());
    this.parrotService.filter(this.filter()).subscribe((data) => {
      this.results.set(data);
      console.log(this.results());
    });
  }

  public clearFilters() {
    this.filterForm.reset();
    this.applyFilters(); 
  }

}

