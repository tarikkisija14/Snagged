import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './pages/home/home';
import {Shop} from './pages/shop/shop';
import {Cart} from './pages/cart/cart';
import {authGuard} from './core/guards/auth/auth-guard';
import{Payment} from './pages/payment/payment';
import {PaymentSuccess} from './layouts/payment-success/payment-success';
import {ProfileComponent} from './pages/profile/profile.component';


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: Home,
    children: [{path: 'auth', loadChildren: () => import('./pages/auth/auth-module').then(m => m.AuthModule)}] },
  {path:'shop', component: Shop},
  { path: 'cart', component: Cart,canActivate:[authGuard] },
  {path:'payment/:orderId', component: Payment},
  {path:'payment-success',component: PaymentSuccess,canActivate:[authGuard] },
  {path:'profile', component: ProfileComponent, canActivate:[authGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

