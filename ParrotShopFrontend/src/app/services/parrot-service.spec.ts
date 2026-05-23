import { TestBed } from '@angular/core/testing';

import { ParrotService } from './parrot-service';

describe('ParrotService', () => {
  let service: ParrotService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ParrotService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
