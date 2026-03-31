import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../../environments/environment';

interface AuthResponse { token: string; }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = `${environment.apiUrl}/auth`;

  private currentUserSubject = new BehaviorSubject<number | null>(
    this.extractUserIdFromToken()
  );
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  register(data: {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
  }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`, data).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        this.currentUserSubject.next(this.extractUserIdFromToken(res.token));
      })
    );
  }

  login(data: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        this.currentUserSubject.next(this.extractUserIdFromToken(res.token));
      })
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }


  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserId(): number | null {
    return this.currentUserSubject.value;
  }

  private extractUserIdFromToken(token?: string): number | null {
    const jwt = token ?? this.getToken();
    if (!jwt) return null;
    try {
      const decoded: any = jwtDecode(jwt);
      const userIdRaw = decoded.userId ?? decoded.sub;
      const userId = userIdRaw ? Number(userIdRaw) : null;
      return userId !== null && !isNaN(userId) ? userId : null;
    } catch (e) {
      console.error('Invalid token', e);
      return null;
    }
  }
}
