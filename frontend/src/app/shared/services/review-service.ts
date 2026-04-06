import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { PagedReviews, Review, ReviewSortOrder } from '../models/review';

export interface AddReviewDto {
  reviewedUserId: number;
  rating: number;
  comment: string;
}

export interface UpdateReviewDto {
  rating: number;
  comment: string;
}

@Injectable({ providedIn: 'root' })
export class ReviewService {
  private readonly apiUrl = `${environment.apiUrl}/review`;

  constructor(private http: HttpClient) {}

  addReview(cmd: AddReviewDto): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.apiUrl, cmd);
  }

  updateReview(id: number, cmd: UpdateReviewDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, cmd);
  }

  deleteReview(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getReviewById(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.apiUrl}/${id}`);
  }

  getPagedReviews(
    reviewedUserId: number,
    page = 1,
    pageSize = 10,
    sortOrder: ReviewSortOrder = 'Newest'
  ): Observable<PagedReviews> {

    const params = new HttpParams()
      .set('Page', page.toString())
      .set('PageSize', pageSize.toString())
      .set('sortOrder', sortOrder);

    return this.http.get<PagedReviews>(
      `${this.apiUrl}/user/${reviewedUserId}/paged`,
      { params }
    );
  }

  getMyReviewForUser(reviewedUserId: number): Observable<Review | null> {
    return this.http
      .get<Review>(`${this.apiUrl}/my/${reviewedUserId}`, { observe: 'response' })
      .pipe(
        map((resp) => (resp.status === 204 ? null : resp.body)),
        catchError(() => of(null))
      );
  }

  getReviewsByReviewedUser(reviewedUserId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/reviewed/${reviewedUserId}`);
  }

  getReviewsByReviewer(reviewerId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/reviewer/${reviewerId}`);
  }
}
