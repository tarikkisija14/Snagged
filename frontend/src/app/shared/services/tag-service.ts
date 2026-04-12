import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TagSuggestion, PopularTag } from '../models/tag-suggestion.model';

@Injectable({ providedIn: 'root' })
export class TagService {
  private readonly apiUrl = `${environment.apiUrl}/tags`;

  constructor(private http: HttpClient) {}

  getSuggestions(query: string, limit = 10): Observable<TagSuggestion[]> {
    const params = new HttpParams()
      .set('Query', query)
      .set('Limit', limit.toString());
    return this.http.get<TagSuggestion[]>(`${this.apiUrl}/suggestions`, { params });
  }

  getPopular(limit = 20): Observable<PopularTag[]> {
    const params = new HttpParams().set('Limit', limit.toString());
    return this.http.get<PopularTag[]>(`${this.apiUrl}/popular`, { params });
  }
}
