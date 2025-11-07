import { TestBed } from '@angular/core/testing';

import { GameHub } from './game-hub';

describe('GameHub', () => {
  let service: GameHub;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GameHub);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
