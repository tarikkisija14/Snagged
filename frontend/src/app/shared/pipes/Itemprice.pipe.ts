import { Pipe, PipeTransform } from '@angular/core';


@Pipe({ name: 'itemPrice',standalone:false  })
export class ItemPricePipe implements PipeTransform {
  transform(value: number | null | undefined, symbol = '$'): string {
    if (value == null || isNaN(value)) return `${symbol}0.00`;
    return `${symbol}${value.toFixed(2)}`;
  }
}
