import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParrotControl } from './parrot-control';

describe('ParrotControl', () => {
  let component: ParrotControl;
  let fixture: ComponentFixture<ParrotControl>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParrotControl],
    }).compileComponents();

    fixture = TestBed.createComponent(ParrotControl);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
