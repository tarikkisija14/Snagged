import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemModel } from '../models/item.model';
import { PageResult } from '../models/page-result';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  private readonly apiUrl = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) {}

  getItems(search?: string): Observable<ItemModel[]> {
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<ItemModel[]>(this.apiUrl, { params });
  }

  getItemById(id: number): Observable<ItemModel> {
    return this.http.get<ItemModel>(`${this.apiUrl}/${id}`);
  }

  addItem(item: Partial<ItemModel>): Observable<number> {
    return this.http.post<number>(this.apiUrl, item);
  }

  getPagedItems(page: number = 1, pageSize: number = 10): Observable<PageResult<ItemModel>> {
    const params = new HttpParams()
      .set('Paging.Page', page.toString())
      .set('Paging.PageSize', pageSize.toString());
    return this.http.get<PageResult<ItemModel>>(`${this.apiUrl}/paged`, { params });
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateItem(id: number, item: Partial<ItemModel>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, item);
  }

  getFilteredItems(filter: {
    page?: number;
    pageSize?: number;
    sortBy?: string;
    sortOrder?: string;
    categoryIds?: number[];
    subcategoryIds?: number[];
    conditions?: string[];
    minPrice?: number;
    maxPrice?: number;
  }): Observable<PageResult<ItemModel>> {
    let params = new HttpParams();

    if (filter.page != null)
      params = params.set('Paging.Page', filter.page.toString());
    if (filter.pageSize != null)
      params = params.set('Paging.PageSize', filter.pageSize.toString());
    if (filter.sortBy)
      params = params.set('SortBy', filter.sortBy);
    if (filter.sortOrder)
      params = params.set('SortOrder', filter.sortOrder);
    if (filter.categoryIds?.length)
      filter.categoryIds.forEach(id => (params = params.append('CategoryIds', id.toString())));
    if (filter.subcategoryIds?.length)
      filter.subcategoryIds.forEach(id => (params = params.append('SubcategoryIds', id.toString())));
    if (filter.conditions?.length)
      filter.conditions.forEach(c => (params = params.append('Conditions', c)));
    if (filter.minPrice != null)
      params = params.set('MinPrice', filter.minPrice.toString());
    if (filter.maxPrice != null)
      params = params.set('MaxPrice', filter.maxPrice.toString());

    return this.http.get<PageResult<ItemModel>>(`${this.apiUrl}/filtered`, { params });
  }
}
