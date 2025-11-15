import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameService } from '../../core/services/game.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {
  gameId: string = '';
  gameState: any = null;

  constructor(
    private route: ActivatedRoute,
    private gameService: GameService
  ) {}

  ngOnInit() {
    this.gameId = this.route.snapshot.paramMap.get('id') || '';
    this.loadGameState();
  }

  loadGameState() {
    this.gameService.getGameState(this.gameId).subscribe(state => {
      this.gameState = state;
    });
  }
}
