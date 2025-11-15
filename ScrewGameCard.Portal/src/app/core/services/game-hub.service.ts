import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

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
  GameCreated: (game: any) => void; // From SignalRService
}

// Client-to-Server Methods
export interface IClientMethods {
  JoinGame: (playerName: string) => Promise<Player>;
  LeaveGame: () => Promise<void>;
  StartGame: () => Promise<void>;
  PlayCard: (cardId: string) => Promise<void>;
  CreateGame: (request: any) => Promise<void>; // From SignalRService
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
  ): Promise<any> {
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
export class GameHubService {
  private hub: TypedHubConnection;
  private hubConnection: HubConnection;
  private connectionIdSubject = new BehaviorSubject<string | null>(null);

  // Signals for game state
  public gameState = signal<GameState>({
    players: [],
    gameStarted: false
  });
  public currentPlayerCards = signal<Card[]>([]);
  public error = signal<string | null>(null);

  // Subjects for lobby events
  private gameCreatedSubject = new Subject<any>();
  private lobbyErrorSubject = new Subject<string>();

  public gameCreated$ = this.gameCreatedSubject.asObservable();
  public lobbyError$ = this.lobbyErrorSubject.asObservable();
  public connectionId$ = this.connectionIdSubject.asObservable();

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .configureLogging(LogLevel.Information)
      .build();

    this.hub = new TypedHubConnection(this.hubConnection);
    this.setupConnectionHandlers();
  }

  private setupConnectionHandlers() {
    // Game events
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

    this.hub.on('GameOver', (winnerId) => {
      // Handle game over
    });

    this.hub.on('Error', (message) => {
      this.error.set(message);
      setTimeout(() => this.error.set(null), 5000);
    });

    // Lobby events
    this.hub.on('GameCreated', (game) => {
      this.gameCreatedSubject.next(game);
    });
  }

  async startConnection(): Promise<void> {
    try {
      await this.hub.start();
      this.connectionIdSubject.next(this.hubConnection.connectionId);
      console.log('Connected to game hub');
    } catch (error) {
      console.error('Error connecting to game hub:', error);
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  // Game methods
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

  // Lobby methods
  async createGame(request: any): Promise<void> {
    await this.hub.invoke('CreateGame', request);
  }

  disconnect(): Promise<void> {
    return this.hub.stop();
  }

  getConnectionId(): string | null {
    return this.hubConnection.connectionId;
  }

  isHost(playerId: string): boolean {
    return this.gameState().hostId === playerId;
  }

  isCurrentPlayer(playerId: string): boolean {
    return this.gameState().currentPlayerId === playerId;
  }
}
