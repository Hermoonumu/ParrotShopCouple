import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Environment } from '../global/env';
import { Observable } from 'rxjs';
import { Parrot } from './parrot-service';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  
  env = inject(Environment);


  constructor(private http: HttpClient){ }

  executeSearchSuggestions(query: string): Observable<string[]>{
    return this.http.get<string[]>(this.env.APIURI+"/items/searchSuggestions?query="+query);
  }

  executeSearch(query: string, page: number): Observable<Parrot[]>{
    return this.http.get<Parrot[]>(this.env.APIURI+"/items/search?query="+query+"&page="+page);
  }

}
