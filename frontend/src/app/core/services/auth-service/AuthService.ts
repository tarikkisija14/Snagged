import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

interface AuthResponse { token: string; }
interface JwtPayload { sub: string; exp: number; email: string; }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = 'https://localhost:7163/api/auth';
  private currentUserSubject = new BehaviorSubject<number | null>(this.extractUserIdFromToken());
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  register(data: { email: string; password: string; firstName: string; lastName: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.baseUrl + '/register', data).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        const userId = this.extractUserIdFromToken(res.token);
        this.currentUserSubject.next(userId);
      })
    );
  }

  login(data: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        const userId = this.extractUserIdFromToken(res.token);
        this.currentUserSubject.next(userId);
      })
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded: any = jwtDecode(token);
    }
    return token;
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
      return (userId !== null && !isNaN(userId)) ? userId : null;  // Added NaN safety
    } catch (e) {
      console.error('Invalid token', e);
      return null;
    }
  }
}
