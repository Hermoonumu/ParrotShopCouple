import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParrotTraits } from './parrot-traits';

describe('ParrotTraits', () => {
  let component: ParrotTraits;
  let fixture: ComponentFixture<ParrotTraits>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParrotTraits],
    }).compileComponents();

    fixture = TestBed.createComponent(ParrotTraits);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
