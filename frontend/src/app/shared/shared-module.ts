import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductCardComponent } from './components/product-card.component/product-card.component';
import { LazyLoadImageDirective } from './directives/lazy-load-image.directive';

@NgModule({
  declarations: [
    LazyLoadImageDirective,
    ProductCardComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    LazyLoadImageDirective,
    ProductCardComponent,
    CommonModule
  ]
})
export class SharedModule { }
