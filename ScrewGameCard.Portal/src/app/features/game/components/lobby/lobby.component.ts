import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { GameHubService } from '../../../../core/services/game-hub.service';
import { CommonModule } from '@angular/common';
import { CreateGameRequest } from '../../../../core/models/game.interface';
import { TranslatePipe } from '../../../../shared/pipes/translate.pipe';

@Component({
  selector: 'app-lobby',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonComponent, TranslatePipe],
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.scss']
})

export class LobbyComponent implements OnInit {
  createForm: FormGroup;
  joinForm: FormGroup;
  showCreateForm = false;
  showJoinForm = false;

  constructor(
    private fb: FormBuilder,
    private gameHubService: GameHubService,
    private router: Router
  ) {
    this.createForm = this.fb.group({
      roomName: ['', Validators.required],
      passCode: [''],
      maxPlayers: [4, [Validators.required, Validators.min(2), Validators.max(8)]]
    });

    this.joinForm = this.fb.group({
      gameId: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.gameHubService.startConnection();
    this.gameHubService.lobbyError$.subscribe(error => {
      alert(error);
    });
  }

  async createGame() {
    if (this.createForm.valid) {
      const request: CreateGameRequest = {
        roomName: this.createForm.value.roomName,
        passCode: this.createForm.value.passCode,
        maximumNumberOfPlayers: this.createForm.value.maxPlayers,
        type: 1, // Classic
        host: {
          connectionId: this.gameHubService.getConnectionId(),
          name: 'Player'
        }
      };
      try {
        const game = await this.gameHubService.createGame(request);
        this.router.navigate(['/game', game.id]);
      } catch (error) {
        alert('Failed to create game: ' + error);
      }
    }
  }

  joinGame() {
    // Implement join logic
    alert('Join functionality to be implemented');
  }
}
