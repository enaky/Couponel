import {NgModule, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile/profile.component';

import { SharedModule } from '../shared/shared.module';
import { ProfileInfoComponent } from './profile-info/profile-info.component';
import {RedeemedVoucherComponent} from './redeemed-voucher/redeemed-voucher.component';
import {RedeemedVoucherDetailsComponent} from './redeemed-voucher-details/redeemed-voucher-details.component';
import {VoucherCreateComponent} from './voucher-create/voucher-create.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AddedVouchersComponent } from './added-vouchers/added-vouchers.component';
import { AddedVoucherDetailsComponent } from './added-voucher-details/added-voucher-details.component';
import {Router} from '@angular/router';
import {UserService} from '../shared/services';

@NgModule({
  declarations: [ProfileComponent, ProfileInfoComponent, RedeemedVoucherComponent, RedeemedVoucherDetailsComponent, VoucherCreateComponent, AddedVouchersComponent, AddedVoucherDetailsComponent],
  imports: [
    CommonModule,
    ProfileRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class ProfileModule {
}
