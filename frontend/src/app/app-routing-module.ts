import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Shop } from './pages/shop/shop';
import { Cart } from './pages/cart/cart';
import { authGuard } from './core/guards/auth/auth-guard';
import { Payment } from './pages/payment/payment';
import { PaymentSuccess } from './layouts/payment-success/payment-success';
import { ProfileComponent } from './pages/profile/profile.component';
import { ItemDetailComponent } from './pages/item-detail/item-detail';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'home',
    component: Home,
    children: [
      {
        path: 'auth',
        loadChildren: () =>
          import('./pages/auth/auth-module').then(m => m.AuthModule),
      },
    ],
  },
  { path: 'shop', component: Shop },
  { path: 'items/:id', component: ItemDetailComponent },
  { path: 'cart', component: Cart },
  { path: 'payment/:orderId', component: Payment, canActivate: [authGuard] },
  { path: 'payment-success', component: PaymentSuccess, canActivate: [authGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
