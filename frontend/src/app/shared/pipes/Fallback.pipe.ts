import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'fallback',standalone:false  })
export class FallbackPipe implements PipeTransform {
  transform(value: string | null | undefined, placeholder: string): string {
    return value?.trim() ? value : placeholder;
  }
}
