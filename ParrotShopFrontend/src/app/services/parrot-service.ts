import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Environment } from '../global/env';
import { Observable } from 'rxjs';


export interface Parrot {
  id: number | null;
  name: string | null;
  description?: string | null;
  price: number | null;
  discount: number | null;
  imageUrl?: string | null;
  categoryId?: number | null;
  isDeleted: boolean | null;
  age: number | null;
  colorType: Color | null;
  genderType: Gender | null;
  speciesType: Species | null;
  traits: ParrotTraits | null;
  traitsId: number | null;
}


export interface NewParrotDTO {
  name: string;
  description?: string | null;
  price?: number | null;
  discount?: number | null;
  imageUrl?: string | null;
  age: number;
  colorType: Color[];
  genderType: Gender;
  speciesType: Species|null;
}

export enum Color {
  Black = 1,
  White = 1 << 1,
  Red = 1 << 2,
  Green = 1 << 3,
  Blue = 1 << 4,
  Grey = 1 << 5,
  Colourful = 1 << 6,
  Pastel = 1 << 7,
  Muted = 1 << 8,
  Yellow = 1 << 9,
  Orange = 1 << 10,
  Brown = 1 << 11,
  Pink = 1 << 12,
  Purple = 1 << 13
}

export enum Gender {
  Male = 0,
  Female = 1
}

export enum Species {
  // Small Parrots
  Budgerigar = 1,
  Cockatiel = 2,
  Lovebird = 3,
  Parrotlet = 4,

  // Medium Parrots
  Conure = 10,
  Caique = 11,
  Ringneck = 12,
  Lorikeet = 13,
  Quaker = 14,

  // Large Parrots
  AfricanGrey = 20,
  AmazonParrot = 21,
  Eclectus = 22,
  Cockatoo = 23,
  Macaw = 24,
  Unknown = 100
}

export interface ParrotTraits {
  id: number;
  parrotId: number;
  size?: Size | null;
  noiseLevel?: Level | null;
  sociability?: Level | null;
  trainability?: Level | null;
  talkativeness?: Level | null;
  chewingRisk?: Level | null;
  careComplexity?: Level | null;
  kidSafety?: KidSafety | null;
}

export interface TraitsDTO{
  size?: Size | null;
  noiseLevel?: Level | null;
  sociability?: Level | null;
  trainability?: Level | null;
  talkativeness?: Level | null;
  chewingRisk?: Level | null;
  careComplexity?: Level | null;
  kidSafety?: KidSafety | null;
}

export enum Traits {
  Size = 0,
  NoiseLevel = 1,
  Sociability = 2,
  Trainability = 3,
  Talkativeness = 4,
  ChewingRisk = 5,
  CareComplexity = 6,
  KidSafety = 9
}

export enum Size {
  Small = 1,
  Medium = 2,
  Large = 3
}

export enum Level {
  Low = 0,
  Mid = 1,
  High = 2
}

export enum KidSafety {
  Yes = 0,
  No = 1,
  Cautious = 2
}
export interface JsonPatch {
  op: string;
  path: string;
  value: any;

}

export interface ParrotFilter{
  Size:Size|null;
  NoiseLevel:Level|null;
  Sociability:Level|null;
  Trainability:Level|null;
  Talkativeness:Level|null;
  ChewingRisk:Level|null;
  CareComplexity:Level|null;
  KidSafety:KidSafety|null;
  Color:Color[]|null;
  Species: Species|null;
  Gender: Gender|null;
  PriceFrom: number|null;
  PriceTo: number|null;
  AscendingPrice: boolean|null;
}

@Injectable({
  providedIn: 'root',
})
export class ParrotService {
  constructor (private http : HttpClient){
    let colourNames = Object.values(Color).slice(0, 14);
    let colourNums = Object.values(Color).slice(14);
    for (let i = 0; i < colourNames.length; i++){
      this.colours[colourNums[i] as number] = colourNames[i] as string;
    }
  }
  env = inject(Environment);
  colours : Record<number, string> = {};



