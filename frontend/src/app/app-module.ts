import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
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
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { LayoutModule } from '@angular/cdk/layout';
import { ProfileComponent } from './pages/profile/profile.component';

import {authInterceptor} from './core/interceptors/auth-interceptor';
import {SharedModule} from './shared/shared-module';

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
    ProfileComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    RouterModule,
    MatProgressSpinnerModule,

    // Angular Material
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatDividerModule,
    MatCardModule,
    MatChipsModule,
    MatProgressSpinnerModule,

    // CDK
    LayoutModule,
    SharedModule,
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(
      withInterceptors([authInterceptor]) //pozvace authInterceptor na svaki http request od httpclienta
    )
  ],
  bootstrap: [App]
})
export class AppModule {}
