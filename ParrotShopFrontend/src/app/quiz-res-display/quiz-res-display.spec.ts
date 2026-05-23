import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizResDisplay } from './quiz-res-display';

describe('QuizResDisplay', () => {
  let component: QuizResDisplay;
  let fixture: ComponentFixture<QuizResDisplay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuizResDisplay],
    }).compileComponents();

    fixture = TestBed.createComponent(QuizResDisplay);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
