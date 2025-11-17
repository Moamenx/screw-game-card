import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse } from '../../features/auth/models/auth-response.interface';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload } from '../models/token.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);
  private apiUrl = 'https://localhost:5001/api/auth'; // Adjust as needed

  constructor(private http: HttpClient) {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');
    if (accessToken) {
      this.currentUserSubject.next(this.decodeToken(accessToken));
    }
    if (refreshToken) {
      this.refreshTokenSubject.next(refreshToken);
    }
  }

  register(username: string, password: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}auth/register`, { username, password });
  }

  login(username: string, password: string): Observable<AuthResponse> {
    return this.http.post(`${environment.apiUrl}auth/login`, { username, password }).pipe(
      tap(response => {
        if (response.accessToken) {
          localStorage.setItem('accessToken', response.accessToken);
          if (response.refreshToken) {
            localStorage.setItem('refreshToken', response.refreshToken);
            this.refreshTokenSubject.next(response.refreshToken);
          }
          this.currentUserSubject.next(response.player);
        }
      })
    );
  }
  refreshToken(): Observable<any> {
    const refreshToken = this.refreshTokenSubject.value;
    if (!refreshToken) {
      this.logout();
      return throwError(() => new Error('No refresh token'));
    }

    return this.http.post(`${environment.apiUrl}auth/refresh`, { refreshToken }).pipe(
      tap((response: any) => {
        localStorage.setItem('accessToken', response.accessToken);
        if (response.refreshToken) {
          localStorage.setItem('refreshToken', response.refreshToken);
          this.refreshTokenSubject.next(response.refreshToken);
        }
        this.currentUserSubject.next(response.player);
      }),
      catchError((error) => {
        this.logout();
        return throwError(() => error);
      })
    );
  }

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.currentUserSubject.next(null);
    this.refreshTokenSubject.next(null);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  private decodeToken(token: string): any {
    try {
      const decoded = jwtDecode<JwtPayload>(token);
      return { id: decoded.sub, name: decoded.fullName};
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  setToken(token: string): void {
    localStorage.setItem('accessToken', token);
  }
}
