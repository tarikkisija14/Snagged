import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Review } from '../../shared/models/review';

export interface ReviewSubmitEvent {
  rating: number;
  comment: string;
}

@Component({
  selector: 'app-review-form',
  standalone: false,
  templateUrl: './review-form-component.html',
  styleUrls: ['./review-form-component.scss']
})
export class ReviewFormComponent implements OnChanges {
  @Input() existingReview: Review | null = null;
  @Input() isSubmitting = false;

  @Output() submitReview = new EventEmitter<ReviewSubmitEvent>();
  @Output() deleteReview = new EventEmitter<void>();
  @Output() cancelForm   = new EventEmitter<void>();

  selectedRating = 0;
  comment = '';
  submitted = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['existingReview']) {
      if (this.existingReview) {
        this.selectedRating = this.existingReview.rating;
        this.comment        = this.existingReview.comment;
      } else {
        this.selectedRating = 0;
        this.comment        = '';
      }
      this.submitted = false;
    }
  }

  submit(): void {
    this.submitted = true;
    if (this.selectedRating === 0 || this.comment.trim().length < 3) return;

    this.submitReview.emit({
      rating:  this.selectedRating,
      comment: this.comment.trim(),
    });
  }

  requestDelete(): void {
    if (confirm('Are you sure you want to delete your review? This cannot be undone.')) {
      this.deleteReview.emit();
    }
  }

  cancel(): void {
    this.submitted = false;
    if (this.existingReview) {
      this.selectedRating = this.existingReview.rating;
      this.comment        = this.existingReview.comment;
    } else {
      this.selectedRating = 0;
      this.comment        = '';
    }
    this.cancelForm.emit();
  }
}