  getParrots(page:number):Observable<Parrot[]>{
    const params = new HttpParams()
    .set('page', page.toString())
    .set('ignoreSoftDelFilter', 'true'); 

    return this.http.get<Parrot[]>(this.env.APIURI+"/parrots", 
    {withCredentials: true,
      params:params
    });
  }

  uploadParrot(selectedFile: File):Observable<string>|null {
    if (!selectedFile) return null;
    const formData = new FormData();
    formData.append('file', selectedFile);
    return this.http.post<string>('api/uploads', formData);
  }

  createParrot(parrot: NewParrotDTO):Observable<any>{
    return this.http.post(this.env.APIURI+"/parrots", parrot, {withCredentials: true});
  }

  alterParrot(parrot: Parrot, id:number):Observable<any>{
    console.log(parrot);
    let patch: JsonPatch[] = [];
    let patchFields = Object.keys(parrot);
    let patchValues = Object.values(parrot);
    for (let i = 0; i < patchFields.length; i++){
      if (patchValues[i]==null||patchValues[i]==""||patchFields[i]=="id") continue;
      patch.push({
        op: "replace",
        path: "/"+patchFields[i],
        value: patchValues[i],
      });
    }
    return this.http.patch(this.env.APIURI+"/parrots/"+id, patch, {withCredentials: true, headers:{
      'Content-Type': 'application/json-patch+json'
    }});
  }

  deleteParrot(id: number):Observable<any>{
    const patch: JsonPatch[] = [{
      op: "replace",
      path: "/isDeleted",
      value: "true",
    }];
    return this.http.patch(this.env.APIURI+"/parrots/"+id, 
      patch, 
      {withCredentials: true, 
        headers:{
          'Content-Type': 'application/json-patch+json'
    }});
  }

  undoDelete(id: number):Observable<any>{
    const patch: JsonPatch[] = [{
      op: "replace",
      path: "/isDeleted",
      value: "false",
    }];
    return this.http.patch(this.env.APIURI+"/parrots/"+id, 
      patch, {withCredentials:true, headers:{
        'Content-Type': 'application/json-patch+json'
      }});
  }


  setTraits(id: number, traits: TraitsDTO):Observable<any>{
    console.log(traits);
    return this.http.post(this.env.APIURI+"/parrots/traits/"+id, traits, {withCredentials: true});
  }


  getSingularParrot(id: number):Observable<Parrot>{
    return this.http.get<Parrot>(this.env.APIURI+"/parrots/"+id, {withCredentials: true, params:{
      includeTraits: true
    }});

  }


  compare( a:Parrot, b:Parrot ) {
    if(a.id==null || b.id==null) return 0;
    if ( a.id < b.id ){
      return -1;
    }
    if ( a.id > b.id ){
      return 1;
    }
    return 0;
  }

  filter(ParrotFilter: ParrotFilter):Observable<Parrot[]>{
    console.log(ParrotFilter)
    const keys = Object.keys(ParrotFilter);
    const values = Object.values(ParrotFilter);
    let params = new HttpParams();
    for (let i = 0; i < keys.length; i++){
      if (values[i]==null) continue;
      params = params.set(keys[i], values[i].toString());
    }
    return this.http.get<Parrot[]>(this.env.APIURI+"/parrots/filter", {withCredentials: true, params:params});
  }

  recommendation(ParrotFilter: ParrotFilter):Observable<Parrot[]>{
    console.log(ParrotFilter)
    const keys = Object.keys(ParrotFilter);
    const values = Object.values(ParrotFilter);
    let params = new HttpParams();
    for (let i = 0; i < keys.length; i++){
      if (values[i]==null) continue;
      params = params.set(keys[i], values[i].toString());
    }
    return this.http.get<Parrot[]>(this.env.APIURI+"/parrots/parrotRecommendation", {withCredentials: true, params:params});
  }
}
