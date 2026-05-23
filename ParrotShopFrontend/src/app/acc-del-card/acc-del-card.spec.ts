import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccDelCard } from './acc-del-card';

describe('AccDelCard', () => {
  let component: AccDelCard;
  let fixture: ComponentFixture<AccDelCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccDelCard],
    }).compileComponents();

    fixture = TestBed.createComponent(AccDelCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
