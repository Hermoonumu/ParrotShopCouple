import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParrotShow } from './parrot-show';

describe('ParrotShow', () => {
  let component: ParrotShow;
  let fixture: ComponentFixture<ParrotShow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParrotShow],
    }).compileComponents();

    fixture = TestBed.createComponent(ParrotShow);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
