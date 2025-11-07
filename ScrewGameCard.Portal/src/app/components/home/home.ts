import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GameHub } from '../../services/game-hub';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class HomeComponent {
  playerForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private gameHub: GameHub
  ) {
    this.playerForm = this.formBuilder.group({
      playerName: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  async onJoinGame() {
    if (this.playerForm.valid) {
      const playerName = this.playerForm.get('playerName')?.value;
      await this.gameHub.startConnection();
      await this.gameHub.joinGame(playerName);
      this.router.navigate(['/waiting-room']);
    }
  }
}
