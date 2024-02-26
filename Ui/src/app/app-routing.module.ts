import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SuccessComponent} from "./success/success.component";
import {CancelComponent} from "./cancel/cancel.component";
import {PaymentComponent} from "./payment/payment.component";

const routes: Routes = [
  {path: '', component: PaymentComponent},
  {path: 'success', component: SuccessComponent},
  {path: 'cancel', component: CancelComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
