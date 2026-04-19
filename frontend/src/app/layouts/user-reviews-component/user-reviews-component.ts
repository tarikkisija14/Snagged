import {
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../../core/services/auth-service/AuthService';
import { Review, ReviewSortOrder } from '../../shared/models/review';
import {
  AddReviewDto,
  UpdateReviewDto,
  ReviewService,
} from '../../shared/services/review-service';
import { ReviewSubmitEvent } from '../review-form-component/review-form-component';

@Component({
  selector: 'app-user-reviews',
  standalone: false,
  templateUrl: './user-reviews-component.html',
  styleUrls: ['./user-reviews-component.scss'],
})
export class UserReviewsComponent implements OnInit, OnChanges, OnDestroy {
  @Input() reviewedUserId!: number;
  @Input() averageRatingInput: number = 0;
  @Input() reviewCountInput: number = 0;

  private destroy$ = new Subject<void>();

  reviews: Review[] = [];
  myReview: Review | null = null;
  averageRating = 0;
  totalReviews = 0;

  page = 1;
  pageSize = 10;
  sortOrder: ReviewSortOrder = 'Newest';

  showForm = false;
  isSubmitting = false;
  isLoadingReviews = false;

  errorMessage = '';
  successMessage = '';

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get currentUserId(): number | null {
    return this.authService.getUserId();
  }

  constructor(
    private reviewService: ReviewService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.syncInputRating();
    this.initLoad();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['averageRatingInput'] || changes['reviewCountInput']) {
      this.syncInputRating();
    }
    if (changes['reviewedUserId'] && !changes['reviewedUserId'].firstChange) {
      this.reset();
      this.syncInputRating();
      this.initLoad();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private syncInputRating(): void {
    if (this.averageRating === 0) {
      this.averageRating = this.averageRatingInput;
    }
    if (this.totalReviews === 0) {
      this.totalReviews = this.reviewCountInput;
    }
  }

  private initLoad(): void {
    this.loadReviews();
    if (this.isLoggedIn && this.currentUserId !== this.reviewedUserId) {
      this.loadMyReview();
    }
  }

  private reset(): void {
    this.page = 1;
    this.sortOrder = 'Newest';
    this.reviews = [];
    this.myReview = null;
    this.averageRating = 0;
    this.totalReviews = 0;
    this.showForm = false;
    this.errorMessage = '';
    this.successMessage = '';
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    this.cdr.markForCheck();
    setTimeout(() => { this.successMessage = ''; this.cdr.markForCheck(); }, 4000);
  }

  private handleError(err: unknown): void {
    this.isSubmitting = false;
    const e = err as any;
    const serverMsg: string | undefined = e?.error?.message;
    const validationErrors: string | null = e?.error?.errors
      ? (Object.values(e.error.errors) as string[][]).flat().join(' ')
      : null;
    this.errorMessage =
      serverMsg ?? validationErrors ?? 'Something went wrong. Please try again.';
    this.cdr.markForCheck();
    setTimeout(() => { this.errorMessage = ''; this.cdr.markForCheck(); }, 6000);
  }

  loadReviews(): void {
    this.isLoadingReviews = true;
    this.cdr.markForCheck();

    // Use real server-side pagination — do NOT fetch 1000 rows and slice client-side.
    this.reviewService
      .getPagedReviews(this.reviewedUserId, this.page, this.pageSize, this.sortOrder)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          const pageReviews = result.items ?? [];
          this.totalReviews = result.total;

          // Recompute average from the current page's items as a best-effort display.
          // The authoritative value is stored on the server Profile (AverageRating).
          if (pageReviews.length > 0) {
            const sum = pageReviews.reduce((acc, r) => acc + r.rating, 0);
            this.averageRating = Math.round((sum / pageReviews.length) * 10) / 10;
          } else if (result.total === 0) {
            this.averageRating = 0;
          }

          this.reviews = pageReviews;
          this.isLoadingReviews = false;
          this.cdr.markForCheck();
        },
        error: (err: unknown) => {
          this.isLoadingReviews = false;
          this.handleError(err);
        },
      });
  }

  private loadMyReview(): void {
    this.reviewService
      .getMyReviewForUser(this.reviewedUserId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (r) => { this.myReview = r; this.cdr.markForCheck(); },
        error: () => {},
      });
  }

  openForm(): void {
    this.errorMessage = '';
    this.showForm = true;
    this.cdr.markForCheck();
  }

  onSubmit(event: ReviewSubmitEvent): void {
    this.isSubmitting = true;
    this.errorMessage = '';
    this.cdr.markForCheck();

    const obs: Observable<any> = this.myReview
      ? this.reviewService.updateReview(this.myReview.id, event as UpdateReviewDto)
      : this.reviewService.addReview({
        reviewedUserId: this.reviewedUserId,
        ...event,
      } as AddReviewDto);

    obs.pipe(takeUntil(this.destroy$)).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.showForm = false;
        this.showSuccess(
          this.myReview ? 'Review updated successfully.' : 'Review submitted successfully.'
        );
        this.loadReviews();
        this.loadMyReview();
      },
      error: (err: unknown) => this.handleError(err),
    });
  }

  onDeleteMyReview(): void {
    if (!this.myReview) return;
    this.isSubmitting = true;
    this.cdr.markForCheck();
    this.reviewService
      .deleteReview(this.myReview.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.myReview = null;
          this.isSubmitting = false;
          this.showForm = false;
          this.showSuccess('Your review has been deleted.');
          this.loadReviews();
        },
        error: (err: unknown) => this.handleError(err),
      });
  }

  onEditFromList(review: Review): void {
    if (review.reviewerId !== this.currentUserId) return;
    this.myReview = review;
    this.showForm = true;
    this.cdr.markForCheck();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onDeleteFromList(review: Review): void {
    if (review.reviewerId !== this.currentUserId) return;
    this.isSubmitting = true;
    this.cdr.markForCheck();
    this.reviewService
      .deleteReview(review.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          if (this.myReview?.id === review.id) this.myReview = null;
          this.isSubmitting = false;
          this.showSuccess('Review deleted.');
          this.loadReviews();
        },
        error: (err: unknown) => this.handleError(err),
      });
  }

  onSortChange(sort: ReviewSortOrder): void {
    this.sortOrder = sort;
    this.page = 1;
    this.loadReviews();
  }

  onPageChange(event: { page: number; pageSize: number }): void {
    this.page = event.page;
    this.pageSize = event.pageSize;
    this.loadReviews();
  }
}
