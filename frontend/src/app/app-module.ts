import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { Header } from './layouts/header/header';
import { Home } from './pages/home/home';

@NgModule({
  declarations: [
    App,
    Header,
    Home
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
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
