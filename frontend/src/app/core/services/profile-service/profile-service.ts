import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProfileModel } from '../../../shared/models/profile.model';
import{ItemModel} from '../../../shared/models/item.model';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {

  private baseUrl = 'https://localhost:7163/api/profile';
  private itemsUrl = 'https://localhost:7163/api/items';
  constructor(private http: HttpClient) {}

  getProfile(): Observable<ProfileModel> {
    return this.http.get<ProfileModel>(`${this.baseUrl}/me`);
  }

  getMyItems(){
    return this.http.get<ItemModel[]>(`${this.itemsUrl}/my`);
  }
}
