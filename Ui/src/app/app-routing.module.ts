import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SuccessComponent} from "./success/success.component";
import {CancelComponent} from "./cancel/cancel.component";
import {PaymentComponent} from "./payment/payment.component";

// Define routes for different components
const routes: Routes = [
  {path: '', component: PaymentComponent}, // Default route to PaymentComponent
  {path: 'success', component: SuccessComponent}, // Route to SuccessComponent
  {path: 'cancel', component: CancelComponent} // Route to CancelComponent
];

@NgModule({
  // Import RouterModule with configured routes
  imports: [RouterModule.forRoot(routes)],
  // Export RouterModule to make routing available throughout the application
  exports: [RouterModule]
})
export class AppRoutingModule {
}
