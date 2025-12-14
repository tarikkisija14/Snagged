import { Injectable } from '@angular/core';
import {environment} from '../../../environments/development';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Item} from '../models/item';
import {PageResult} from '../models/page-result';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  private apiUrl = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) { }

  getItems(search?: string): Observable<Item[]> {
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<Item[]>(this.apiUrl, { params });
  }

  getItemById(id: number):Observable<Item>{
    return this.http.get<Item>(`${this.apiUrl}/${id}`);
  }

  addItem(item: Partial<Item>): Observable<number> {
    return this.http.post<number>(this.apiUrl, item);
  }

  getPagedItems(page: number = 1, pageSize: number = 10): Observable<PageResult<Item>> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PageResult<Item>>(`${this.apiUrl}/paged`, { params });
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateItem(id: number, item: Partial<Item>): Observable<void> {

    return this.http.put<void>(`${this.apiUrl}/${id}`, item);
  }

  getFilteredItems(filter: any): Observable<PageResult<Item>>
  {
    let params = new HttpParams();

    Object.keys(filter).forEach(key => {
      const value = filter[key];

      if (value !== undefined && value !== null) {
        if (Array.isArray(value)) {

          value.forEach(item => {
            params = params.append(key, item.toString());
          });
        } else {

          params = params.set(key, value.toString());
        }
      }
    });

    console.log('Params being sent:', params.toString());
    return this.http.get<PageResult<Item>>(`${this.apiUrl}/filtered`, { params });
  }

}
