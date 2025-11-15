import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private apiUrl = environment.apiUrl + 'game'; // Use environment API URL

  constructor(private http: HttpClient) {}

  createGame(request: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, request);
  }

  joinGame(request: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/join`, request);
  }

  getGameState(gameId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${gameId}/state`);
  }

  // Add more methods as needed
}
