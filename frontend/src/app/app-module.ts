import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { Header } from './layouts/header/header';
import { Home } from './pages/home/home';
import { CatalogList } from './layouts/catalog-list/catalog-list';
import { Shop } from './pages/shop/shop';
import { Cart } from './pages/cart/cart';
import { Payment } from './pages/payment/payment';
import { PaymentSuccess } from './layouts/payment-success/payment-success';
import { ItemDetailComponent } from './pages/item-detail/item-detail';
import { ProfileComponent } from './pages/profile/profile.component';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { LayoutModule } from '@angular/cdk/layout';

import { authInterceptor } from './core/interceptors/auth-interceptor';
import { SharedModule } from './shared/shared-module';

import { register } from 'swiper/element/bundle';
import { StarRatingComponent } from './layouts/star-rating-component/star-rating-component';
import { ReviewFormComponent } from './layouts/review-form-component/review-form-component';
import { ReviewListComponent } from './layouts/review-list-component/review-list-component';
import { UserReviewsComponent } from './layouts/user-reviews-component/user-reviews-component';


register();

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
    ItemDetailComponent,
    StarRatingComponent,
    ReviewFormComponent,
    ReviewListComponent,
    UserReviewsComponent,

  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule,

    // Material modules not re-exported by SharedModule
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
  ],
  providers: [
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [App],
})
export class AppModule {}
