import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderMgmt } from './order-mgmt';

describe('OrderMgmt', () => {
  let component: OrderMgmt;
  let fixture: ComponentFixture<OrderMgmt>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderMgmt],
    }).compileComponents();

    fixture = TestBed.createComponent(OrderMgmt);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
