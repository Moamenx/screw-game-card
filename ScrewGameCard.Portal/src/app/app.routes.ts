import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'lobby',
    loadComponent: () => import('./features/lobby/lobby.component').then(m => m.LobbyComponent)
  },
  {
    path: 'game/:id',
    loadComponent: () => import('./features/game/game.component').then(m => m.GameComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
