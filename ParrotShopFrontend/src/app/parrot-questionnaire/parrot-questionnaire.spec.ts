import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParrotQuestionnaire } from './parrot-questionnaire';

describe('ParrotQuestionnaire', () => {
  let component: ParrotQuestionnaire;
  let fixture: ComponentFixture<ParrotQuestionnaire>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParrotQuestionnaire],
    }).compileComponents();

    fixture = TestBed.createComponent(ParrotQuestionnaire);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
