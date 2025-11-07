import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home';
import { WaitingRoom } from './components/waiting-room/waiting-room';
import { GameRoom } from './components/game-room/game-room';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'waiting-room', component: WaitingRoom },
  { path: 'game-room', component: GameRoom },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
