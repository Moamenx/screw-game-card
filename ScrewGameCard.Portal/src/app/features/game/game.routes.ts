import { Routes } from '@angular/router';

export const GAME_ROUTES: Routes = [
  {
    path: 'lobby',
    loadComponent: () => import('./components/lobby/lobby.component').then(m => m.LobbyComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./components/board/game.component').then(m => m.GameComponent)
  },
  {
    path: '',
    redirectTo: 'lobby',
    pathMatch: 'full'
  }
];
