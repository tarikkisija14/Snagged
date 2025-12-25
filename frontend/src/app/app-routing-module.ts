import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './pages/home/home';
import {Shop} from './pages/shop/shop';
import {Cart} from './pages/cart/cart';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: Home,
    children: [{path: 'auth', loadChildren: () => import('./pages/auth/auth-module').then(m => m.AuthModule)}] },
  {path:'shop', component: Shop},
  { path: 'cart', component: Cart },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
