import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse } from '../../features/auth/models/auth-response.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

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
    // Simple decode, in real app use jwt-decode library
    const payload = JSON.parse(atob(token.split('.')[1]));
    return { id: payload.sub, name: payload.unique_name };
  }
}
