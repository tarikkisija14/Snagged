import { Injectable } from '@angular/core';
import {environment} from '../../../environments/development';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Category} from '../models/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = `${environment.apiUrl}/category`;
  constructor(private http: HttpClient) {}

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl);
  }

  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  addCategory(name: string): Observable<number> {
    return this.http.post<number>(this.apiUrl, { name });
  }

  updateCategory(id: number, name: string): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/${id}`, { id, name });
  }

  deleteCategory(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }

}
