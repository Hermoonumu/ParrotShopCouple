import { Injectable, signal } from '@angular/core';
import { Parrot, ParrotFilter } from '../services/parrot-service';

@Injectable({
  providedIn: 'root',
})
export class Environment {
  readonly ANGULARURI:string = "http://192.168.1.118:4200";
  readonly BASEURI:string = "http://192.168.1.118:5084";
  readonly APIURI:string = this.BASEURI+"/api";
  readonly ASSETURI:string = this.BASEURI+"/images";
  readonly STRIPEKEY:string = "pk_test_51TXlgXFLUywDcCf4X2HImDnJxtoZt6iEg7qQS6dMiU5Z0GaJdSjPqvn2ShwqkJXaR0uDUqYjHRmWgUuirRdIGN4F00ddnLE23E";
  public selectParrot = signal<Parrot|null>(null);
  public parrotAnswers = signal<ParrotFilter>({} as ParrotFilter);
}
