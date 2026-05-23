import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParrotTraitShow } from './parrot-trait-show';

describe('ParrotTraitShow', () => {
  let component: ParrotTraitShow;
  let fixture: ComponentFixture<ParrotTraitShow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParrotTraitShow],
    }).compileComponents();

    fixture = TestBed.createComponent(ParrotTraitShow);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
