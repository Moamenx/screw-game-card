import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameHub } from '../../services/game-hub';
import { computed } from '@angular/core';

export interface Card {
  suit: 'hearts' | 'diamonds' | 'clubs' | 'spades';
  value: string;
}

@Component({
  selector: 'app-game-room',
  standalone: false,
  templateUrl: './game-room.html',
  styleUrl: './game-room.css',
})
export class GameRoom implements OnInit, OnDestroy {
  players = computed(() => this.gameHub.gameState().players);
  currentPlayerCards: Card[] = []; // Will be populated from SignalR
  selectedCard: Card | null = null;

  constructor(
    private gameHub: GameHub,
    private router: Router
  ) {}

  ngOnInit() {
    if (!this.gameHub.gameState().gameStarted) {
      this.router.navigate(['/']);
    }
    
    // Add game-specific SignalR handlers here
  }

  ngOnDestroy() {
    this.gameHub.leaveGame();
  }

  onCardSelected(card: Card) {
    this.selectedCard = card;
  }

  async playCard() {
    if (this.selectedCard) {
      // await this.gameHub.playCard(this.selectedCard);
      this.selectedCard = null;
    }
  }

  async leaveGame() {
    await this.gameHub.leaveGame();
    this.router.navigate(['/']);
  }
}
