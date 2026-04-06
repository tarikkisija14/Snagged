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
    (event.target as HTMLImageElement).src = 'assets/default-avatar.png';
  }
}
