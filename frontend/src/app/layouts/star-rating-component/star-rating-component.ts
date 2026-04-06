import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'app-star-rating',
  standalone: false,
  templateUrl: './star-rating-component.html',
  styleUrls: ['./star-rating-component.scss'],
})
export class StarRatingComponent implements OnChanges {
  @Input() rating = 0;
  @Input() readonly = true;
  @Input() showLabel = false;
  @Input() reviewCount?: number;

  @Output() ratingChange = new EventEmitter<number>();

  readonly stars = [1, 2, 3, 4, 5];
  hoverRating = 0;
  displayRating = 0;
  private _selectedRating = 0;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['rating']) {
      this.displayRating = this.rating;
    }
  }

  onHover(star: number): void {
    if (this.readonly) return;
    this.hoverRating = star;
    this.displayRating = star;
  }

  onHoverLeave(): void {
    if (this.readonly) return;
    this.hoverRating = 0;
    this.displayRating = this._selectedRating || this.rating;
  }

  onSelect(star: number): void {
    if (this.readonly) return;
    this._selectedRating = star;
    this.displayRating = star;
    this.ratingChange.emit(star);
  }
}
