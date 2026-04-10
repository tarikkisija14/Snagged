// MODIFIED
import {
  Component,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { SearchSuggestion } from '../../models/search-suggestion.model';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-search-suggestions',
  standalone: false,
  templateUrl: './search-suggestions.html',
  styleUrl: './search-suggestions.scss',
})
export class SearchSuggestions {
  @Input() suggestions: SearchSuggestion[] = [];
  @Input() recentSearches: string[]         = [];
  @Input() loading                          = false;
  @Input() query                            = '';
  @Input() activeIndex                      = -1;

  @Output() selectSuggestion = new EventEmitter<SearchSuggestion>();
  @Output() selectRecent     = new EventEmitter<string>();
  @Output() deleteRecent     = new EventEmitter<string>();
  @Output() clearRecent      = new EventEmitter<void>();

  private readonly baseImageUrl = environment.apiUrl.replace('/api', '');

  get showRecent(): boolean {
    return !this.query && this.recentSearches.length > 0;
  }

  get showSuggestions(): boolean {
    return !!this.query && (this.loading || this.suggestions.length > 0);
  }

  get noResults(): boolean {
    return !!this.query && !this.loading && this.suggestions.length === 0;
  }

  get visible(): boolean {
    return this.showRecent || this.showSuggestions || this.noResults;
  }

  getImageUrl(imageUrl?: string): string {
    if (!imageUrl) return `${this.baseImageUrl}/images/items/placeholder.png`;
    return imageUrl.startsWith('http') ? imageUrl : `${this.baseImageUrl}${imageUrl}`;
  }

  highlight(title: string, query: string): { text: string; match: boolean }[] {
    if (!query) return [{ text: title, match: false }];
    const idx = title.toLowerCase().indexOf(query.toLowerCase());
    if (idx === -1) return [{ text: title, match: false }];
    return [
      { text: title.slice(0, idx),                  match: false },
      { text: title.slice(idx, idx + query.length), match: true  },
      { text: title.slice(idx + query.length),      match: false },
    ];
  }
}
