import { Component, ElementRef, HostListener, inject, signal } from '@angular/core';
import { SearchService } from '../services/search-service';
import { Router, RouterLink } from "@angular/router";

@Component({
  selector: 'app-searchbar',
  imports: [RouterLink],
  templateUrl: './searchbar.html',
  styleUrl: './searchbar.css',
})
export class Searchbar {
private eRef = inject(ElementRef);

  fieldState = signal(false);
  router = inject(Router);
  search = inject(SearchService);
  result = signal<string[]>([]);
  query = signal('');
  
  onInput(s : string){
    this.query.set(s);
    if (!s) {
      this.result.set([]);
      return;
    }
    this.search.executeSearchSuggestions(s).subscribe((data)=>{
      this.result.set(data);
    });
  }

  searchOn(){
    this.fieldState.set(true);
  }

  @HostListener('document:click', ['$event'])
  clickOut(event: Event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.fieldState.set(false);
    }
  }

  goto(s: string){
    this.router.navigate(['/search'], { queryParams: { query: s } });
    this.fieldState.set(false);
  }

  gotoLink(s: string){
    this.query.set(s);
    this.router.navigate(['/search'], { queryParams: { query: this.query() } });
    this.fieldState.set(false);
  }
}
