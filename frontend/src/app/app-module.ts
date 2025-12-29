import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { Header } from './layouts/header/header';
import { Home } from './pages/home/home';
import { CatalogList } from './layouts/catalog-list/catalog-list';
import {FormsModule} from "@angular/forms";
import { Shop } from './pages/shop/shop';
import { RouterModule } from '@angular/router';
import { Cart } from './pages/cart/cart';
import {MatIcon} from '@angular/material/icon';
import { Payment } from './pages/payment/payment';
import { PaymentSuccess } from './layouts/payment-success/payment-success';


@NgModule({
  declarations: [
    App,
    Header,
    Home,
    CatalogList,
    Shop,
    Cart,
    Payment,
    PaymentSuccess,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    RouterModule,
    MatIcon
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),

    //replacement for deprecated HttpClientModule
    provideHttpClient(
      withFetch(),
      withInterceptorsFromDi()
    )
  ],
  bootstrap: [App]
})
export class AppModule { }
