import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MatCardModule }            from '@angular/material/card';
import { MatButtonModule }          from '@angular/material/button';
import { MatIconModule }            from '@angular/material/icon';
import { MatFormFieldModule }       from '@angular/material/form-field';
import { MatInputModule }           from '@angular/material/input';
import { MatSelectModule }          from '@angular/material/select';
import { MatPaginatorModule }       from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule }           from '@angular/material/chips';
import { MatSnackBarModule }        from '@angular/material/snack-bar';

import { ProductCardComponent }      from './components/product-card.component/product-card.component';
import { LazyLoadImageDirective }    from './directives/lazy-load-image.directive';
import { ButtonComponent }           from './components/button.component/button.component';
import { SearchSuggestions }         from './components/search-suggestions/search-suggestions';
import { TagInputComponent }         from './components/tag-input-component/tag-input.component';
import { ImageZoomDirective }        from './directives/image-zoom.directive';
import {ImageSlideshowComponent} from './components/image-slideshow-component/image-slideshow.component';


@NgModule({
  declarations: [
    LazyLoadImageDirective,
    ProductCardComponent,
    ButtonComponent,
    SearchSuggestions,
    TagInputComponent,
    ImageZoomDirective,
    ImageSlideshowComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatSnackBarModule,
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    LazyLoadImageDirective,
    ProductCardComponent,
    ButtonComponent,
    SearchSuggestions,
    TagInputComponent,
    ImageZoomDirective,
    ImageSlideshowComponent,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatSnackBarModule,
  ],
})
export class SharedModule {}
