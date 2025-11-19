import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'game',
    loadChildren: () => import('./features/game/game.routes').then(m => m.GAME_ROUTES),
    canActivate: [AuthGuard]
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/components/register/register.component').then(m => m.RegisterComponent),
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/components/login/login.component').then(m => m.LoginComponent),
  },
  {
    path: '',
    redirectTo: 'game',
    pathMatch: 'full'
  }
];

