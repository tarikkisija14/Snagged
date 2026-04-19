import { Pipe, PipeTransform } from '@angular/core';


@Pipe({ name: 'relativeDate',standalone:false })
export class RelativeDatePipe implements PipeTransform {
  transform(value: Date | string | null | undefined): string {
    if (!value) return '';

    const date   = value instanceof Date ? value : new Date(value);
    if (isNaN(date.getTime())) return '';

    const now     = Date.now();
    const diffMs  = now - date.getTime();
    const diffSec = Math.floor(diffMs / 1000);
    const diffMin = Math.floor(diffSec / 60);
    const diffHr  = Math.floor(diffMin / 60);
    const diffDay = Math.floor(diffHr  / 24);

    if (diffSec < 60)  return 'just now';
    if (diffMin < 60)  return `${diffMin} minute${diffMin === 1 ? '' : 's'} ago`;
    if (diffHr  < 24)  return `${diffHr} hour${diffHr   === 1 ? '' : 's'} ago`;
    if (diffDay < 7)   return `${diffDay} day${diffDay   === 1 ? '' : 's'} ago`;
    if (diffDay < 30)  return `${Math.floor(diffDay / 7)} week${Math.floor(diffDay / 7) === 1 ? '' : 's'} ago`;

    // Older than 30 days — use a readable absolute date
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }
}
