import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import {
  debounceTime, distinctUntilChanged, switchMap,
  catchError, tap, filter,
} from 'rxjs/operators';
import { ItemService }       from './item-service';
import { SearchSuggestion }  from '../models/search-suggestion.model';

const RECENT_KEY  = 'snagged_recent_searches';
const MAX_RECENT  = 5;
const DEBOUNCE_MS = 300;
const MIN_QUERY   = 2;

@Injectable({ providedIn: 'root' })
export class SearchService {
  private readonly query$       = new Subject<string>();
  private readonly suggestions$ = new BehaviorSubject<SearchSuggestion[]>([]);
  private readonly loading$     = new BehaviorSubject<boolean>(false);

  readonly suggestions: Observable<SearchSuggestion[]> = this.suggestions$.asObservable();
  readonly loading: Observable<boolean>                 = this.loading$.asObservable();

  constructor(private itemService: ItemService) {
    this.query$
      .pipe(
        tap(q => {
          if (q.length < MIN_QUERY) {
            this.suggestions$.next([]);
            this.loading$.next(false);
          } else {
            // Set loading immediately so noResults does not flash during debounce
            this.loading$.next(true);
          }
        }),
        debounceTime(DEBOUNCE_MS),
        distinctUntilChanged(),
        filter(q => q.length >= MIN_QUERY),
        switchMap(q =>
          this.itemService.getSuggestions(q).pipe(
            catchError(() => of([] as SearchSuggestion[]))
          )
        ),
        tap(() => this.loading$.next(false)),
      )
      .subscribe(results => this.suggestions$.next(results));
  }

  search(query: string): void {
    this.query$.next(query.trim());
  }

  clear(): void {
    this.suggestions$.next([]);
    this.loading$.next(false);
  }

  getRecentSearches(): string[] {
    try { return JSON.parse(localStorage.getItem(RECENT_KEY) ?? '[]'); }
    catch { return []; }
  }

  addRecentSearch(term: string): void {
    const t = term.trim();
    if (!t) return;
    const prev    = this.getRecentSearches().filter(s => s !== t);
    const updated = [t, ...prev].slice(0, MAX_RECENT);
    localStorage.setItem(RECENT_KEY, JSON.stringify(updated));
  }

  deleteRecentSearch(term: string): void {
    const updated = this.getRecentSearches().filter(s => s !== term);
    localStorage.setItem(RECENT_KEY, JSON.stringify(updated));
  }

  clearRecentSearches(): void {
    localStorage.removeItem(RECENT_KEY);
  }
}
