import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProfileModel } from '../models/profile.model';
import { ItemModel } from '../models/item.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private readonly baseUrl = `${environment.apiUrl}/profile`;
  private readonly itemsUrl = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) {}

  getProfile(): Observable<ProfileModel> {
    return this.http.get<ProfileModel>(`${this.baseUrl}/me`);
  }

  getMyItems(): Observable<ItemModel[]> {
    return this.http.get<ItemModel[]>(`${this.itemsUrl}/my`);
  }
}
