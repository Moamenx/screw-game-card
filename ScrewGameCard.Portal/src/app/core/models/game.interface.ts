import { Player } from "./player.interface";

export interface GameDto {
  id: string;
  name?: string;
  status?: GameStatus;
}

export interface CreateGameRequest {
  roomName: string;
  passCode?: string;
  maximumNumberOfPlayers: number;
  type: number;
  host: PlayerDto;
}

export enum GameStatus {
       Waiting = 1,
       Started,
       InProgress,
       RoundEnded,
       Ended
}

export interface PlayerDto{
  id?: string | null;
  connectionId?: string | null;
  name?: string | null;
}