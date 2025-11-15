import { Player } from "../../../core/types/player.interface";

export interface AuthResponse {
  accessToken?: string;
  refreshToken?: string;
  player?: Player;
}
