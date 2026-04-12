import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
} from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { TagSuggestion } from '../../models/tag-suggestion.model';
import { TagService } from '../../services/tag-service';

@Component({
  selector: 'app-tag-input',
  standalone: false,
  templateUrl: './tag-input.component.html',
  styleUrls: ['./tag-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TagInputComponent implements OnDestroy {
  @Input() tags: string[] = [];
  @Input() placeholder = 'Add a tag…';
  @Input() maxTags = 20;
  @Output() tagsChange = new EventEmitter<string[]>();

  inputValue = '';
  suggestions: TagSuggestion[] = [];
  showDropdown = false;
  activeIndex = -1;

  private readonly search$ = new Subject<string>();
  private readonly destroy$ = new Subject<void>();

  constructor(private tagService: TagService, private cdr: ChangeDetectorRef) {
    this.search$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(q =>
        q.length >= 1 ? this.tagService.getSuggestions(q) : [[] as TagSuggestion[]]
      ),
      takeUntil(this.destroy$),
    ).subscribe(results => {
      this.suggestions  = results;
      this.showDropdown = results.length > 0;
      this.activeIndex  = -1;
      this.cdr.markForCheck();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onInput(value: string): void {
    this.inputValue = value;
    this.search$.next(value.trim().toLowerCase());
  }

  onKeydown(event: KeyboardEvent): void {
    switch (event.key) {
      case 'Enter':
        event.preventDefault();
        if (this.activeIndex >= 0 && this.suggestions[this.activeIndex]) {
          this.addTag(this.suggestions[this.activeIndex].name);
        } else {
          this.addTag(this.inputValue);
        }
        break;
      case 'ArrowDown':
        event.preventDefault();
        this.activeIndex = Math.min(this.activeIndex + 1, this.suggestions.length - 1);
        this.cdr.markForCheck();
        break;
      case 'ArrowUp':
        event.preventDefault();
        this.activeIndex = Math.max(this.activeIndex - 1, -1);
        this.cdr.markForCheck();
        break;
      case 'Escape':
        this.closeDropdown();
        break;
      case ',':
      case 'Tab':
        if (this.inputValue.trim()) {
          event.preventDefault();
          this.addTag(this.inputValue);
        }
        break;
    }
  }

  selectSuggestion(name: string): void {
    this.addTag(name);
  }

  addTag(raw: string): void {
    const name = raw.trim().toLowerCase();
    if (!name) return;
    if (this.tags.some(t => t.toLowerCase() === name)) {
      this.inputValue = '';
      this.closeDropdown();
      return;
    }
    if (this.tags.length >= this.maxTags) return;

    this.tagsChange.emit([...this.tags, name]);
    this.inputValue = '';
    this.closeDropdown();
  }

  removeTag(tag: string): void {
    this.tagsChange.emit(this.tags.filter(t => t !== tag));
  }

  onBlur(): void {
    // Delay so mousedown on a suggestion fires before the dropdown closes
    setTimeout(() => this.closeDropdown(), 150);
  }

  private closeDropdown(): void {
    this.showDropdown = false;
    this.activeIndex  = -1;
    this.cdr.markForCheck();
  }
}
