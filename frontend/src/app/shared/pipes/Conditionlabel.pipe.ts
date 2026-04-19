import { Pipe, PipeTransform } from '@angular/core';


const CONDITION_MAP: Record<string, { label: string; slug: string }> = {
  NEW:       { label: 'New',        slug: 'new' },
  LIKE_NEW:  { label: 'Like New',   slug: 'like-new' },
  GOOD:      { label: 'Good',       slug: 'good' },
  FAIR:      { label: 'Fair',       slug: 'fair' },
  POOR:      { label: 'Poor',       slug: 'poor' },

  new:       { label: 'New',        slug: 'new' },
  like_new:  { label: 'Like New',   slug: 'like-new' },
  good:      { label: 'Good',       slug: 'good' },
  fair:      { label: 'Fair',       slug: 'fair' },
  poor:      { label: 'Poor',       slug: 'poor' },
};

@Pipe({ name: 'conditionLabel',standalone:false  })
export class ConditionLabelPipe implements PipeTransform {
  transform(value: string | null | undefined, format: 'label' | 'slug' = 'label'): string {
    if (!value) return '';
    const key = value.trim().toUpperCase().replace(/\s+/g, '_');
    const entry = CONDITION_MAP[key] ?? CONDITION_MAP[value.trim()];
    if (!entry) {

      return format === 'slug'
        ? value.toLowerCase().replace(/\s+/g, '-')
        : value.charAt(0).toUpperCase() + value.slice(1).toLowerCase();
    }
    return format === 'slug' ? entry.slug : entry.label;
  }
}
