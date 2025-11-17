import { Player } from "../../../core/models/player.interface";

export interface AuthResponse {
  accessToken?: string;
  refreshToken?: string;
  player?: Player;
}
