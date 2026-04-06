import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subcategory } from '../models/subcategory';

@Injectable({
  providedIn: 'root',
})
export class SubcategoryService {
  private apiUrl = `${environment.apiUrl}/subcategory`;

  constructor(private http: HttpClient) {}

  getSubcategories(categoryId?: number): Observable<Subcategory[]> {
    let params = new HttpParams();
    if (categoryId !== undefined) {
      params = params.set('categoryId', categoryId.toString());
    }
    return this.http.get<Subcategory[]>(this.apiUrl, { params });
  }

  getSubcategoryById(id: number): Observable<Subcategory> {
    return this.http.get<Subcategory>(`${this.apiUrl}/${id}`);
  }

  addSubcategory(name: string, categoryId: number): Observable<number> {
    return this.http.post<number>(this.apiUrl, { name, categoryId });
  }

  updateSubcategory(id: number, name: string, categoryId: number): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/${id}`, { id, name, categoryId });
  }

  deleteSubcategory(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}
