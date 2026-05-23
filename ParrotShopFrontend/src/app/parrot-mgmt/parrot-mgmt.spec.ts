import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParrotMgmt } from './parrot-mgmt';

describe('ParrotMgmt', () => {
  let component: ParrotMgmt;
  let fixture: ComponentFixture<ParrotMgmt>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParrotMgmt],
    }).compileComponents();

    fixture = TestBed.createComponent(ParrotMgmt);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
