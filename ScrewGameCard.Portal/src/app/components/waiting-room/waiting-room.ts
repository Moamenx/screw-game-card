import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameHub, Player } from '../../services/game-hub';
import { computed } from '@angular/core';

@Component({
  selector: 'app-waiting-room',
  standalone: false,
  templateUrl: './waiting-room.html',
  styleUrl: './waiting-room.css',
})
export class WaitingRoom implements OnInit, OnDestroy {
  players = computed(() => this.gameHub.gameState().players);
  isHost = computed(() => {
    const state = this.gameHub.gameState();
    return state.hostId === state.players.find(p => p)?.id;
  });

  constructor(
    private gameHub: GameHub,
    private router: Router
  ) {}

  ngOnInit() {
    // If game started, redirect to game room
    if (this.gameHub.gameState().gameStarted) {
      this.router.navigate(['/game-room']);
    }
  }

  ngOnDestroy() {
    // Clean up if user leaves without starting game
    this.gameHub.leaveGame();
  }

  async startGame() {
    if (this.isHost()) {
      await this.gameHub.startGame();
      this.router.navigate(['/game-room']);
    }
  }

  async leaveGame() {
    await this.gameHub.leaveGame();
    this.router.navigate(['/']);
  }
}
