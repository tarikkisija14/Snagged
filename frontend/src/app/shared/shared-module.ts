import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductCardComponent } from './components/product-card.component/product-card.component';
import { LazyLoadImageDirective } from './directives/lazy-load-image.directive';
import { ButtonComponent } from './components/button.component/button.component';

@NgModule({
  declarations: [
    LazyLoadImageDirective,
    ProductCardComponent,
    ButtonComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    LazyLoadImageDirective,
    ProductCardComponent,
    CommonModule,
    ButtonComponent
  ]
})
export class SharedModule { }
