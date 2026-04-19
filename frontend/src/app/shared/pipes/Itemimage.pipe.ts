import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';

const PLACEHOLDER = 'assets/placeholder.png';

@Pipe({ name: 'itemImageUrl', standalone: false })
export class ItemImageUrlPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return PLACEHOLDER;
    if (value.startsWith('http://') || value.startsWith('https://') || value.startsWith('data:')) {
      return value;
    }
    const base = environment.apiUrl.replace(/\/api$/, '');
    return `${base}/${value.replace(/^\//, '')}`;
  }
}
