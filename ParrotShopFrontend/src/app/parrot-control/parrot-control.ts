import { Component, inject, signal } from '@angular/core';
import { Color, NewParrotDTO, Parrot, ParrotService, Species, Traits } from '../services/parrot-service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Environment } from '../global/env';
import { Router } from '@angular/router';

@Component({
  selector: 'app-parrot-control',
  imports: [ReactiveFormsModule],
  templateUrl: './parrot-control.html',
  styleUrl: './parrot-control.css',
})
export class ParrotControl {
  env = inject(Environment);
  parrotService = inject(ParrotService);
  parrotList = signal<Parrot[]>([]);
  selected = signal(-1);
  selectedParrot = signal<Parrot>({} as Parrot);
  selectedFile: File | null = null;
  colourNames = Object.values(Color).slice(0, 14);
  speciesNames = Object.values(Species).slice(0, 15);
  colours : Record<string, number> = {};
  species: Record<string, number> = {};
  success = signal(false);

  formNewParrot: any;

  formAlterParrot: any;

  alterParrot = signal<Parrot>({
    id: null,
    name: null,
    description:null,
    price: null,
    discount: null,
    imageUrl: null,
    categoryId: null,
    isDeleted: null,
    age: null,
    colorType: null,
    genderType: null,
    speciesType: null } as Parrot);

  newParrot = signal<NewParrotDTO>({
      name: '',
      description:'',
      imageUrl: '',
      price: 0,
      discount: 0,
      age: 0,
      colorType: [],
      genderType: 0,
      speciesType: null
  });


  constructor(private fb : FormBuilder, private router: Router){

    this.formNewParrot = fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required]],
      imageUrl: [''],
      price: [null, [Validators.required, Validators.min(0)]],
      discount: [0, [Validators.min(0), Validators.max(100)]],
      age: [null, [Validators.required, Validators.min(0)]],
      colorType: [[]],
      genderType: [0, [Validators.required]],
      speciesType: [0, [Validators.required, Validators.min(1)]]
    });

    this.formAlterParrot = fb.group({
      name: [null, [Validators.required]],
      description: [null],
      imageUrl: [null],
      price: [null, [Validators.required, Validators.min(0)]],
      discount: [null, [Validators.min(0), Validators.max(100)]],
      isDeleted: [null],
      age: [null, [Validators.required, Validators.min(0)]],
      colorType: [[]],
      genderType: [null, [Validators.required]],
      speciesType: [null, [Validators.required]]
    });
    this.formNewParrot.valueChanges.subscribe((value:any) => {this.newParrot.set(value)});
    this.formAlterParrot.valueChanges.subscribe((value:any) => {this.alterParrot.set(value)});

  }

  ngOnInit(){
    this.getParrots();
    let colourNums = Object.values(Color).slice(14);
    let speciesNums = Object.values(Species).slice(15);
    for (let i = 0; i < this.colourNames.length; i++){
      this.colours[this.colourNames[i] as string] = colourNums[i] as number;
    }
    for (let i = 0; i < this.speciesNames.length; i++){
      this.species[this.speciesNames[i] as string] = speciesNums[i] as number;
    }
  }

  onChange(e : Event){
    this.success.set(false);

    this.selected.set(Number((e.target as HTMLSelectElement).value));
    this.selectedParrot.set(this.parrotList()[this.selected()]);
    console.log(this.selectedParrot());
    this.formAlterParrot.patchValue({
      name: this.selectedParrot().name,
      description: this.selectedParrot().description,
      price: this.selectedParrot().price,
      discount: this.selectedParrot().discount,
      age: this.selectedParrot().age,
      genderType: this.selectedParrot().genderType, 
      speciesType: this.selectedParrot().speciesType,
      imageUrl: this.selectedParrot().imageUrl,
      colorType: this.selectedParrot().colorType,
      isDeleted: this.selectedParrot().isDeleted
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      console.log(file);
      this.selectedFile = file;
      this.parrotService.uploadParrot(this.selectedFile)?.subscribe({
      next: (data)=>{
        this.formNewParrot.patchValue({ imageUrl: this.env.ASSETURI+((data as unknown as {imageUrl:string}).imageUrl) });
        this.formAlterParrot.patchValue({ imageUrl: this.env.ASSETURI+((data as unknown as {imageUrl:string}).imageUrl) });
      }
    });
    }
  }

  get currentColors(): number[] {
    if (this.selected()==-1){
      return this.formNewParrot.get('colorType').value;
    } else {
      return this.formAlterParrot.get('colorType').value.split(", ").map((x : string) => this.colours[x]);
    }
  }

  addColor(event: Event) {
    const select = event.target as HTMLSelectElement;
    const colorValue = Number(select.value);
    if (!colorValue || this.currentColors.includes(colorValue)) {
      select.value = '0'; // Reset the dropdown
      return;
    }
    const updatedColors = [...this.currentColors, colorValue];
    if(this.selected()==-1){
      this.formNewParrot.patchValue({ colorType: updatedColors });
    } else {
      const str = updatedColors.map(c => this.parrotService.colours[c]).join(", ");
      this.formAlterParrot.patchValue({ colorType: str });
    }
    select.value = "0"; 
  }

  removeColor(colorToRemove: number) {
    const updatedColors = this.currentColors.filter(c => c !== colorToRemove);
    if (this.selected()==-1){
      this.formNewParrot.patchValue({ colorType: updatedColors });
    } else {
      const str = updatedColors.map(c => this.parrotService.colours[c]).join(", ");
      this.formAlterParrot.patchValue({ colorType: str });
    }
  }

  getColorName(colorValue: number): string {
    return Object.keys(this.colours).find(key => this.colours[key] == colorValue) || 'Unknown';
  }



  getParrots(page: number = 0) {
    if (page === 0) this.parrotList.set([]);

    this.parrotService.getParrots(page).subscribe({
      next: (data) => {
        if (data && data.length > 0) {
          this.parrotList.update((pl) => [...pl, ...data]);
          this.getParrots(page + 1);
        }
        this.parrotList.update( c => c.sort(this.parrotService.compare) );
      },
      error: (err) => console.error("Error fetching parrots", err)
    });
  }

  onSubmit(){
    if (this.selected()==-1){
      this.parrotService.createParrot(this.newParrot()).subscribe({
        next:()=>{
          this.success.set(true);
          this.getParrots();
          this.selected.set(-1);
          this.selectedParrot.set({} as Parrot);
          this.formAlterParrot.reset();
          this.formNewParrot.reset();
          this.selectedFile = null;
        }
      });
    } else {
      console.log(this.alterParrot());
      this.parrotService.alterParrot(this.alterParrot(), this.selectedParrot().id as number).subscribe({
        next:()=>{
          this.success.set(true);
          this.getParrots();
          this.selected.set(-1);
          this.selectedParrot.set({} as Parrot);
          this.formAlterParrot.reset();
          this.formNewParrot.reset();
          this.selectedFile = null;
        }
      });
    }

  }

  onDelete(){
    this.parrotService.deleteParrot(this.selectedParrot().id as number).subscribe(
      {
        next:()=>{
          this.success.set(true);
          this.getParrots();
          this.selected.set(-1);
          this.selectedParrot.set({} as Parrot);
          this.formAlterParrot.reset();
          this.formNewParrot.reset();
          this.selectedFile = null;
        }
      }
    );

  }

  onUndoDelete(){
    this.parrotService.undoDelete(this.selectedParrot().id as number).subscribe(
      {
        next:()=>{
          this.success.set(true);
          this.getParrots();
          this.selected.set(-1);
        }
      });
  }
}
