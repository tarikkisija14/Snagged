
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Notification } from '../models/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly apiUrl = `${environment.apiUrl}/notification`;

  constructor(private http: HttpClient) {}

  getMyNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/my`);
  }

  getUnreadCount(): Observable<{ unread: number }> {
    return this.http.get<{ unread: number }>(`${this.apiUrl}/my/unread-count`);
  }

  markAsRead(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/read`, {});
  }

  markAllAsRead(): Observable<{ marked: number }> {
    return this.http.put<{ marked: number }>(`${this.apiUrl}/my/read-all`, {});
  }

  deleteNotification(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  deleteAllMyNotifications(): Observable<{ deleted: number }> {
    return this.http.delete<{ deleted: number }>(`${this.apiUrl}/my`);
  }
}
