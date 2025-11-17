import { Player } from './player.interface';

export interface IClientMethods {
  JoinGame: (playerName: string) => Promise<Player>;
  LeaveGame: () => Promise<void>;
  StartGame: () => Promise<void>;
  PlayCard: (cardId: string) => Promise<void>;
  CreateGame: (request: any) => Promise<void>;
}
