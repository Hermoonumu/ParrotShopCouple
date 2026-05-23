import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Parrot, ParrotService } from '../services/parrot-service';
import { SearchService } from '../services/search-service';
import { Searchbar } from '../searchbar/searchbar';
import { ItemCard } from '../item-card/item-card';

@Component({
  selector: 'app-search',
  imports: [Searchbar, ItemCard],
  templateUrl: './search.html',
  styleUrl: './search.css',
})
export class Search {
  search = inject(SearchService);
  results = signal<Parrot[]>([]);
  query = signal('');
  page = signal(0);

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const q = params['query'];
      if (q) {
        this.query.set(q);
        this.performSearch(q);
      }
    });
  }

  constructor(private route: ActivatedRoute, 
    private parrotService: ParrotService, private router: Router) {}

  performSearch(query: string) {
    this.query.set(query);
    this.page.set(0);
    this.results.set([]);
    this.search.executeSearch(query, this.page()).subscribe((data) => {
      this.results.set(data);
    });
  }

  more(){
    this.page.update((p) => p + 1);
    this.search.executeSearch(this.query(), this.page()).subscribe((data) => {
      this.results.update((r) => [...r, ...data]);
    });
  }

  gotoParrot(id:number){
    this.router.navigate(['/parrot/'+id]);
  }

}
