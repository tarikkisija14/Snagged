import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Review, ReviewSortOrder } from '../../shared/models/review';

@Component({
  selector: 'app-review-list',
  standalone: false,
  templateUrl: './review-list-component.html',
  styleUrls: ['./review-list-component.scss'],
})
export class ReviewListComponent implements OnChanges {
  @Input() reviews: Review[] = [];
  @Input() total = 0;
  @Input() page = 1;
  @Input() pageSize = 10;
  @Input() isLoading = false;
  @Input() currentUserId: number | null = null;
  @Input() currentSort: ReviewSortOrder = 'Newest';

  @Output() sortChange   = new EventEmitter<ReviewSortOrder>();
  @Output() pageChange   = new EventEmitter<{ page: number; pageSize: number }>();
  @Output() editReview   = new EventEmitter<Review>();
  @Output() deleteReview = new EventEmitter<Review>();

  sortValue: ReviewSortOrder = 'Newest';

  // Inline SVG data URI — no external file required.
  readonly defaultAvatar =
    'data:image/svg+xml,%3Csvg xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22 viewBox%3D%220 0 40 40%22%3E%3Ccircle cx%3D%2220%22 cy%3D%2220%22 r%3D%2220%22 fill%3D%22%23e0e0e0%22%2F%3E%3Ccircle cx%3D%2220%22 cy%3D%2215%22 r%3D%226%22 fill%3D%22%23bdbdbd%22%2F%3E%3Cellipse cx%3D%2220%22 cy%3D%2232%22 rx%3D%2210%22 ry%3D%228%22 fill%3D%22%23bdbdbd%22%2F%3E%3C%2Fsvg%3E';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentSort']) {
      this.sortValue = this.currentSort;
    }
  }

  onSortChange(value: ReviewSortOrder): void {
    this.sortValue = value;
    this.sortChange.emit(value);
  }

  isEdited(review: Review): boolean {
    return new Date(review.updatedAt).getTime() - new Date(review.createdAt).getTime() > 5000;
  }

  onPage(event: PageEvent): void {
    this.pageChange.emit({ page: event.pageIndex + 1, pageSize: event.pageSize });
  }

  onDelete(review: Review): void {
    if (confirm('Delete your review? This cannot be undone.')) {
      this.deleteReview.emit(review);
    }
  }

  onAvatarError(event: Event): void {
    const img = event.target as HTMLImageElement;

    if (!img.src.startsWith('data:')) {
      img.src = this.defaultAvatar;
    }
  }
}
