import { Player } from './player.interface';
import { Card } from './card.interface';
import { GameState } from './game-state.interface';

export interface IServerMethods {
  PlayerJoined: (player: Player, isHost: boolean) => void;
  PlayerLeft: (playerId: string, newHostId?: string) => void;
  GameStarted: () => void;
  UpdateGameState: (gameState: GameState) => void;
  DealCards: (cards: Card[]) => void;
  CardPlayed: (playerId: string, card: Card) => void;
  GameOver: (winnerId: string) => void;
  Error: (message: string) => void;
  GameCreated: (game: any) => void;
}
