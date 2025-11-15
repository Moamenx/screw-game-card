import { Player } from './player.interface';
import { Card } from './card.interface';

export interface GameState {
  players: Player[];
  gameStarted: boolean;
  hostId?: string;
  currentPlayerId?: string;
  currentCard?: Card;
  deck?: Card[];
}
