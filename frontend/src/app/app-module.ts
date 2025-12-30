import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { Header } from './layouts/header/header';
import { Home } from './pages/home/home';
import { CatalogList } from './layouts/catalog-list/catalog-list';
import { FormsModule } from '@angular/forms';
import { Shop } from './pages/shop/shop';
import { RouterModule } from '@angular/router';
import { Cart } from './pages/cart/cart';
import { Payment } from './pages/payment/payment';
import { PaymentSuccess } from './layouts/payment-success/payment-success';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';

import { LayoutModule } from '@angular/cdk/layout';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

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

    // Angular Material
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatDividerModule,

    // CDK
    LayoutModule,
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideAnimationsAsync(),
    provideHttpClient(
      withFetch(),
      withInterceptorsFromDi()
    )
  ],
  bootstrap: [App]
})
export class AppModule {}
