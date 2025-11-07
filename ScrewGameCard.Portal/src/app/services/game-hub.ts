import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

export interface Player {
  id: string;
  name: string;
}

export interface Card {
  suit: 'hearts' | 'diamonds' | 'clubs' | 'spades';
  value: string;
  id: string;
}

export interface GameState {
  players: Player[];
  gameStarted: boolean;
  hostId?: string;
  currentPlayerId?: string;
  currentCard?: Card;
  deck?: Card[];
}

// Server-to-Client Methods
export interface IServerMethods {
  PlayerJoined: (player: Player, isHost: boolean) => void;
  PlayerLeft: (playerId: string, newHostId?: string) => void;
  GameStarted: () => void;
  UpdateGameState: (gameState: GameState) => void;
  DealCards: (cards: Card[]) => void;
  CardPlayed: (playerId: string, card: Card) => void;
  GameOver: (winnerId: string) => void;
  Error: (message: string) => void;
}

// Client-to-Server Methods
export interface IClientMethods {
  JoinGame: (playerName: string) => Promise<Player>;
  LeaveGame: () => Promise<void>;
  StartGame: () => Promise<void>;
  PlayCard: (cardId: string) => Promise<void>;
}

// Type-safe hub connection wrapper
class TypedHubConnection {
  constructor(private connection: HubConnection) {}

  public on<K extends keyof IServerMethods>(
    methodName: K,
    handler: (...args: Parameters<IServerMethods[K]>) => void
  ): void {
    this.connection.on(methodName, handler);
  }

  public invoke<K extends keyof IClientMethods>(
    methodName: K,
    ...args: Parameters<IClientMethods[K]>
  ): ReturnType<IClientMethods[K]> {
    return this.connection.invoke(methodName, ...args);
  }

  public start(): Promise<void> {
    return this.connection.start();
  }

  public stop(): Promise<void> {
    return this.connection.stop();
  }
}

@Injectable({
  providedIn: 'root',
})
export class GameHub {
  private hub: TypedHubConnection;
  public gameState = signal<GameState>({ 
    players: [], 
    gameStarted: false 
  });
  public currentPlayerCards = signal<Card[]>([]);
  public error = signal<string | null>(null);

  constructor() {
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:7209/gamehub')
      .withAutomaticReconnect()
      .build();

    this.hub = new TypedHubConnection(connection);
    this.setupConnectionHandlers();
  }

  private setupConnectionHandlers() {
    this.hub.on('PlayerJoined', (player, isHost) => {
      const currentState = this.gameState();
      this.gameState.set({
        ...currentState,
        players: [...currentState.players, player],
        hostId: isHost ? player.id : currentState.hostId
      });
    });

    this.hub.on('PlayerLeft', (playerId, newHostId) => {
      const currentState = this.gameState();
      this.gameState.set({
        ...currentState,
        players: currentState.players.filter(p => p.id !== playerId),
        hostId: newHostId || currentState.hostId
      });
    });

    this.hub.on('GameStarted', () => {
      const currentState = this.gameState();
      this.gameState.set({
        ...currentState,
        gameStarted: true
      });
    });

    this.hub.on('UpdateGameState', (newState) => {
      this.gameState.set(newState);
    });

    this.hub.on('DealCards', (cards) => {
      this.currentPlayerCards.set(cards);
    });

    this.hub.on('CardPlayed', (playerId, card) => {
      const currentState = this.gameState();
      this.gameState.set({
        ...currentState,
        currentPlayerId: playerId,
        currentCard: card
      });
    });

    this.hub.on('Error', (message) => {
      this.error.set(message);
      setTimeout(() => this.error.set(null), 5000);
    });
  }

  async startConnection(): Promise<void> {
    try {
      await this.hub.start();
      console.log('Connected to game hub');
    } catch (error) {
      console.error('Error connecting to game hub:', error);
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  async joinGame(playerName: string): Promise<Player> {
    return await this.hub.invoke('JoinGame', playerName);
  }

  async leaveGame(): Promise<void> {
    await this.hub.invoke('LeaveGame');
  }

  async startGame(): Promise<void> {
    await this.hub.invoke('StartGame');
  }

  async playCard(cardId: string): Promise<void> {
    await this.hub.invoke('PlayCard', cardId);
  }

  disconnect(): Promise<void> {
    return this.hub.stop();
  }

  isHost(playerId: string): boolean {
    return this.gameState().hostId === playerId;
  }

  isCurrentPlayer(playerId: string): boolean {
    return this.gameState().currentPlayerId === playerId;
  }
}
}
}
