import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

interface AuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'https://localhost:7163/api/auth';

  constructor(private http: HttpClient){}

  register(data:{email: string; password: string; firstName: string; lastName: string })
    : Observable<AuthResponse>
  {
    return this.http.post<AuthResponse>(this.baseUrl + '/register', data);
  }

  login(data: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data);
  }
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('token');
  }

}
