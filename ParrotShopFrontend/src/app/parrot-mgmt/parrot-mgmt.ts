import { Component, inject, signal } from '@angular/core';
import { ParrotTraits } from '../parrot-traits/parrot-traits';
import { OrderMgmt } from '../order-mgmt/order-mgmt';
import { UserMgmt } from '../user-mgmt/user-mgmt';
import { ParrotControl } from '../parrot-control/parrot-control';
import { Parrot, ParrotService } from '../services/parrot-service';

@Component({
  selector: 'app-parrot-mgmt',
  imports: [ParrotControl, ParrotTraits, OrderMgmt, UserMgmt],
  templateUrl: './parrot-mgmt.html',
  styleUrl: './parrot-mgmt.css',
})
export class ParrotMgmt {
  section = signal("manp");
  onChange(e : Event){
    this.section.set((e.target as HTMLSelectElement).value);
  }

  onBtnClick(s: string){
    this.section.set(s);
  }

}
