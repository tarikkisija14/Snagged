import { Injectable } from '@angular/core';
import {environment} from '../../../environments/development';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ItemModel} from '../models/item.model';
import {PageResult} from '../models/page-result';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  private apiUrl = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) { }

  getItems(search?: string): Observable<ItemModel[]> {
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<ItemModel[]>(this.apiUrl, { params });
  }

  getItemById(id: number):Observable<ItemModel>{
    return this.http.get<ItemModel>(`${this.apiUrl}/${id}`);
  }

  addItem(item: Partial<ItemModel>): Observable<number> {
    return this.http.post<number>(this.apiUrl, item);
  }

  getPagedItems(page: number = 1, pageSize: number = 10): Observable<PageResult<ItemModel>> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PageResult<ItemModel>>(`${this.apiUrl}/paged`, { params });
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateItem(id: number, item: Partial<ItemModel>): Observable<void> {

    return this.http.put<void>(`${this.apiUrl}/${id}`, item);
  }

  getFilteredItems(filter: any): Observable<PageResult<ItemModel>>
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
    return this.http.get<PageResult<ItemModel>>(`${this.apiUrl}/filtered`, { params });
  }

}
